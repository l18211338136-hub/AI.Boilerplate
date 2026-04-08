using System.Threading.Channels;
using AI.Boilerplate.Shared.Features.Chatbot;
using AI.Boilerplate.Shared.Features.Identity.Dtos;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace AI.Boilerplate.Client.Core.Components.Layout;

public partial class AppAiChatPanel
{
    [CascadingParameter] public BitDir? CurrentDir { get; set; }

    [CascadingParameter] public AppThemeType? CurrentTheme { get; set; }

    [CascadingParameter] public UserDto? CurrentUser { get; set; }


    [AutoInject] private HubConnection hubConnection = default!;
    [AutoInject] private IJSRuntime jsRuntime = default!;
    [AutoInject] private ILogger<AppAiChatPanel> logger = default!;


    private bool isOpen;
    private bool isLoading;
    private string? userInput;
    private bool isSmallScreen;
    private int responseCounter;
    private Channel<AiChatMessage>? channel;
    private AiChatMessage? lastAssistantMessage;
    private List<AiChatMessage> chatMessages = []; // TODO: Persist these values in client-side storage to retain them across app restarts.
    private List<string> followUpSuggestions = [];
    private Action unsubAdHaveTrouble = default!;

    private bool isDashboardModalOpen;
    private string dashboardHtml = string.Empty;

    // Pending attachments state
    private List<AiChatAttachment> pendingAttachments = new();

    private async Task HandleUploadClick()
    {
        await jsRuntime.InvokeVoidAsync("eval", "document.getElementById('chat-file-upload').click()");
    }

    private async Task HandleFileUpload(InputFileChangeEventArgs e)
    {
        try
        {
            var files = e.GetMultipleFiles();
            if (files == null || files.Count == 0) return;

            foreach (var file in files)
            {
                // Limit file size to 10MB
                if (file.Size > 1024 * 1024 * 10) 
                {
                    userInput = Localizer["AppAiChatPanelFileIsTooLarge", file.Name];
                    continue;
                }

                using var stream = file.OpenReadStream(1024 * 1024 * 10);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream, CurrentCancellationToken);
                var bytes = memoryStream.ToArray();
                
                var mimeType = file.ContentType;
                if (string.IsNullOrEmpty(mimeType))
                {
                    mimeType = "application/octet-stream";
                }

                pendingAttachments.Add(new AiChatAttachment
                {
                    Base64Data = Convert.ToBase64String(bytes),
                    MimeType = mimeType,
                    FileName = file.Name
                });
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading file");
            userInput = Localizer["AppAiChatPanelErrorReadingFile"];
            StateHasChanged();
        }
    }

    private void RemovePendingAttachment(AiChatAttachment attachment)
    {
        pendingAttachments.Remove(attachment);
        StateHasChanged();
    }

    private bool ExtractHtmlDashboard(string? content, out string html)
    {
        html = string.Empty;
        if (string.IsNullOrWhiteSpace(content)) return false;

        var startTag = "```html";
        var endTag = "```";
        var startIndex = content.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
        if (startIndex >= 0)
        {
            var codeStartIndex = startIndex + startTag.Length;
            var endIndex = content.IndexOf(endTag, codeStartIndex, StringComparison.OrdinalIgnoreCase);
            if (endIndex > codeStartIndex)
            {
                html = content.Substring(codeStartIndex, endIndex - codeStartIndex).Trim();
                return true;
            }
        }
        return false;
    }

    private string RemoveHtmlDashboard(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return content;
        
        var startTag = "```html";
        var endTag = "```";
        var startIndex = content.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
        if (startIndex >= 0)
        {
            var codeStartIndex = startIndex + startTag.Length;
            var endIndex = content.IndexOf(endTag, codeStartIndex, StringComparison.OrdinalIgnoreCase);
            if (endIndex > codeStartIndex)
            {
                return content.Remove(startIndex, endIndex + endTag.Length - startIndex).Trim();
            }
        }
        return content;
    }

    private void OpenDashboard(string html)
    {
        dashboardHtml = html;
        isDashboardModalOpen = true;
    }

    protected override Task OnInitAsync()
    {

        unsubAdHaveTrouble = PubSubService.Subscribe(ClientAppMessages.AD_HAVE_TROUBLE, async _ =>
        {
            if (isOpen) return;

            isOpen = true;

            StateHasChanged();

            var message = Localizer[nameof(AppStrings.UpgradeAdHaveTroublePrompt)];

            await SendPromptMessage(message);
        });

        return base.OnInitAsync();
    }

    protected override async Task OnAfterFirstRenderAsync()
    {
        SetDefaultValues();
        StateHasChanged();
        hubConnection.Reconnected += HubConnection_Reconnected;

        await base.OnAfterFirstRenderAsync();
    }


    private async Task HubConnection_Reconnected(string? _)
    {
        if (channel is null) return;
        await RestartChannel();
    }

    private async Task SendPromptMessage(string message)
    {
        followUpSuggestions = [];
        userInput = message;
        StateHasChanged();
        await SendMessage();
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(userInput) && !pendingAttachments.Any())
        {
            return;
        }

        if (channel is null)
        {
            _ = StartChannel();
        }

        isLoading = true;

        var input = userInput;
        userInput = string.Empty;

        var message = new AiChatMessage 
        { 
            Content = input, 
            Role = AiChatMessageRole.User,
            Attachments = new List<AiChatAttachment>(pendingAttachments)
        };
        
        // Clear pending attachment state
        pendingAttachments.Clear();

        chatMessages.Add(message);
        lastAssistantMessage = new() { Role = AiChatMessageRole.Assistant };
        chatMessages.Add(lastAssistantMessage);

        StateHasChanged();

        await channel!.Writer.WriteAsync(message, CurrentCancellationToken);
    }

    private async Task ClearChat()
    {
        SetDefaultValues();

        await RestartChannel();
    }

    private void SetDefaultValues()
    {
        isLoading = false;
        responseCounter = 0;
        followUpSuggestions = [];
        lastAssistantMessage = new() { Role = AiChatMessageRole.Assistant };
        chatMessages = [
            new()
            {
                Role = AiChatMessageRole.Assistant,
                Content = Localizer[nameof(AppStrings.AiChatPanelInitialResponse), string.IsNullOrEmpty(CurrentUser?.DisplayName) ? string.Empty : $" {CurrentUser.DisplayName}"],
            }
        ];
    }

    private async Task HandleOnDismissPanel()
    {
        await StopChannel();
    }

    private async Task HandleOnUserInputEnter(KeyboardEventArgs e)
    {
        if (e.ShiftKey) return;

        await SendMessage();
    }

    private async Task StartChannel()
    {
        channel = Channel.CreateUnbounded<AiChatMessage>(new() { SingleReader = true, SingleWriter = true });

        // The following code streams user's input messages to the server and processes the streamed responses.
        // It keeps the chat ongoing until CurrentCancellationToken is cancelled.
        await foreach (var response in hubConnection.StreamAsync<string>(SharedAppMessages.StartChat,
                                                                         new StartChatRequest()
                                                                         {
                                                                             CultureId = CultureInfo.CurrentCulture.LCID,
                                                                             TimeZoneId = TimeZoneInfo.Local.Id,
                                                                             DeviceInfo = TelemetryContext.Platform,
                                                                             ChatMessagesHistory = chatMessages,
                                                                             ServerApiAddress = AbsoluteServerAddress.GetAddress()
                                                                         },
                                                                         channel.Reader.ReadAllAsync(CurrentCancellationToken),
                                                                         cancellationToken: CurrentCancellationToken))
        {
            int expectedResponsesCount = chatMessages.Count(c => c.Role is AiChatMessageRole.User);

            if (response.Contains(nameof(AiChatFollowUpList.FollowUpSuggestions)))
            {
                followUpSuggestions = JsonSerializer.Deserialize<AiChatFollowUpList>(response)?.FollowUpSuggestions ?? [];
            }
            else
            {
                if (response is SharedAppMessages.MESSAGE_PROCESS_SUCCESS)
                {
                    responseCounter++;
                    isLoading = false;
                }
                else if (response is SharedAppMessages.MESSAGE_PROCESS_ERROR)
                {
                    responseCounter++;
                    if (responseCounter == expectedResponsesCount)
                    {
                        isLoading = false; // Hide loading only if this is an error for the last user's message.
                    }
                    chatMessages[responseCounter * 2].Successful = false;
                }
                else
                {
                    if ((responseCounter + 1) == expectedResponsesCount)
                    {
                        lastAssistantMessage!.Content += response;
                    }
                }
            }

            StateHasChanged();
        }
    }

    private async Task StopChannel()
    {
        if (channel is null) return;

        channel.Writer.Complete();
        channel = null;
    }

    private async Task RestartChannel()
    {
        await StopChannel();
        await StartChannel();
    }


    protected override async ValueTask DisposeAsync(bool disposing)
    {

        unsubAdHaveTrouble();

        hubConnection.Reconnected -= HubConnection_Reconnected;

        await StopChannel();

        await base.DisposeAsync(disposing);
    }
}

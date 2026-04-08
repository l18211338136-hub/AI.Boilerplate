namespace AI.Boilerplate.Shared.Features.Chatbot;

public class StartChatRequest
{
    public int? CultureId { get; set; }

    public string? DeviceInfo { get; set; }

    public string? TimeZoneId { get; set; }

    /// <summary>
    /// On chat restart (e.g., SignalR reconnection or chat panel close), 
    /// Server's AppHub releases chat related resources including chat history. 
    /// When the chat panel is reopened, the client must resend the chat history to the server.
    /// </summary>
    public List<AiChatMessage> ChatMessagesHistory { get; set; } = [];

    public Uri? ServerApiAddress { get; set; } // Getting the api address in ChatBot Hub has some complexities, specially when using Azure SignalR or being behind a reverse proxy, so we pass it from the client side.
}

public enum AiChatMessageRole
{
    User,
    Assistant
}

public class AiChatAttachment
{
    public string? Base64Data { get; set; }
    public string? MimeType { get; set; }
    public string? FileName { get; set; }
}

public class AiChatMessage
{
    public AiChatMessageRole Role { get; set; }
    public string? Content { get; set; }

    /// <summary>
    /// Attachments like images or documents.
    /// </summary>
    public List<AiChatAttachment> Attachments { get; set; } = new();

    /// <summary>
    /// Base64 encoded image data or other file data.
    /// </summary>
    [Obsolete("Use Attachments instead")]
    public string? Base64Data { get; set; }

    /// <summary>
    /// MIME type of the data (e.g. image/png, image/jpeg).
    /// </summary>
    [Obsolete("Use Attachments instead")]
    public string? MimeType { get; set; }

    [JsonIgnore]
    public bool Successful { get; set; } = true;
}

public class AiChatFollowUpList
{
    public List<string> FollowUpSuggestions { get; set; } = [];
}

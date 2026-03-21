using System.ComponentModel;
using ModelContextProtocol.Server;
using Microsoft.AspNetCore.SignalR;
using AI.Boilerplate.Shared.Features.Diagnostic;
using AI.Boilerplate.Server.Api.Features.Identity;
using AI.Boilerplate.Shared.Features.Identity.Dtos;
using AI.Boilerplate.Server.Api.Infrastructure.Services;

namespace AI.Boilerplate.Server.Api.Infrastructure.SignalR;

[McpServerToolType]
public partial class AppChatbot
{
    /// <summary>
    /// 返回基于用户时区的当前日期和时间。
    /// </summary>
    [Description("返回基于用户所在时区的当前日期和时间。")]
    [McpServerTool(Name = nameof(GetCurrentDateTime))]
    private string GetCurrentDateTime([Required, Description("用户的时区ID (例如: Asia/Shanghai)")] string timeZoneId)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(GetCurrentDateTime)} (timeZoneId: {timeZoneId})");
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            var userDateTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

            return $"Current date/time in user's timezone ({timeZoneId}) is {userDateTime:o}";
        }
        catch
        {
            return $"Current date/time in utc is {DateTimeOffset.UtcNow:o}";
        }
    }

    /// <summary>
    /// 保存用户的电子邮箱地址和对话历史以供日后参考。
    /// </summary>
    [Description("保存用户的电子邮箱地址和对话历史以供日后参考或问题排查。")]
    [McpServerTool(Name = nameof(SaveUserEmailAndConversationHistory))]
    private async Task<string?> SaveUserEmailAndConversationHistory(
        [Required, Description("用户的电子邮箱地址")] string emailAddress,
        [Required, Description("完整的对话历史记录")] string conversationHistory)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(SaveUserEmailAndConversationHistory)} (emailAddress: {emailAddress})");
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();

            // Ideally, store these in a CRM or app database,
            // but for now, we'll log them!
            scope.ServiceProvider.GetRequiredService<ILogger<IChatClient>>()
                .LogError("Chat reported issue: User email: {emailAddress}, Conversation history: {conversationHistory}", emailAddress, conversationHistory);

            return "User email and conversation history saved successfully.";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to save user email and conversation history.";
        }
    }

    /// <summary>
    /// 将用户导航到应用程序中的特定页面。
    /// </summary>
    [Description("将用户导航到应用程序中的特定页面。当用户要求前往应用程序的某个特定部分或功能时，请使用此工具。")]
    [McpServerTool(Name = nameof(NavigateToPage))]
    private async Task<string?> NavigateToPage(
        [Required, Description("要导航到的目标页面URL")] string pageUrl)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(NavigateToPage)} (pageUrl: {pageUrl})");
        await EnsureSignalRConnectionIdIsPresent();

        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
                .Clients.Client(signalRConnectionId!)
                .InvokeAsync<bool>(SharedAppMessages.NAVIGATE_TO, pageUrl, CancellationToken.None);

            return "Navigation completed";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Navigation failed";
        }
    }

    [Description("向用户显示登录弹窗，并等待登录成功或取消操作。")]
    [McpServerTool(Name = nameof(ShowSignInModal))]
    public async Task<UserDto?> ShowSignInModal()
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(ShowSignInModal)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            await EnsureSignalRConnectionIdIsPresent();

            var accessToken = await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
                .Clients.Client(signalRConnectionId!)
                .InvokeAsync<string>(SharedAppMessages.SHOW_SIGN_IN_MODAL, CancellationToken.None);

            var bearerTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).BearerTokenProtector;
            var accessTokenTicket = bearerTokenProtector.Unprotect(accessToken);
            var user = accessTokenTicket!.Principal;

            return await scope.ServiceProvider.GetRequiredService<AppDbContext>()
                .Users
                .Project()
                .FirstOrDefaultAsync(u => u.Id == user.GetUserId());
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return null;
        }
    }

    /// <summary>
    /// 更改用户的区域/语言设置。
    /// </summary>
    [Description("更改用户的区域/语言设置。当用户要求更改应用程序语言时，请使用此工具。常见LCID代码：1033=英文(en-US)，1065=波斯文(fa-IR)，1053=瑞典文(sv-SE)，2057=英式英文(en-GB)，1043=荷兰文(nl-NL)，1081=印地文(hi-IN)，2052=简体中文(zh-CN)，3082=西班牙文(es-ES)，1036=法文(fr-FR)，1025=阿拉伯文(ar-SA)，1031=德文(de-DE)。")]
    [McpServerTool(Name = nameof(SetCulture))]
    private async Task<string?> SetCulture(
        [Required, Description("区域LCID代码 (例如：1033代表en-US, 2052代表zh-CN)")] int cultureLcid)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(SetCulture)} (cultureLcid: {cultureLcid})");
        await EnsureSignalRConnectionIdIsPresent();

        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var culture = CultureInfo.GetCultureInfo(cultureLcid);

            if (CultureInfoManager.SupportedCultures.All(c => c.Culture.LCID != cultureLcid))
                return $"The requested culture is not supported. Available cultures: {string.Join(", ", CultureInfoManager.SupportedCultures.Select(c => c.Culture.NativeName))}";

            _ = await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
                .Clients.Client(signalRConnectionId!)
                .InvokeAsync<bool>(SharedAppMessages.CHANGE_CULTURE, cultureLcid, CancellationToken.None);

            return "Culture/Language changed successfully";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to change culture/language";
        }
    }

    /// <summary>
    /// 在亮色和暗色模式之间更改用户的主题偏好。
    /// </summary>
    [Description("在亮色和暗色模式之间更改用户的主题偏好。当用户要求更改应用程序主题或外观时，请使用此工具。")]
    [McpServerTool(Name = nameof(SetTheme))]
    private async Task<string?> SetTheme(
        [Required, Description("主题名称：'light'(亮色) 或 'dark'(暗色)")] string theme)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(SetTheme)} (theme: {theme})");
        await EnsureSignalRConnectionIdIsPresent();

        if (theme != "light" && theme != "dark")
            return "Invalid theme. Use 'light' or 'dark'.";

        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
                .Clients.Client(signalRConnectionId!)
                .InvokeAsync<bool>(SharedAppMessages.CHANGE_THEME, theme, CancellationToken.None);

            return $"Theme changed to {theme} successfully";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to change theme";
        }
    }

    /// <summary>
    /// 从诊断日志中检索用户设备上发生的最后一个错误。
    /// </summary>
    [Description("从诊断日志中检索用户设备上发生的最后一个错误。在排查用户报告的问题、调查应用程序崩溃或当用户提到某些功能不起作用时，请使用此工具。")]
    [McpServerTool(Name = nameof(CheckLastError))]
    private async Task<string?> CheckLastError()
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(CheckLastError)}");
        await EnsureSignalRConnectionIdIsPresent();

        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var lastError = await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
                .Clients.Client(signalRConnectionId!)
                .InvokeAsync<DiagnosticLogDto?>(SharedAppMessages.UPLOAD_LAST_ERROR, CancellationToken.None);

            if (lastError is null)
                return "No errors found in the diagnostic logs.";

            return lastError.ToString();
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to retrieve error information from the device.";
        }
    }

    /// <summary>
    /// 清除用户设备上的应用程序文件以修复问题。
    /// </summary>
    [Description("清除用户设备上的应用程序文件（缓存）以修复可能存在的问题。")]
    [McpServerTool(Name = nameof(ClearAppFiles))]
    private async Task<string?> ClearAppFiles()
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(ClearAppFiles)}");
        await EnsureSignalRConnectionIdIsPresent();

        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
                .Clients.Client(signalRConnectionId!)
                .InvokeAsync<DiagnosticLogDto?>(SharedAppMessages.CLEAR_APP_FILES, CancellationToken.None);

            return "App files cleared successfully on the device.";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to clear app files on the device.";
        }
    }

}

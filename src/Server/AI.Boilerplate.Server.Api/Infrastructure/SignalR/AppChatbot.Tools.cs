﻿using System.ComponentModel;
using System.Data.Common;
using System.Text.Json;
using System.Text.RegularExpressions;
using ModelContextProtocol.Server;
using Microsoft.AspNetCore.SignalR;
using AI.Boilerplate.Shared.Features.Diagnostic;
using AI.Boilerplate.Server.Api.Features.Identity;
using AI.Boilerplate.Shared.Features.Identity.Dtos;
using AI.Boilerplate.Server.Api.Infrastructure.Services;
using AI.Boilerplate.Server.Api.Features.Todo;
using AI.Boilerplate.Shared.Features.Todo;

namespace AI.Boilerplate.Server.Api.Infrastructure.SignalR;

[McpServerToolType]
public partial class AppChatbot
{
    private static readonly Regex ReadOnlySqlRegex = new(@"^\s*(select|with)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex ForbiddenSqlRegex = new(@"\b(insert|update|delete|drop|alter|truncate|create|grant|revoke|comment|vacuum|analyze|call|do|merge|refresh|copy)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex WriteSqlRegex = new(@"^\s*(insert|update|delete)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex DdlSqlRegex = new(@"\b(drop|alter|truncate|create|grant|revoke|comment|vacuum|analyze|call|do|merge|refresh|copy)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex UpdateDeleteWithoutWhereRegex = new(@"^\s*(update|delete)\b(?![\s\S]*\bwhere\b)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex DangerousSqlRegex = new(@"\b(pg_sleep|dblink|postgres_fdw)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

    [Description("添加一个新的待办事项。")]
    [McpServerTool(Name = nameof(AddTodoItem))]
    private async Task<TodoItemDto?> AddTodoItem(
        [Required, Description("待办标题")] string title)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(AddTodoItem)} (title: {title})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent(); 
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = new TodoItem
            {
                Id = Guid.CreateVersion7().ToString(),
                Title = title,
                IsDone = false,
                UserId = userId,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            entity.Id ??= Guid.CreateVersion7().ToString();

            await db.TodoItems.AddAsync(entity, CancellationToken.None);
            await db.SaveChangesAsync(CancellationToken.None);
            await NotifyTodoItemsChanged();

            return entity.Map();
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return null;
        }
    }

    [Description("按原标题更新待办事项。可以修改标题、完成状态，或同时修改二者。")]
    [McpServerTool(Name = nameof(UpdateTodoItem))]
    private async Task<string?> UpdateTodoItem(
        [Required, Description("原待办标题（精确匹配）")] string currentTitle,
        [Description("新的待办标题")] string? newTitle = null,
        [Description("新的完成状态，true为已完成，false为进行中")] bool? isDone = null)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(UpdateTodoItem)} (currentTitle: {currentTitle}, newTitle: {newTitle}, isDone: {isDone})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            if (newTitle is null && isDone is null)
                return "No changes provided. Please specify newTitle or isDone.";

            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.TodoItems
                .Where(t => t.UserId == userId && t.Title == currentTitle)
                .OrderByDescending(t => t.UpdatedAt)
                .FirstOrDefaultAsync(CancellationToken.None)
                ?? throw new ResourceNotFoundException("Todo item not found.");

            if (newTitle is not null)
                entity.Title = newTitle;
            if (isDone is not null)
                entity.IsDone = isDone.Value;

            entity.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(CancellationToken.None);
            await NotifyTodoItemsChanged();

            return $"Todo item updated successfully. title: {entity.Title}, isDone: {entity.IsDone}";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to update todo item.";
        }
    }

    [Description("将指定标题的待办标记为已完成。")]
    [McpServerTool(Name = nameof(CompleteTodoItem))]
    private async Task<string?> CompleteTodoItem(
    [Required, Description("待办标题（精确匹配）")] string title)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(CompleteTodoItem)} (title: {title})");
        return await UpdateTodoItem(title, null, true);
    }

    [Description("删除指定的待办事项。")]
    [McpServerTool(Name = nameof(DeleteTodoItem))]
    private async Task<string?> DeleteTodoItem(
        [Required, Description("待办标题")] string title)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(DeleteTodoItem)} (title: {title})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.TodoItems
                .FirstOrDefaultAsync(t => t.Title == title && t.UserId == userId, CancellationToken.None)
                ?? throw new ResourceNotFoundException("Todo item not found.");

            db.TodoItems.Remove(entity);
            await db.SaveChangesAsync(CancellationToken.None);
            await NotifyTodoItemsChanged();
            return "Todo item deleted successfully.";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "Failed to delete todo item.";
        }
    }

    [Description("查询待办事项。支持全部(all)、正在进行(in_progress)和已完成(completed)。")]
    [McpServerTool(Name = nameof(GetTodoItems))]
    private async Task<TodoItemDto[]> GetTodoItems(
        [Required, Description("查询类型：all、in_progress、completed")] string status)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(GetTodoItems)} (status: {status})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var normalizedStatus = status.Trim().ToLowerInvariant();

            var query = db.TodoItems
                .Where(t => t.UserId == userId);

            query = normalizedStatus switch
            {
                "all" => query,
                "in_progress" => query.Where(t => t.IsDone == false),
                "completed" => query.Where(t => t.IsDone),
                _ => throw new ValidationException("Invalid status. Use all, in_progress, or completed.")
            };

            var result = await query
                .OrderByDescending(t => t.UpdatedAt)
                .Project()
                .ToArrayAsync(CancellationToken.None);
            return result;
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return [];
        }
    }

    private async Task NotifyTodoItemsChanged()
    {
        await EnsureSignalRConnectionIdIsPresent();

        await using var scope = serviceProvider.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>()
            .Clients.Client(signalRConnectionId!)
            .Publish(SharedAppMessages.TODO_ITEMS_CHANGED, CancellationToken.None);
    }

    private async Task<Guid> EnsureCurrentUserIdIsPresent()
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
        var authenticatedHttpUser = httpContextAccessor?.HttpContext?.User;

        if (authenticatedHttpUser?.IsAuthenticated() is true)
        {
            currentUserId = authenticatedHttpUser.GetUserId();
            return currentUserId.Value;
        }

        if (currentUserId is not null)
            return currentUserId.Value;

        throw new UnauthorizedException("User must be authenticated to manage todo items.");
    }
}

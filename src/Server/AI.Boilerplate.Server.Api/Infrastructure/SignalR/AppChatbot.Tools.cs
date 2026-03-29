using System.ComponentModel;
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

    [Description("列出 PostgreSQL 中可访问的所有业务表（schema.table）。")]
    [McpServerTool(Name = nameof(PgListTables))]
    private async Task<string[]> PgListTables()
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgListTables)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            await using var command = connection.CreateCommand();
            command.CommandText = """
                                  SELECT table_schema, table_name
                                  FROM information_schema.tables
                                  WHERE table_type = 'BASE TABLE'
                                    AND table_schema NOT IN ('pg_catalog', 'information_schema', 'jops')
                                  ORDER BY table_schema, table_name;
                                  """;

            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);
            var tables = new List<string>();
            while (await reader.ReadAsync(CancellationToken.None))
            {
                tables.Add($"{reader.GetString(0)}.{reader.GetString(1)}");
            }

            return [.. tables];
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return [];
        }
    }

    [Description("查询指定表的字段信息。table 支持 table 或 schema.table。")]
    [McpServerTool(Name = nameof(PgGetTableColumns))]
    private async Task<string> PgGetTableColumns(
        [Required, Description("表名，支持 table 或 schema.table")] string table)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgGetTableColumns)} (table: {table})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);
            return JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                columns = tableInfo.Columns.Values.OrderBy(c => c).ToArray()
            });
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("查询指定表的数据。whereJson 为 JSON 对象，仅支持等值过滤；orderBy 例如: UpdatedAt desc, Title asc。")]
    [McpServerTool(Name = nameof(PgSelectRows))]
    private async Task<string> PgSelectRows(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Description("过滤条件 JSON，例如 {\"Title\":\"任务A\",\"IsDone\":false}")] string? whereJson = null,
        [Description("排序字段，例如 UpdatedAt desc, Title asc")] string? orderBy = null,
        [Description("返回行数，默认50，最大500")] int limit = 50,
        [Description("分页偏移，默认0")] int offset = 0)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgSelectRows)} (table: {table}, limit: {limit}, offset: {offset})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            await using var command = connection.CreateCommand();
            var parameterIndex = 0;
            var whereClause = BuildWhereClause(whereJson, tableInfo, command, ref parameterIndex);
            var orderByClause = BuildOrderByClause(orderBy, tableInfo);
            var safeLimit = Math.Clamp(limit, 1, 500);
            var safeOffset = Math.Max(offset, 0);

            command.CommandText = $"SELECT * FROM {tableInfo.QuotedName}{whereClause}{orderByClause} LIMIT @p{parameterIndex++} OFFSET @p{parameterIndex};";
            AddParameter(command, $"@p{parameterIndex - 1}", safeLimit);
            AddParameter(command, $"@p{parameterIndex}", safeOffset);

            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);
            var rows = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync(CancellationToken.None))
            {
                rows.Add(ReadRow(reader));
            }

            return JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                total = rows.Count,
                rows
            });
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("向指定表插入一条数据。dataJson 为 JSON 对象。")]
    [McpServerTool(Name = nameof(PgInsertRow))]
    private async Task<string> PgInsertRow(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Required, Description("要插入的数据 JSON，例如 {\"Title\":\"任务A\",\"IsDone\":false}")] string dataJson)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgInsertRow)} (table: {table})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            using var doc = JsonDocument.Parse(dataJson);
            if (doc.RootElement.ValueKind is not JsonValueKind.Object)
                throw new ValidationException("dataJson must be a JSON object.");

            var pairs = ExtractColumnsAndValues(doc.RootElement, tableInfo);
            if (pairs.Count == 0)
                throw new ValidationException("dataJson must contain at least one field.");

            await using var command = connection.CreateCommand();
            var cols = new List<string>();
            var vals = new List<string>();
            for (var i = 0; i < pairs.Count; i++)
            {
                var p = $"@p{i}";
                cols.Add(QuoteIdentifier(pairs[i].Column));
                vals.Add(p);
                AddParameter(command, p, pairs[i].Value);
            }

            command.CommandText = $"INSERT INTO {tableInfo.QuotedName} ({string.Join(", ", cols)}) VALUES ({string.Join(", ", vals)}) RETURNING *;";

            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);
            Dictionary<string, object?>? row = null;
            if (await reader.ReadAsync(CancellationToken.None))
            {
                row = ReadRow(reader);
            }

            if (IsTodoTable(tableInfo))
                await NotifyTodoItemsChanged();

            return JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                affectedRows = 1,
                row
            });
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("更新指定表数据。dataJson 为更新字段；whereJson 为等值过滤。默认禁止无条件全表更新。")]
    [McpServerTool(Name = nameof(PgUpdateRows))]
    private async Task<string> PgUpdateRows(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Required, Description("要更新的数据 JSON，例如 {\"IsDone\":true}")] string dataJson,
        [Description("过滤条件 JSON，例如 {\"Title\":\"任务A\"}")] string? whereJson = null,
        [Description("是否允许无条件更新全部数据，默认 false")] bool allowAffectAll = false)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgUpdateRows)} (table: {table}, allowAffectAll: {allowAffectAll})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            using var dataDoc = JsonDocument.Parse(dataJson);
            if (dataDoc.RootElement.ValueKind is not JsonValueKind.Object)
                throw new ValidationException("dataJson must be a JSON object.");

            var updates = ExtractColumnsAndValues(dataDoc.RootElement, tableInfo);
            if (updates.Count == 0)
                throw new ValidationException("dataJson must contain at least one field.");

            if (string.IsNullOrWhiteSpace(whereJson) && allowAffectAll is false)
                throw new ValidationException("whereJson is required unless allowAffectAll is true.");

            await using var command = connection.CreateCommand();
            var setClauses = new List<string>();
            var parameterIndex = 0;
            foreach (var update in updates)
            {
                var p = $"@p{parameterIndex++}";
                setClauses.Add($"{QuoteIdentifier(update.Column)} = {p}");
                AddParameter(command, p, update.Value);
            }

            var whereClause = BuildWhereClause(whereJson, tableInfo, command, ref parameterIndex);
            command.CommandText = $"UPDATE {tableInfo.QuotedName} SET {string.Join(", ", setClauses)}{whereClause};";
            var affectedRows = await command.ExecuteNonQueryAsync(CancellationToken.None);

            if (affectedRows > 0 && IsTodoTable(tableInfo))
                await NotifyTodoItemsChanged();

            return JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                affectedRows
            });
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("删除指定表数据。whereJson 为等值过滤。默认禁止无条件全表删除。")]
    [McpServerTool(Name = nameof(PgDeleteRows))]
    private async Task<string> PgDeleteRows(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Description("过滤条件 JSON，例如 {\"Title\":\"任务A\"}")] string? whereJson = null,
        [Description("是否允许无条件删除全部数据，默认 false")] bool allowAffectAll = false)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgDeleteRows)} (table: {table}, allowAffectAll: {allowAffectAll})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            if (string.IsNullOrWhiteSpace(whereJson) && allowAffectAll is false)
                throw new ValidationException("whereJson is required unless allowAffectAll is true.");

            await using var command = connection.CreateCommand();
            var parameterIndex = 0;
            var whereClause = BuildWhereClause(whereJson, tableInfo, command, ref parameterIndex);
            command.CommandText = $"DELETE FROM {tableInfo.QuotedName}{whereClause};";
            var affectedRows = await command.ExecuteNonQueryAsync(CancellationToken.None);

            if (affectedRows > 0 && IsTodoTable(tableInfo))
                await NotifyTodoItemsChanged();

            return JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                affectedRows
            });
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("执行报表查询SQL，支持多表关联，仅允许只读 SELECT/WITH 语句。")]
    [McpServerTool(Name = nameof(PgQueryReport))]
    private async Task<string> PgQueryReport(
        [Required, Description("报表SQL，仅支持 SELECT/WITH")] string sql,
        [Description("最大返回行数，默认500，最大2000")] int limit = 500)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgQueryReport)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var reportResult = await ExecuteReportQueryAsync(connection, sql, limit, CancellationToken.None);
            return JsonSerializer.Serialize(reportResult);
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("将自然语言报表需求转换为SQL并执行。仅执行只读 SELECT/WITH；失败时自动带错误上下文重试一次。拿到执行结果后，你必须将执行结果的所有列和数据原样使用 markdown 表格展示给用户，并将表格的列名（表头）翻译为易懂的中文。绝对不要擅自总结、解释、截断或遗漏数据行。不要输出其他多余的自然语言。")]
    [McpServerTool(Name = nameof(PgTextToSqlReport))]
    private async Task<string> PgTextToSqlReport(
        [Required, Description("报表需求自然语言描述，例如：统计近30天各分类产品数量")] string reportRequirement,
        [Description("最大返回行数，默认500，最大2000")] int limit = 500)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgTextToSqlReport)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var schemaSummary = await BuildSchemaSummaryAsync(connection, CancellationToken.None);

            var firstSql = await GenerateTextToSqlAsync(reportRequirement, schemaSummary, userId, null, CancellationToken.None);

            try
            {
                var firstResult = await ExecuteReportQueryAsync(connection, firstSql, limit, CancellationToken.None);
                return JsonSerializer.Serialize(new
                {
                    sql = firstResult.sql,
                    retry = false,
                    firstResult.total,
                    firstResult.rows
                });
            }
            catch (Exception firstExp)
            {
                var secondSql = await GenerateTextToSqlAsync(reportRequirement, schemaSummary, userId, firstExp.Message, CancellationToken.None);
                var secondResult = await ExecuteReportQueryAsync(connection, secondSql, limit, CancellationToken.None);

                return JsonSerializer.Serialize(new
                {
                    sql = secondResult.sql,
                    retry = true,
                    previousError = firstExp.Message,
                    secondResult.total,
                    secondResult.rows
                });
            }
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("将自然语言数据变更需求转换为 SQL 并执行（INSERT/UPDATE/DELETE）。请将执行结果反馈给用户。")]
    [McpServerTool(Name = nameof(PgTextToSqlWrite))]
    private async Task<string> PgTextToSqlWrite(
        [Required, Description("自然语言数据变更需求，例如：把标题为A的任务标记完成")] string writeRequirement,
        [Description("最大允许影响行数，默认50，最大200")] int maxAffectedRows = 50)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgTextToSqlWrite)} (maxAffectedRows: {maxAffectedRows})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var schemaSummary = await BuildSchemaSummaryAsync(connection, CancellationToken.None);
            var accessibleTables = await LoadAccessibleTablesAsync(connection, CancellationToken.None);
            var safeMaxAffectedRows = Math.Clamp(maxAffectedRows, 1, 200);

            var firstSql = await GenerateTextToWriteSqlAsync(writeRequirement, schemaSummary, userId, null, CancellationToken.None);

            try
            {
                var firstResult = await ExecuteWriteSqlAsync(connection, firstSql, safeMaxAffectedRows, accessibleTables, CancellationToken.None);
                return JsonSerializer.Serialize(new
                {
                    firstResult.sql,
                    retry = false,
                    firstResult.affectedRows,
                    firstResult.firstRow
                });
            }
            catch (Exception firstExp)
            {
                var secondSql = await GenerateTextToWriteSqlAsync(writeRequirement, schemaSummary, userId, firstExp.Message, CancellationToken.None);
                var secondResult = await ExecuteWriteSqlAsync(connection, secondSql, safeMaxAffectedRows, accessibleTables, CancellationToken.None);
                return JsonSerializer.Serialize(new
                {
                    secondResult.sql,
                    retry = true,
                    previousError = firstExp.Message,
                    secondResult.affectedRows,
                    secondResult.firstRow
                });
            }
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    private static string QuoteIdentifier(string identifier) => $"\"{identifier.Replace("\"", "\"\"")}\"";

    private static async Task<(string sql, int total, List<Dictionary<string, object?>> rows)> ExecuteReportQueryAsync(
        DbConnection connection,
        string sql,
        int limit,
        CancellationToken cancellationToken)
    {
        var sanitizedSql = ValidateReadOnlySql(sql);
        var safeLimit = Math.Clamp(limit, 1, 2000);

        await using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM ({sanitizedSql}) AS report_result LIMIT @p0;";
        AddParameter(command, "@p0", safeLimit);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var rows = new List<Dictionary<string, object?>>();
        while (await reader.ReadAsync(cancellationToken))
        {
            rows.Add(ReadRow(reader));
        }

        return (sanitizedSql, rows.Count, rows);
    }

    private static async Task<(string sql, int affectedRows, Dictionary<string, object?>? firstRow)> ExecuteWriteSqlAsync(
        DbConnection connection,
        string sql,
        int maxAffectedRows,
        List<PgTableNameInfo> accessibleTables,
        CancellationToken cancellationToken)
    {
        var (normalizedSql, _) = PrepareWriteSql(sql, accessibleTables);
        var executableSql = EnsureReturningAllColumns(normalizedSql);

        await using var command = connection.CreateCommand();
        command.CommandText = executableSql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var affectedRows = 0;
        Dictionary<string, object?>? firstRow = null;
        while (await reader.ReadAsync(cancellationToken))
        {
            affectedRows++;
            firstRow ??= ReadRow(reader);
        }

        if (affectedRows > maxAffectedRows)
            throw new ValidationException($"Affected rows {affectedRows} exceeded safety limit {maxAffectedRows}.");

        return (normalizedSql, affectedRows, firstRow);
    }

    private static string EnsureReturningAllColumns(string sql)
    {
        if (Regex.IsMatch(sql, @"\bRETURNING\b", RegexOptions.IgnoreCase))
            return sql;

        return $"{sql.Trim().TrimEnd(';')} RETURNING *;";
    }

    private static (string normalizedSql, PgTableNameInfo targetTable) PrepareWriteSql(
        string sql,
        List<PgTableNameInfo> accessibleTables)
    {
        var sanitizedSql = ValidateWriteSql(sql);
        var tableNormalizedSql = NormalizeWriteSqlTableIdentifiers(sanitizedSql, accessibleTables);
        var targetTable = ResolveWriteTargetTableInfo(tableNormalizedSql, accessibleTables);
        var columnNormalizedSql = NormalizeWriteSqlColumnIdentifiers(tableNormalizedSql, targetTable);
        var normalizedSql = EnrichInsertSqlWithRequiredColumns(columnNormalizedSql, targetTable);
        return (normalizedSql, targetTable);
    }

    private static async Task<int?> EstimateAffectedRowsAsync(
        DbConnection connection,
        string normalizedSql,
        PgTableNameInfo targetTable,
        CancellationToken cancellationToken)
    {
        if (normalizedSql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase))
            return 1;

        if (normalizedSql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase))
        {
            var match = Regex.Match(normalizedSql, @"\bWHERE\b(?<where>[\s\S]*)$", RegexOptions.IgnoreCase);
            if (match.Success is false) return null;
            var whereClause = match.Groups["where"].Value.Trim();
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(1) FROM {QuoteIdentifier(targetTable.Schema)}.{QuoteIdentifier(targetTable.Table)} WHERE {whereClause};";
            var count = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(count);
        }

        if (normalizedSql.TrimStart().StartsWith("delete", StringComparison.OrdinalIgnoreCase))
        {
            var match = Regex.Match(normalizedSql, @"\bWHERE\b(?<where>[\s\S]*)$", RegexOptions.IgnoreCase);
            if (match.Success is false) return null;
            var whereClause = match.Groups["where"].Value.Trim();
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(1) FROM {QuoteIdentifier(targetTable.Schema)}.{QuoteIdentifier(targetTable.Table)} WHERE {whereClause};";
            var count = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(count);
        }

        return null;
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private static object? JsonElementToDbValue(JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String => value.GetString(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number when value.TryGetInt32(out var v) => v,
            JsonValueKind.Number when value.TryGetInt64(out var v) => v,
            JsonValueKind.Number when value.TryGetDecimal(out var v) => v,
            JsonValueKind.Number => value.GetDouble(),
            _ => value.GetRawText()
        };
    }

    private static Dictionary<string, object?> ReadRow(DbDataReader reader)
    {
        var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < reader.FieldCount; i++)
        {
            var value = reader.GetValue(i);
            row[reader.GetName(i)] = value == DBNull.Value ? null : value;
        }
        return row;
    }

    private static string ValidateReadOnlySql(string sql)
    {
        var normalized = sql.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationException("sql is required.");

        if (normalized.EndsWith(';'))
            normalized = normalized[..^1].TrimEnd();

        if (normalized.Contains(';'))
            throw new ValidationException("Only single SQL statement is allowed.");

        if (ReadOnlySqlRegex.IsMatch(normalized) is false)
            throw new ValidationException("Only SELECT/WITH read-only SQL is allowed.");

        if (ForbiddenSqlRegex.IsMatch(normalized))
            throw new ValidationException("Write/DDL SQL is not allowed in report query.");

        if (DangerousSqlRegex.IsMatch(normalized))
            throw new ValidationException("Dangerous SQL function is not allowed.");

        return normalized;
    }

    private static string ValidateWriteSql(string sql)
    {
        var normalized = sql.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationException("sql is required.");

        if (normalized.EndsWith(';'))
            normalized = normalized[..^1].TrimEnd();

        if (normalized.Contains(';'))
            throw new ValidationException("Only single SQL statement is allowed.");

        if (WriteSqlRegex.IsMatch(normalized) is false)
            throw new ValidationException("Only INSERT/UPDATE/DELETE SQL is allowed.");

        if (DdlSqlRegex.IsMatch(normalized))
            throw new ValidationException("DDL SQL is not allowed.");

        if (DangerousSqlRegex.IsMatch(normalized))
            throw new ValidationException("Dangerous SQL function is not allowed.");

        if (UpdateDeleteWithoutWhereRegex.IsMatch(normalized))
            throw new ValidationException("UPDATE/DELETE must include WHERE clause.");

        return normalized;
    }

    private async Task<string> GenerateTextToSqlAsync(
        string reportRequirement,
        string schemaSummary,
        Guid currentUserId,
        string? previousError,
        CancellationToken cancellationToken)
    {
        if (serviceProvider.GetService<IChatClient>() is not IChatClient chatClient)
            throw new ResourceNotFoundException("IChatClient is not available.");

        var sqlAgent = chatClient.AsAIAgent(
            instructions: """
            你是 PostgreSQL 报表 SQL 生成器。
            目标：根据用户需求和给定 schema 生成可执行、只读的 SQL。

            约束：
            1) 只允许 SELECT 或 WITH 开头语句；
            2) 只生成一条 SQL，不允许分号分隔多语句；
            3) 禁止 INSERT/UPDATE/DELETE/DDL；
            4) 如果表中有比较耗时的字段（如大文本 DescriptionHTML, 向量 Embedding 等），请绝对不要使用 SELECT *，必须明确指定需要查询的核心字段（如主键、名称、状态、日期等）。仅当表结构简单且无大文本字段时，才可使用 SELECT *。
            5) 表名和字段名必须严格来自 schema，绝对不要凭空捏造或联想不存在的字段。如果要查询的字段不在主表中，必须通过 JOIN 关联；如果用户条件涉及的字段在当前查询的表中不存在，绝对不要将该条件强加到该表上，直接忽略该条件或寻找合理的替代方案。
            6) 如果需求不清晰，做最保守可执行推断；
            7) 所有的表名和列名（包括 SELECT 列表、WHERE 条件、GROUP BY 和 ORDER BY 中的列名）都必须使用双引号包裹，以保证大小写精确匹配。当使用表别名时，表别名不要加双引号，但列名必须加双引号，格式为：别名."列名"（例如：SELECT p."Id", c."Name" FROM "public"."Products" p JOIN "public"."Categories" c ON p."CategoryId" = c."Id" WHERE p."IsDone" = true）。绝对不要使用未带双引号的列名，也不要将别名和列名一起放在一个双引号内（如 "p.Id" 是错误的，必须是 p."Id"）。
            8) 用户提供的业务值必须原样保留，不允许擅自改写、替换、联想或翻译。
            9) 虽然我们在上下文提供了当前用户的 UserId，但你必须检查你要查询或操作的表是否真的有 UserId 字段，如果没有，请绝对不要在 WHERE 条件中加入 UserId 的过滤！
            10) 严禁在 WHERE 子句中使用窗口函数（如 ROW_NUMBER() OVER ()）。PostgreSQL 不允许在 WHERE 中直接使用窗口函数。如果需要限制行数，请使用 LIMIT 关键字；如果需要基于排序限制，请使用 ORDER BY 结合 LIMIT，或者使用子查询。

            输出格式：
            仅返回 JSON 对象：{"sql":"..."}，不要附加解释文本。
            """,
            name: "PgTextToSqlAgent",
            description: "Converts natural language report requirement into safe PostgreSQL read-only SQL");

        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.Json,
            AdditionalProperties = new()
            {
                ["response_format"] = new { type = "json_object" }
            }
        };
        configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);

        var prompt = $"""
            当前用户的 UserId 为：{currentUserId}

            用户报表需求：
            {reportRequirement}

            可用数据库 schema（仅这些表和字段可用）：
            {schemaSummary}
            """;

        if (string.IsNullOrWhiteSpace(previousError) is false)
        {
            prompt += $"""

                上一次SQL执行错误：
                {previousError}

                请修正SQL并重新输出。
                """;
        }

        var response = await sqlAgent.RunAsync<TextToSqlResponse>(
            messages: [new(ChatRole.User, prompt)],
            cancellationToken: cancellationToken,
            options: new Microsoft.Agents.AI.ChatClientAgentRunOptions(chatOptions));

        var sql = response.Result.Sql?.Trim();
        if (string.IsNullOrWhiteSpace(sql))
            throw new ValidationException("Text-to-SQL model returned empty SQL.");

        Console.WriteLine($"\n[Generated SQL]: {sql}");
        return sql!;
    }

    private async Task<string> GenerateTextToWriteSqlAsync(
        string writeRequirement,
        string schemaSummary,
        Guid currentUserId,
        string? previousError,
        CancellationToken cancellationToken)
    {
        if (serviceProvider.GetService<IChatClient>() is not IChatClient chatClient)
            throw new ResourceNotFoundException("IChatClient is not available.");

        var sqlAgent = chatClient.AsAIAgent(
            instructions: """
            你是 PostgreSQL 写入 SQL 生成器。
            目标：根据用户需求和给定 schema 生成可执行的数据变更 SQL。

            约束：
            1) 只允许 INSERT/UPDATE/DELETE；
            2) 只生成一条 SQL，不允许分号分隔多语句；
            3) 严禁 DDL（CREATE/ALTER/DROP/TRUNCATE 等）；
            4) UPDATE/DELETE 必须带 WHERE；
            5) 表名和字段名严格来自 schema。如果用户指令中包含在当前表 schema 中不存在的字段（如尝试按 UserId 更新但表里没有 UserId），请绝对不要将该字段加入 SQL 中，直接忽略该无效条件。
            6) 若需求不清晰，做最保守的精确变更，不做全表操作。
            7) 所有的表名和列名（包括 INSERT 列表、UPDATE SET 列表和 WHERE 条件中的列名）都必须使用双引号包裹，以保证大小写精确匹配（例如：UPDATE "public"."TodoItems" SET "IsDone" = true WHERE "Id" = '...'）。不要使用未带双引号的标识符。
            8) 用户提供的业务值必须原样保留，不允许擅自改写、替换、联想或翻译（例如“劳斯莱斯”不能改成其他名称）。
            9) 虽然我们在上下文提供了当前用户的 UserId，但你必须检查你要查询或操作的表是否真的有 UserId 字段，如果没有，请不要在 WHERE 条件中加入 UserId 的过滤！

            输出格式：
            仅返回 JSON 对象：{"sql":"..."}，不要附加解释文本。
            """,
            name: "PgTextToWriteSqlAgent",
            description: "Converts natural language write requirement into safe PostgreSQL DML SQL");

        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.Json,
            AdditionalProperties = new()
            {
                ["response_format"] = new { type = "json_object" }
            }
        };
        configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);

        var prompt = $"""
            当前用户的 UserId 为：{currentUserId}

            用户写入需求：
            {writeRequirement}

            可用数据库 schema（仅这些表和字段可用）：
            {schemaSummary}
            """;

        if (string.IsNullOrWhiteSpace(previousError) is false)
        {
            prompt += $"""

                上一次SQL执行错误：
                {previousError}

                请修正SQL并重新输出。
                """;
        }

        var response = await sqlAgent.RunAsync<TextToSqlResponse>(
            messages: [new(ChatRole.User, prompt)],
            cancellationToken: cancellationToken,
            options: new Microsoft.Agents.AI.ChatClientAgentRunOptions(chatOptions));

        var sql = response.Result.Sql?.Trim();
        if (string.IsNullOrWhiteSpace(sql))
            throw new ValidationException("Text-to-SQL model returned empty SQL.");

        Console.WriteLine($"\n[Generated SQL]: {sql}");
        return sql!;
    }

    private static async Task<string> BuildSchemaSummaryAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT c.table_schema,
                                     c.table_name,
                                     c.column_name,
                                     COALESCE(obj_description((quote_ident(c.table_schema) || '.' || quote_ident(c.table_name))::regclass), '') AS table_comment,
                                     COALESCE(col_description((quote_ident(c.table_schema) || '.' || quote_ident(c.table_name))::regclass, c.ordinal_position), '') AS column_comment
                              FROM information_schema.columns c
                              JOIN information_schema.tables t
                                ON t.table_schema = c.table_schema
                               AND t.table_name = c.table_name
                              WHERE t.table_type = 'BASE TABLE'
                                AND c.table_schema NOT IN ('pg_catalog', 'information_schema','jobs')
                              ORDER BY c.table_schema, c.table_name, c.ordinal_position;
                              """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var grouped = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        var tableComments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            var schema = reader.GetString(0);
            var table = reader.GetString(1);
            var column = reader.GetString(2);
            var tableComment = reader.GetString(3);
            var columnComment = reader.GetString(4);
            var key = $"{schema}.{table}";
            if (grouped.TryGetValue(key, out var cols) is false)
            {
                cols = [];
                grouped[key] = cols;
            }
            if (tableComments.ContainsKey(key) is false)
                tableComments[key] = tableComment;

            var resolvedColumnComment = string.IsNullOrWhiteSpace(columnComment) ? column : columnComment;
            cols.Add($"{column}({resolvedColumnComment})");
        }

        var sb = new System.Text.StringBuilder();
        foreach (var kv in grouped.Take(100))
        {
            var full = kv.Key.Split('.');
            var tableComment = tableComments.TryGetValue(kv.Key, out var comment) && string.IsNullOrWhiteSpace(comment) is false
                ? comment
                : full[1];
            sb.Append("- ")
              .Append(QuoteIdentifier(full[0]))
              .Append('.')
              .Append(QuoteIdentifier(full[1]))
              .Append(" [业务含义: ").Append(tableComment).Append("]")
              .Append(" (raw: ").Append(kv.Key).Append("): ")
              .Append(string.Join(", ", kv.Value.Take(80))).AppendLine();
        }

        return sb.ToString();
    }

    private static async Task<List<PgTableNameInfo>> LoadAccessibleTablesAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT c.table_schema,
                                     c.table_name,
                                     c.column_name,
                                     c.data_type,
                                     c.udt_name,
                                     c.is_nullable,
                                     COALESCE(c.column_default, '')
                              FROM information_schema.columns c
                              JOIN information_schema.tables t
                                ON t.table_schema = c.table_schema
                               AND t.table_name = c.table_name
                              WHERE t.table_type = 'BASE TABLE'
                                AND c.table_schema NOT IN ('pg_catalog', 'information_schema', 'jobs')
                              ORDER BY c.table_schema, c.table_name, c.ordinal_position;
                              """;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var grouped = new Dictionary<string, PgTableNameInfo>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            var schema = reader.GetString(0);
            var table = reader.GetString(1);
            var column = reader.GetString(2);
            var dataType = reader.GetString(3);
            var udtName = reader.GetString(4);
            var isNullable = reader.GetString(5).Equals("YES", StringComparison.OrdinalIgnoreCase);
            var columnDefault = reader.GetString(6);
            var key = $"{schema}.{table}";

            if (grouped.TryGetValue(key, out var info) is false)
            {
                info = new PgTableNameInfo(schema, table, new Dictionary<string, PgTableColumnInfo>(StringComparer.OrdinalIgnoreCase));
                grouped[key] = info;
            }

            info.Columns[column] = new PgTableColumnInfo(column, dataType, udtName, isNullable, columnDefault);
        }

        return grouped.Values.ToList();
    }

    private static string NormalizeWriteSqlTableIdentifiers(string sql, List<PgTableNameInfo> accessibleTables)
    {
        return WriteSqlRegex.IsMatch(sql) switch
        {
            true when sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase) =>
                Regex.Replace(sql, @"\bINSERT\s+INTO\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))",
                    m => $"INSERT INTO {ResolveAndQuoteTableIdentifier(m.Groups["id"].Value, accessibleTables)}", RegexOptions.IgnoreCase),
            true when sql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase) =>
                Regex.Replace(sql, @"\bUPDATE\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))",
                    m => $"UPDATE {ResolveAndQuoteTableIdentifier(m.Groups["id"].Value, accessibleTables)}", RegexOptions.IgnoreCase),
            true when sql.TrimStart().StartsWith("delete", StringComparison.OrdinalIgnoreCase) =>
                Regex.Replace(sql, @"\bDELETE\s+FROM\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))",
                    m => $"DELETE FROM {ResolveAndQuoteTableIdentifier(m.Groups["id"].Value, accessibleTables)}", RegexOptions.IgnoreCase),
            _ => sql
        };
    }

    private static string ResolveAndQuoteTableIdentifier(string input, List<PgTableNameInfo> accessibleTables)
    {
        var raw = input.Replace("\"", string.Empty);
        var parts = raw.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 2)
        {
            var match = accessibleTables.FirstOrDefault(t =>
                t.Schema.Equals(parts[0], StringComparison.OrdinalIgnoreCase) &&
                t.Table.Equals(parts[1], StringComparison.OrdinalIgnoreCase));
            if (match is null)
                throw new ValidationException($"Table '{input}' is not accessible.");
            return $"{QuoteIdentifier(match.Schema)}.{QuoteIdentifier(match.Table)}";
        }

        if (parts.Length == 1)
        {
            var candidates = accessibleTables
                .Where(t => t.Table.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (candidates.Length == 0)
                throw new ValidationException($"Table '{input}' is not accessible.");
            if (candidates.Length > 1)
                throw new ValidationException($"Table '{input}' is ambiguous. Use schema-qualified table name.");
            return $"{QuoteIdentifier(candidates[0].Schema)}.{QuoteIdentifier(candidates[0].Table)}";
        }

        throw new ValidationException($"Invalid table identifier '{input}'.");
    }

    private static PgTableNameInfo ResolveWriteTargetTableInfo(string sql, List<PgTableNameInfo> accessibleTables)
    {
        var pattern = sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase)
            ? @"\bINSERT\s+INTO\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))"
            : sql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase)
                ? @"\bUPDATE\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))"
                : @"\bDELETE\s+FROM\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))";

        var match = Regex.Match(sql, pattern, RegexOptions.IgnoreCase);
        if (match.Success is false)
            throw new ValidationException("Unable to resolve target table from SQL.");

        return ResolveTableInfoFromIdentifier(match.Groups["id"].Value, accessibleTables);
    }

    private static PgTableNameInfo ResolveTableInfoFromIdentifier(string input, List<PgTableNameInfo> accessibleTables)
    {
        var raw = input.Replace("\"", string.Empty);
        var parts = raw.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 2)
        {
            return accessibleTables.FirstOrDefault(t =>
                       t.Schema.Equals(parts[0], StringComparison.OrdinalIgnoreCase) &&
                       t.Table.Equals(parts[1], StringComparison.OrdinalIgnoreCase))
                   ?? throw new ValidationException($"Table '{input}' is not accessible.");
        }

        if (parts.Length == 1)
        {
            var candidates = accessibleTables
                .Where(t => t.Table.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (candidates.Length == 0)
                throw new ValidationException($"Table '{input}' is not accessible.");
            if (candidates.Length > 1)
                throw new ValidationException($"Table '{input}' is ambiguous. Use schema-qualified table name.");
            return candidates[0];
        }

        throw new ValidationException($"Invalid table identifier '{input}'.");
    }

    private static string NormalizeWriteSqlColumnIdentifiers(string sql, PgTableNameInfo tableInfo)
    {
        if (sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase))
        {
            return Regex.Replace(sql,
                @"\bINSERT\s+INTO\s+(?<table>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))\s*\((?<cols>[^)]*)\)",
                m =>
                {
                    var cols = m.Groups["cols"].Value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(c => QuoteIdentifier(ResolveColumnName(tableInfo, c).Name))
                        .ToArray();
                    return $"INSERT INTO {m.Groups["table"].Value} ({string.Join(", ", cols)})";
                },
                RegexOptions.IgnoreCase);
        }

        if (sql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase))
        {
            var normalized = Regex.Replace(sql,
                @"\bSET\s+(?<set>[\s\S]*?)(?<tail>\s+WHERE\s+[\s\S]*$|$)",
                m =>
                {
                    var assignments = m.Groups["set"].Value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(a =>
                        {
                            var idx = a.IndexOf('=');
                            if (idx <= 0) return a;
                            var left = a[..idx].Trim();
                            var right = a[(idx + 1)..].Trim();
                            return $"{QuoteIdentifier(ResolveColumnName(tableInfo, left).Name)} = {right}";
                        });
                    return $"SET {string.Join(", ", assignments)}{m.Groups["tail"].Value}";
                },
                RegexOptions.IgnoreCase);
            return NormalizeWhereColumns(normalized, tableInfo);
        }

        return NormalizeWhereColumns(sql, tableInfo);
    }

    private static string NormalizeWhereColumns(string sql, PgTableNameInfo tableInfo)
    {
        return Regex.Replace(sql,
            @"(?<lhs>(?:""[^""]+""|\w+)(?:\.(?:""[^""]+""|\w+))?)\s*(?<op>=|<>|!=|<=|>=|<|>|\bLIKE\b|\bIN\b|\bIS\b)",
            m =>
            {
                var left = m.Groups["lhs"].Value;
                if (left.Contains('.', StringComparison.Ordinal))
                    return $"{left} {m.Groups["op"].Value}";

                if (tableInfo.Columns.ContainsKey(UnwrapIdentifier(left)))
                {
                    return $"{QuoteIdentifier(ResolveColumnName(tableInfo, left).Name)} {m.Groups["op"].Value}";
                }

                return m.Value;
            },
            RegexOptions.IgnoreCase);
    }

    private static PgTableColumnInfo ResolveColumnName(PgTableNameInfo tableInfo, string inputColumnName)
    {
        var key = UnwrapIdentifier(inputColumnName);
        if (key.Contains('.', StringComparison.Ordinal))
            key = key.Split('.').Last();

        if (tableInfo.Columns.TryGetValue(key, out var resolved))
            return resolved;

        throw new ValidationException($"Column '{inputColumnName}' does not exist in table '{tableInfo.Schema}.{tableInfo.Table}'.");
    }

    private static string UnwrapIdentifier(string input) => input.Replace("\"", string.Empty).Trim();

    private static string EnrichInsertSqlWithRequiredColumns(string sql, PgTableNameInfo tableInfo)
    {
        if (sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase) is false)
            return sql;

        var match = Regex.Match(sql,
            @"\bINSERT\s+INTO\s+(?<table>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))\s*\((?<cols>[^)]*)\)\s*VALUES\s*\((?<vals>[\s\S]*)\)(?:\s*RETURNING[\s\S]*)?(?:\s*;)?\s*$",
            RegexOptions.IgnoreCase);
        if (match.Success is false)
            return sql;

        var cols = SplitSqlCsv(match.Groups["cols"].Value).Select(UnwrapIdentifier).ToList();
        var vals = SplitSqlCsv(match.Groups["vals"].Value).ToList();
        if (cols.Count != vals.Count)
            return sql;

        foreach (var column in tableInfo.Columns.Values)
        {
            if (cols.Contains(column.Name, StringComparer.OrdinalIgnoreCase))
                continue;

            if (column.IsNullable || string.IsNullOrWhiteSpace(column.ColumnDefault) is false)
                continue;

            var generated = BuildGeneratedColumnValue(column);
            if (generated is null)
                continue;

            cols.Add(column.Name);
            vals.Add(generated);
        }

        var rebuilt = $"INSERT INTO {match.Groups["table"].Value} ({string.Join(", ", cols.Select(QuoteIdentifier))}) VALUES ({string.Join(", ", vals)});";
        return rebuilt;
    }

    private static List<string> SplitSqlCsv(string input)
    {
        var result = new List<string>();
        var sb = new System.Text.StringBuilder();
        var depth = 0;
        var inSingleQuote = false;

        foreach (var ch in input)
        {
            if (ch == '\'' && (sb.Length == 0 || sb[^1] != '\\'))
                inSingleQuote = !inSingleQuote;

            if (inSingleQuote is false)
            {
                if (ch == '(') depth++;
                if (ch == ')') depth--;
                if (ch == ',' && depth == 0)
                {
                    result.Add(sb.ToString().Trim());
                    sb.Clear();
                    continue;
                }
            }

            sb.Append(ch);
        }

        if (sb.Length > 0)
            result.Add(sb.ToString().Trim());
        return result;
    }

    private static string? BuildGeneratedColumnValue(PgTableColumnInfo column)
    {
        if (column.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            if (column.DataType.Equals("uuid", StringComparison.OrdinalIgnoreCase) ||
                column.UdtName.Equals("uuid", StringComparison.OrdinalIgnoreCase))
                return $"'{Guid.CreateVersion7()}'";

            return $"'{Guid.CreateVersion7()}'";
        }

        if (column.Name.Equals("Version", StringComparison.OrdinalIgnoreCase))
            return "1";

        if (column.Name.Equals("CreatedOn", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("UpdatedAt", StringComparison.OrdinalIgnoreCase))
            return "NOW()";

        return null;
    }

    private static string BuildOrderByClause(string? orderBy, PgTableInfo tableInfo)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return string.Empty;

        var segments = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var orders = new List<string>();
        foreach (var segment in segments)
        {
            var parts = segment.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 0)
                continue;

            var column = ResolveColumnName(tableInfo, parts[0]);
            var direction = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "DESC"
                : "ASC";
            orders.Add($"{QuoteIdentifier(column)} {direction}");
        }

        return orders.Count == 0 ? string.Empty : $" ORDER BY {string.Join(", ", orders)}";
    }

    private static string BuildWhereClause(
        string? whereJson,
        PgTableInfo tableInfo,
        DbCommand command,
        ref int parameterIndex)
    {
        if (string.IsNullOrWhiteSpace(whereJson))
            return string.Empty;

        using var doc = JsonDocument.Parse(whereJson);
        if (doc.RootElement.ValueKind is not JsonValueKind.Object)
            throw new ValidationException("whereJson must be a JSON object.");

        var whereConditions = new List<string>();
        foreach (var property in doc.RootElement.EnumerateObject())
        {
            var column = ResolveColumnName(tableInfo, property.Name);
            if (property.Value.ValueKind is JsonValueKind.Null)
            {
                whereConditions.Add($"{QuoteIdentifier(column)} IS NULL");
                continue;
            }

            var parameterName = $"@p{parameterIndex++}";
            whereConditions.Add($"{QuoteIdentifier(column)} = {parameterName}");
            AddParameter(command, parameterName, JsonElementToDbValue(property.Value));
        }

        if (whereConditions.Count == 0)
            return string.Empty;

        return $" WHERE {string.Join(" AND ", whereConditions)}";
    }

    private static List<(string Column, object? Value)> ExtractColumnsAndValues(JsonElement root, PgTableInfo tableInfo)
    {
        var pairs = new List<(string Column, object? Value)>();
        foreach (var property in root.EnumerateObject())
        {
            var column = ResolveColumnName(tableInfo, property.Name);
            pairs.Add((column, JsonElementToDbValue(property.Value)));
        }
        return pairs;
    }

    private static string ResolveColumnName(PgTableInfo tableInfo, string inputColumnName)
    {
        if (tableInfo.Columns.TryGetValue(inputColumnName, out var resolvedColumn))
            return resolvedColumn;

        throw new ValidationException($"Column '{inputColumnName}' does not exist in table '{tableInfo.FullName}'.");
    }

    private static bool IsTodoTable(PgTableInfo tableInfo)
        => tableInfo.Schema.Equals("public", StringComparison.OrdinalIgnoreCase)
           && tableInfo.Name.Equals("TodoItems", StringComparison.OrdinalIgnoreCase);

    private async Task<DbConnection> OpenAppDbConnectionAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var connection = db.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private async Task<PgTableInfo> ResolveTableInfoAsync(DbConnection connection, string tableInput, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT table_schema, table_name, column_name
                              FROM information_schema.columns
                              WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
                              ORDER BY table_schema, table_name, ordinal_position;
                              """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var tables = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            var schema = reader.GetString(0);
            var table = reader.GetString(1);
            var column = reader.GetString(2);
            var fullName = $"{schema}.{table}";

            if (tables.TryGetValue(fullName, out var columns) is false)
            {
                columns = [];
                tables[fullName] = columns;
            }

            columns.Add(column);
        }

        if (tables.Count == 0)
            throw new ResourceNotFoundException("No accessible tables were found in database.");

        var input = tableInput.Trim();
        if (string.IsNullOrWhiteSpace(input))
            throw new ValidationException("table is required.");

        var hasSchema = input.Contains('.', StringComparison.Ordinal);
        string selectedFullName;

        if (hasSchema)
        {
            var parts = input.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
                throw new ValidationException("table format must be table or schema.table.");
            selectedFullName = $"{parts[0]}.{parts[1]}";
            if (tables.ContainsKey(selectedFullName) is false)
                throw new ResourceNotFoundException($"Table '{selectedFullName}' not found.");
        }
        else
        {
            var matched = tables.Keys
                .Where(k => k.Split('.')[1].Equals(input, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matched.Length == 0)
                throw new ResourceNotFoundException($"Table '{input}' not found.");
            if (matched.Length > 1)
                throw new ValidationException($"Table name '{input}' is ambiguous. Use schema.table format.");
            selectedFullName = matched[0];
        }

        var fullNameParts = selectedFullName.Split('.');
        var schemaName = fullNameParts[0];
        var tableName = fullNameParts[1];
        var columnLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var column in tables[selectedFullName])
        {
            columnLookup[column] = column;
        }

        return new PgTableInfo(
            schemaName,
            tableName,
            columnLookup,
            $"{QuoteIdentifier(schemaName)}.{QuoteIdentifier(tableName)}",
            $"{schemaName}.{tableName}");
    }

    private sealed record PgTableInfo(
        string Schema,
        string Name,
        Dictionary<string, string> Columns,
        string QuotedName,
        string FullName);

    private sealed record PgTableNameInfo(string Schema, string Table, Dictionary<string, PgTableColumnInfo> Columns);
    private sealed record PgTableColumnInfo(string Name, string DataType, string UdtName, bool IsNullable, string ColumnDefault);

    private sealed class TextToSqlResponse
    {
        public string? Sql { get; init; }
    }

    private static async Task<bool> LooksLikeBusinessDataMutationAsync(
        string text,
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        var value = text.Trim();
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var keywords = await LoadBusinessKeywordsFromDatabaseAsync(connection, cancellationToken);
        return keywords.Any(k => value.Contains(k, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task<HashSet<string>> LoadBusinessKeywordsFromDatabaseAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT c.table_name,
                                     c.column_name,
                                     COALESCE(obj_description((quote_ident(c.table_schema) || '.' || quote_ident(c.table_name))::regclass), '') AS table_comment,
                                     COALESCE(col_description((quote_ident(c.table_schema) || '.' || quote_ident(c.table_name))::regclass, c.ordinal_position), '') AS column_comment
                              FROM information_schema.columns c
                              JOIN information_schema.tables t
                                ON t.table_schema = c.table_schema
                               AND t.table_name = c.table_name
                              WHERE t.table_type = 'BASE TABLE'
                                AND c.table_schema NOT IN ('pg_catalog', 'information_schema', 'jobs')
                              ORDER BY c.table_name, c.ordinal_position;
                              """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            AddBusinessTerms(keywords, reader.GetString(0));
            AddBusinessTerms(keywords, reader.GetString(1));
            AddBusinessTerms(keywords, reader.GetString(2));
            AddBusinessTerms(keywords, reader.GetString(3));
        }

        return keywords;
    }

    private static void AddBusinessTerms(HashSet<string> keywords, string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return;

        foreach (var term in source
                     .Replace('_', ' ')
                     .Split([' ', ',', '，', '/', '、', ';', '；', '(', ')', '[', ']', '{', '}', '|'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (term.Length < 2)
                continue;

            keywords.Add(term);
        }
    }

}

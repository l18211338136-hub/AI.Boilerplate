# 第十五阶段：日志记录、OpenTelemetry 与健康检查

欢迎回到第十五阶段！在本阶段，您将了解 AI.Boilerplate 项目中内置的综合日志记录、可观测性和健康监控基础设施。

---

## 目录

1. [ILogger 用于错误、警告和信息](#ilogger-用于错误警告和信息)
2. [Activity 和 Meter 用于跟踪操作](#activity-和-meter-用于跟踪操作)
3. [日志配置](#日志配置)
4. [应用内诊断日志器](#应用内诊断日志器)
5. [与 Sentry 和 Azure Application Insights 的集成](#与-sentry-和-azure-application-insights-的集成)
6. [Aspire 仪表板](#aspire-仪表板)
7. [健康检查](#健康检查)

---

## 1. ILogger 用于错误、警告和信息

本项目在整个应用中使用 `Microsoft.Extensions.Logging` 中的 **`ILogger<T>`** 进行结构化日志记录。

### 基本用法

```csharp
[AutoInject] private ILogger<MyService> logger = default!;

public async Task ProcessData()
{
    logger.LogInformation("处理已开始");
    
    try
    {
        // 您的代码
        logger.LogWarning("发生了一些不寻常的情况");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "数据处理失败");
    }
}
```

### 使用作用域进行结构化日志记录

为了向日志添加上下文信息，请使用 **`BeginScope`**：

```csharp
var data = new Dictionary<string, object?>
{
    { "UserId", userId },
    { "OrderId", orderId },
    { "Culture", CultureInfo.CurrentUICulture.Name }
};

using var scope = logger.BeginScope(data);
logger.LogError(exception, "订单处理失败");
```

---

## 2. Activity 和 Meter 用于跟踪操作

为了跟踪**操作次数和持续时间**，本项目使用 **OpenTelemetry 的 ActivitySource**。

### ActivitySource

### 使用 Activity 跟踪操作

```csharp
using var activity = ActivitySource.Current.StartActivity("ProcessOrder");

try
{
    // 您的操作
    activity?.SetTag("orderId", orderId);
    activity?.SetTag("customerId", customerId);
}
catch (Exception ex)
{
    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
    throw;
}
```

### 使用 Meter 跟踪计数指标
为了跟踪**计数指标**（例如正在进行的操作数量），请使用 **OpenTelemetry 的 Meter**：

```csharp
// 在类级别定义计数器
private static readonly UpDownCounter<long> ongoingConversationsCount = 
    Meter.Current.CreateUpDownCounter<long>(
        "appHub.ongoing_conversations_count", 
        "聊天机器人中心正在进行的对话数量。");

// 操作开始时递增
ongoingConversationsCount.Add(1);

try
{
    // 您的长时间运行操作
    await ProcessConversation();
}
finally
{
    // 操作完成时递减
    ongoingConversationsCount.Add(-1);
}
```

此模式用于 `AppHub.Chatbot.cs` 中，以实时跟踪活跃的聊天机器人对话数量，这些数量可以在 Aspire 仪表板、Azure Application Insights 或其他可观测性工具中进行监控。

### 优势

- **持续时间跟踪**：自动测量操作耗时
- **分布式追踪**：跨多个服务跟踪请求
- **性能洞察**：识别瓶颈和缓慢操作
- **可视化**：在 Aspire 仪表板、Application Insights 或其他工具中查看追踪

---

## 3. 日志配置

日志配置集中在 [`src/Shared/appsettings.json`](/src/Shared/appsettings.json) 中。

### 配置结构

```json
{
  "ApplicationInsights": {
    "ConnectionString": null
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information",
      "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware": "None"
    },
    "Sentry": {
      "Sentry_Comment": "https://docs.sentry.io/platforms/dotnet/guides/extensions-logging/",
      "Dsn": "",
      "SendDefaultPii": true,
      "EnableScopeSync": true,
      "LogLevel": {
        "Default": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "DiagnosticLogger": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore*": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    }
  }
}
```

### 关键配置部分

- **默认日志级别**: `Warning` - 默认仅记录警告及以上级别
- **EF Core 命令**: `Information` - 在日志中显示 SQL 查询（便于调试）
- **Sentry**: 生产环境错误跟踪，级别为 `Warning`
- **DiagnosticLogger**: 应用内诊断的 `Information` 级别
- **Console**: 在移动平台上记录到设备日志/logcat

---

## 4. 应用内诊断日志器

本项目最有用的故障排除功能之一是 **诊断日志器 (Diagnostic Logger)** —— 一种自定义的内存日志器，有助于实时调试问题。

### 什么是诊断日志器？

诊断日志器是一个自定义的 `ILogger` 实现，它：
- 在客户端设备上**内存中**存储日志
- 默认为 **`Information` 级别**（比生产日志器捕获更多细节）
- 允许直接在应用程序 UI 中查看日志
- 支持人员可以访问它以远程排查用户问题

### 实现

### 访问诊断模态框

有**三种方法**可以打开诊断模态框：

1. 在运行中的应用标题栏的空白处**点击 7 次**
2. **按下** `Ctrl+Shift+X`（键盘快捷键）
3. 在浏览器开发者工具中**运行 JavaScript**：`App.showDiagnostic()`

### 诊断模态框 UI

**特定于环境的行为：**
- 诊断模态框显示来自内存中 `DiagnosticLogger.Store` 的**客户端日志**
- 这对于能够远程访问用户机器/设备以排查问题的支持人员非常有用

### 远程故障排除

对于**实时支持场景**，支持人员可以请求用户当前会话的诊断日志：

1. 支持人员打开用户页面并找到该用户
2. 点击“查看诊断日志”按钮
3. 服务器向用户的设备发送 SignalR 消息
4. 设备将其内存中的日志上传到服务器
5. 支持人员可以实时查看日志

这在 [`src/Server/AI.Boilerplate.Server.Api/Infrastructure/SignalR/AppHub.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/SignalR/AppHub.cs) 中实现：

```csharp
/// <inheritdoc cref="SharedAppMessages.UPLOAD_DIAGNOSTIC_LOGGER_STORE"/>
[Authorize(Policy = AppFeatures.System.ManageLogs)]
public async Task<DiagnosticLogDto[]> GetUserSessionLogs(Guid userSessionId, [FromServices] AppDbContext dbContext)
{
    ...
}
```

---

## 5. 与 Sentry 和 Azure Application Insights 的集成

本项目已**预配置**，可轻松集成流行的日志提供商。

### Sentry 集成

**Sentry** 是一个生产环境错误跟踪服务。在 `appsettings.json` 中配置：

```json
"Sentry": {
  "Dsn": "", // 在此处添加您的 Sentry DSN
  "SendDefaultPii": true,
  "EnableScopeSync": true,
  "LogLevel": {
    "Default": "Warning"
  }
}
```

### Azure Application Insights 集成

**Application Insights** 提供全面的遥测和监控。配置如下：

```json
"ApplicationInsights": {
  "ConnectionString": null // 在此处添加您的连接字符串
}
```

### 工作原理

1. 如果提供了连接字符串，OpenTelemetry 配置会自动导出到 Application Insights。

来自 [`src/Server/AI.Boilerplate.Server.Shared/Extensions/Infrastructure/WebApplicationBuilderExtensions.cs`](/src/Server/AI.Boilerplate.Server.Shared/Infrastructure/Extensions/WebApplicationBuilderExtensions.cs)：
2. Client.Core 项目中由 `BlazorApplicationInsights` NuGet 包添加的 Azure Application Insights JavaScript SDK 将从浏览器和 Blazor Hybrid 的 WebView 中收集 JavaScript 错误等信息。

### OpenTelemetry 配置

本项目跟踪：

**指标 (Metrics):**
- ASP.NET Core 仪器化 (HTTP 请求指标)
- HTTP 客户端仪器化
- 运行时仪器化 (GC, 线程池等)
- 通过 `Meter.Current` 的自定义指标

**追踪 (Tracing):**
- ASP.NET Core 请求 (排除静态文件和健康检查)
- HTTP 客户端调用
- Entity Framework Core 查询 (排除 Hangfire 查询)
- Hangfire 后台作业
- 通过 `ActivitySource.Current` 的自定义活动

---

## 6. Aspire 仪表板

**.NET Aspire 仪表板** 提供了所有日志、追踪和指标的统一视图。

### 什么是 Aspire 仪表板？

Aspire 仪表板是一个基于 Web 的 UI，显示：
- **日志**: 来自所有服务的所有记录消息
- **追踪**: 显示跨服务请求流的分布式追踪
- **指标**: 性能指标 (CPU, 内存, 请求率, 自定义指标)
- **资源**: 所有运行服务及其健康状况的概览

### 访问仪表板

当使用 .NET Aspire 运行项目时（通过 `AI.Boilerplate.Server.AppHost`），仪表板将自动在以下地址可用：

```
https://localhost:2198
```

### 关键特性

- **实时更新**: 实时查看日志和追踪
- **高级过滤**: 按级别、类别、服务、时间范围过滤日志
- **追踪可视化**: 查看请求如何在系统中流动
- **性能分析**: 识别缓慢操作和瓶颈

---

## 7. 健康检查

本项目包含**健康检查端点**以监控应用程序健康状况。

### 可用端点

1. **`/health`** - 所有健康检查必须通过
2. **`/alive`** - 仅标记为 "live" 的检查必须通过
3. **`/healthz`** - 详细健康报告 (UI 格式)

### 健康检查实现

来自 [`src/Server/AI.Boilerplate.Server.Shared/Infrastructure/Extensions/WebApplicationExtensions.cs`](/src/Server/AI.Boilerplate.Server.Shared/Infrastructure/Extensions/WebApplicationExtensions.cs)：

```csharp
public static WebApplication MapAppHealthChecks(this WebApplication app)
{
    // 在非开发环境中为应用程序启用健康检查端点存在安全隐患。
    // 在非开发环境中启用这些端点之前，请参阅 https://aka.ms/dotnet/aspire/healthchecks 了解详情。
    if (app.Environment.IsDevelopment())
    {
        var healthChecks = app.MapGroup("");

        healthChecks.CacheOutput("HealthChecks");

        // 所有健康检查必须通过，应用才被视为就绪
        healthChecks.MapHealthChecks("/health");

        // 仅标记为 "live" 的健康检查必须通过
        healthChecks.MapHealthChecks("/alive", new()
        {
            Predicate = static r => r.Tags.Contains("live")
        });

        // 带有 UI 的详细健康报告
        healthChecks.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    return app;
}
```

### 默认健康检查

来自 [`src/Server/AI.Boilerplate.Server.Shared/Infrastructure/Extensions/WebApplicationBuilderExtensions.cs`](/src/Server/AI.Boilerplate.Server.Shared/Infrastructure/Extensions/WebApplicationBuilderExtensions.cs)：

```csharp
public static IHealthChecksBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
    where TBuilder : IHostApplicationBuilder
{
    return builder.Services.AddHealthChecks()
        .AddDiskStorageHealthCheck(opt => 
            opt.AddDrive(Path.GetPathRoot(Directory.GetCurrentDirectory())!, 
            minimumFreeMegabytes: 5 * 1024), 
            tags: ["live"]);
}
```

这将检查是否至少有 **5GB 的可用磁盘空间**。

### 自定义健康检查示例

本项目在 [`src/Server/AI.Boilerplate.Server.Api/Infrastructure/Services/AppStorageHealthCheck.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Services/AppStorageHealthCheck.cs) 中包含了一个用于存储的自定义健康检查：

```csharp
/// <summary>
/// 检查底层的 S3、Azure Blob 存储或本地文件系统存储是否健康。
/// </summary>
public partial class AppStorageHealthCheck : IHealthCheck
{
    [AutoInject] private IBlobStorage blobStorage = default!;
    [AutoInject] private ServerApiSettings settings = default!;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _ = await blobStorage.ExistsAsync(settings.UserProfileImagesDir, cancellationToken);

            return HealthCheckResult.Healthy("存储健康");
        }
        catch (Exception exp)
        {
            return HealthCheckResult.Unhealthy("存储不健康", exp);
        }
    }
}
```

### 内置健康检查

本项目自动为 `AddServerApiProjectServices` 方法中注册的所有基础设施组件配置健康检查，包括：
- 数据库连接
- 磁盘存储可用性 (至少 5GB 空闲)
- Blob 存储 (S3, Azure Blob, 或本地文件系统)

---
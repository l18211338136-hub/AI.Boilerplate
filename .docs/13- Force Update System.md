# 第十三阶段：强制更新系统

欢迎回到 AI.Boilerplate 项目入门指南的**第十三阶段**！在本阶段，我们将探讨**强制更新系统 **(Force Update System) —— 这是一个关键功能，可确保所有用户都在运行与您应用程序兼容的版本。

---

## 概述

强制更新系统旨在维护客户端应用程序与后端服务器之间的版本兼容性。当您部署了破坏性变更 (breaking change) 到 API 时，您需要一种方法来确保旧版本的客户端停止工作，并提示用户更新。

**核心概念**：与仅在应用启动时进行的典型版本检查不同，该系统会**在每一次请求**中验证客户端版本。这意味着即使是正在积极使用应用的用户，如果其版本低于最低支持版本，也会被强制更新。

---

## 工作原理：架构概览

强制更新系统由四个主要组件组成：

1. **客户端版本头信息**：每个 HTTP 请求都包含客户端应用版本信息
2. **服务器端中间件**：验证版本，如果不支持则抛出异常
3. **异常处理**：捕获异常并发布强制更新消息
4. **特定于平台的更新逻辑**：每个平台以不同方式处理更新

让我们详细探讨每个组件。

---

## 1. 客户端：发送版本头信息

来自客户端的每个 HTTP 请求都会自动包含两个关键头信息：
- `X-App-Version`: 当前应用程序版本（例如 "1.2.0"）
- `X-App-Platform`: 平台类型（Android, iOS, Windows, Web, macOS）

这发生在 **`RequestHeadersDelegatingHandler`** 类中：

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Services/HttpMessageHandlers/RequestHeadersDelegatingHandler.cs`](/src/Client/AI.Boilerplate.Client.Core/Services/HttpMessageHandlers/RequestHeadersDelegatingHandler.cs)

```csharp
protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
{
    // ... 其他代码 ...
    
    var isInternalRequest = request.HasExternalApiAttribute() is false;
    if (isInternalRequest)
    {
        request.Headers.Add("X-App-Version", telemetryContext.AppVersion);
        request.Headers.Add("X-App-Platform", AppPlatform.Type.ToString());
    }
    
    return await base.SendAsync(request, cancellationToken);
}
```

**重要提示**：
- 这些头信息**仅添加到内部 API 调用**（对您自己后端的调用）
- 外部 API 调用不包含这些头信息，以避免 CORS 问题
- 这适用于**HTTP 请求和 SignalR 连接**（SignalR 在协商时使用 `HttpMessageHandlerFactory`）

### SignalR 集成

SignalR 连接也受益于该系统，因为它们使用相同的 `HttpMessageHandlerFactory`：

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Extensions/IClientCoreServiceCollectionExtensions.cs`](/src/Client/AI.Boilerplate.Client.Core/Extensions/IClientCoreServiceCollectionExtensions.cs)

```csharp
var hubConnection = new HubConnectionBuilder()
    .WithUrl(new Uri(absoluteServerAddressProvider.GetAddress(), "app-hub"), options =>
    {
        options.HttpMessageHandlerFactory = httpClientHandler => 
            sp.GetRequiredService<HttpMessageHandlersChainFactory>().Invoke(httpClientHandler);
        // ... 其他代码 ...
    })
    .Build();
```

这确保了即使是 SignalR 连接也会进行版本验证。

---

## 2. 服务器端：版本验证中间件

在服务器端，**`ForceUpdateMiddleware`** 会拦截每个请求并验证客户端版本。

**文件**: [`src/Server/AI.Boilerplate.Server.Api/RequestPipeline/ForceUpdateMiddleware.cs`](/src/Server/AI.Boilerplate.Server.Api/RequestPipeline/ForceUpdateMiddleware.cs)

```csharp
public class ForceUpdateMiddleware(RequestDelegate next, ServerApiSettings settings)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-App-Version", out var appVersionHeaderValue)
            && appVersionHeaderValue.Any())
        {
            var appVersion = appVersionHeaderValue.Single()!;
            var appPlatformType = Enum.Parse<AppPlatformType>(context.Request.Headers["X-App-Platform"].Single()!);
            var minVersion = settings.SupportedAppVersions!.GetMinimumSupportedAppVersion(appPlatformType);
            
            if (minVersion != null && Version.Parse(appVersion) < minVersion)
            {
                throw new ClientNotSupportedException();
            }
        }

        await next(context);
    }
}
```

### 工作原理：

1. 提取 `X-App-Version` 和 `X-App-Platform` 头信息
2. 从配置中获取该平台的最低支持版本
3. 将客户端版本与最低支持版本进行比较
4. 如果客户端版本**小于**最低版本，则抛出 `ClientNotSupportedException` 异常
5. 否则，允许请求继续

### 配置

最低支持版本在 **appsettings.json** 中配置：

**文件**: [`src/Server/AI.Boilerplate.Server.Api/appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json)

```json
"SupportedAppVersions": {
    "MinimumSupportedAndroidAppVersion": "1.0.0",
    "MinimumSupportedIosAppVersion": "1.0.0",
    "MinimumSupportedMacOSAppVersion": "1.0.0",
    "MinimumSupportedWindowsAppVersion": "1.0.0",
    "MinimumSupportedWebAppVersion": "1.0.0"
}
```

**何时更新这些值**：
- 当您部署破坏性 API 变更时
- 当您出于安全原因需要强制用户更新时
- 当您想停止支持旧版本时

### 设置类

配置在 **`ServerApiSettings`** 类中是强类型的：

**文件**: [`src/Server/AI.Boilerplate.Server.Api/ServerApiSettings.cs`](/src/Server/AI.Boilerplate.Server.Api/ServerApiSettings.cs)

```csharp
public class SupportedAppVersionsOptions
{
    public Version? MinimumSupportedAndroidAppVersion { get; set; }
    public Version? MinimumSupportedIosAppVersion { get; set; }
    public Version? MinimumSupportedMacOSAppVersion { get; set; }
    public Version? MinimumSupportedWindowsAppVersion { get; set; }
    public Version? MinimumSupportedWebAppVersion { get; set; }

    public Version? GetMinimumSupportedAppVersion(AppPlatformType platformType)
    {
        return platformType switch
        {
            AppPlatformType.Android => MinimumSupportedAndroidAppVersion,
            AppPlatformType.Ios => MinimumSupportedIosAppVersion,
            AppPlatformType.MacOS => MinimumSupportedMacOSAppVersion,
            AppPlatformType.Windows => MinimumSupportedWindowsAppVersion,
            AppPlatformType.Web => MinimumSupportedWebAppVersion,
            _ => throw new ArgumentOutOfRangeException(nameof(platformType))
        };
    }
}
```

---

## 3. 异常处理：触发更新流程

当抛出 `ClientNotSupportedException` 时，它会遵循特殊的处理路径。

### 异常类

**文件**: [`src/Shared/Exceptions/ClientNotSupportedException.cs`](/src/Shared/Exceptions/ClientNotSupportedException.cs)

```csharp
public partial class ClientNotSupportedException : BadRequestException
{
    public ClientNotSupportedException()
        : this(nameof(AppStrings.ForceUpdateTitle))
    {
    }
    // ... 其他构造函数 ...
}
```

### 客户端异常处理

当客户端收到此异常时，会在 **`ExceptionDelegatingHandler`** 中捕获它：

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Services/HttpMessageHandlers/ExceptionDelegatingHandler.cs`](/src/Client/AI.Boilerplate.Client.Core/Services/HttpMessageHandlers/ExceptionDelegatingHandler.cs)

```csharp
catch (ClientNotSupportedException)
{
    pubSubService.Publish(ClientAppMessages.FORCE_UPDATE, persistent: true);
    throw;
}
```

**关键点**：该异常作为**持久性消息**通过 PubSub 系统发布。这确保了向用户显示强制更新 UI。

### 异常日志记录

`ClientNotSupportedException` 被明确忽略记录，以防止产生噪音：

**文件**: [`src/Shared/Services/SharedExceptionHandler.cs`](/src/Shared/Services/SharedExceptionHandler.cs)

```csharp
public virtual bool IgnoreException(Exception exception)
{
    if (exception is ClientNotSupportedException)
        return true; // 参见 ExceptionDelegatingHandler

    // ... 其他代码 ...
}
```

---

## 4. 特定于平台的更新逻辑

每个平台都实现 **`IAppUpdateService`** 接口以不同地处理更新。

### 接口定义

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Services/Contracts/IAppUpdateService.cs`](/src/Client/AI.Boilerplate.Client.Core/Services/Contracts/IAppUpdateService.cs)

```csharp
public interface IAppUpdateService
{
    Task ForceUpdate();
}
```

### Web 平台：通过 Service Worker 自动更新

**文件**: [`src/Client/AI.Boilerplate.Client.Web/Infrastructure/Services/WebAppUpdateService.cs`](/src/Client/AI.Boilerplate.Client.Web/Infrastructure/Services/WebAppUpdateService.cs)

```csharp
public partial class WebAppUpdateService : IAppUpdateService
{
    [AutoInject] private IJSRuntime jsRuntime = default!;

    public async Task ForceUpdate()
    {
        const bool autoReload = true;
        await jsRuntime.InvokeVoidAsync("App.tryUpdatePwa", autoReload);
    }
}
```

**工作原理**：
- 调用一个 JavaScript 函数，触发 service worker 进行更新
- `autoReload = true` 参数强制页面在下载更新后重新加载
- 新版本从服务器获取并由 service worker 缓存
- 页面自动重新加载新版本

### Windows 平台：通过 Velopack 自动更新

**文件**: [`src/Client/AI.Boilerplate.Client.Windows/Services/WindowsAppUpdateService.cs`](/src/Client/AI.Boilerplate.Client.Windows/Services/WindowsAppUpdateService.cs)

```csharp
public partial class WindowsAppUpdateService : IAppUpdateService
{
    [AutoInject] private ClientWindowsSettings settings = default!;

    public async Task ForceUpdate()
    {
        var windowsUpdateSettings = settings.WindowsUpdate;
        if (string.IsNullOrEmpty(windowsUpdateSettings?.FilesUrl))
            return;
        windowsUpdateSettings.AutoReload = true; // 强制更新在更新后重新加载应用
        await Update();
    }

    public async Task Update()
    {
        var windowsUpdateSettings = settings.WindowsUpdate;
        if (string.IsNullOrEmpty(windowsUpdateSettings?.FilesUrl))
            return;
        var updateManager = new UpdateManager(windowsUpdateSettings.FilesUrl);
        var updateInfo = await updateManager.CheckForUpdatesAsync();
        if (updateInfo is not null)
        {
            await updateManager.DownloadUpdatesAsync(updateInfo);
            if (windowsUpdateSettings.AutoReload)
            {
                updateManager.ApplyUpdatesAndRestart(updateInfo, Environment.GetCommandLineArgs());
            }
        }
    }
}
```

**工作原理**：
- 使用 **Velopack** 从配置的 URL 检查更新
- 在后台下载新版本
- 自动使用新版本重启应用程序

### Android, iOS, macOS: 打开应用商店

**文件**: [`src/Client/AI.Boilerplate.Client.Maui/Services/MauiAppUpdateService.cs`](/src/Client/AI.Boilerplate.Client.Maui/Services/MauiAppUpdateService.cs)

```csharp
public partial class MauiAppUpdateService : IAppUpdateService
{
    public async Task ForceUpdate()
    {
        await AppStoreInfo.Current.OpenApplicationInStoreAsync();
    }
}
```

**工作原理**：
- 打开相应的应用商店（Google Play Store, Apple App Store, 或 Mac App Store）
- 直接导航到您应用的页面
- 用户必须手动下载并安装更新

---

## 5. 用户界面：强制更新 Snackbar

当发布强制更新消息时，会向用户显示一个 Snackbar（消息提示框）。

### Snackbar 组件

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Components/Layout/ForceUpdateSnackBar.razor`](/src/Client/AI.Boilerplate.Client.Core/Components/Layout/ForceUpdateSnackBar.razor)

```xml
<BitSnackBar @ref="bitSnackBar" AutoDismiss="false" Persistent>
    <TitleTemplate>
        <BitStack FitHeight>
            <BitText Typography="BitTypography.H5">
                @Localizer[nameof(AppStrings.ForceUpdateTitle)]
            </BitText>
        </BitStack>
    </TitleTemplate>
    <BodyTemplate>
        <BitStack>
            <BitText Typography="BitTypography.Body1">
                @Localizer[nameof(AppStrings.ForceUpdateBody)]
            </BitText>
            <BitButton Color="BitColor.Tertiary"
                       OnClick="WrapHandled(Update)"
                       IconName="@BitIconName.Download">
                @Localizer[nameof(AppStrings.Update)]
            </BitButton>
        </BitStack>
    </BodyTemplate>
</BitSnackBar>
```

### Snackbar 代码后置

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Components/Layout/ForceUpdateSnackBar.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Layout/ForceUpdateSnackBar.razor.cs)

```csharp
public partial class ForceUpdateSnackBar
{
    [AutoInject] private IAppUpdateService appUpdateService = default!;

    private bool isShown;
    private Action? unsubscribe;
    private BitSnackBar bitSnackBar = default!;

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        if (InPrerenderSession) return;

        unsubscribe = PubSubService.Subscribe(ClientAppMessages.FORCE_UPDATE, async (_) =>
        {
            if (isShown) return;

            isShown = true;
            await bitSnackBar.Error(string.Empty);
        });
    }

    private async Task Update()
    {
        await appUpdateService.ForceUpdate();
    }

    protected override async ValueTask DisposeAsync(bool disposing)
    {
        unsubscribe?.Invoke();
        await base.DisposeAsync(disposing);
    }
}
```

**关键特性**：
- 订阅 `FORCE_UPDATE` pub-sub 消息
- 收到消息时显示 Snackbar
- 防止显示多个 Snackbar（`isShown` 标志）
- Snackbar 是持久性的，用户无法关闭它
- 点击“更新”按钮会调用特定于平台的更新逻辑

---

## 关键区别：全天候验证

**🚨 最重要的概念**：该系统**在每一次请求**中验证客户端版本，而不仅仅是在应用启动时。

### 为什么这很重要

想象这样一个场景：

1. 用户在上午 9:00 使用版本 1.0.0 打开您的应用
2. 上午 10:00，您部署了一个破坏性变更，并将 `MinimumSupportedWebAppVersion` 设置为 1.1.0
3. 用户在上午 10:05 仍在积极使用该应用

**会发生什么**？
- 用户发出的下一个 API 请求将立即因 `ClientNotSupportedException` 而失败
- 强制更新 Snackbar 会立即出现
- 用户**必须**更新才能继续使用应用

**传统方法**（仅在启动时验证）：
- 用户可以继续使用 1.0.0 版本数小时甚至数天
- 他们只有在重启应用时才会看到更新提示
- 如果 API 已更改，这可能导致数据损坏或错误

---

## 配置最佳实践

### 何时增加最低版本

✅ **应该增加时**：
- 您以破坏性方式更改了 API 响应/请求 DTO
- 您移除或重命名了端点
- 您更改了认证/授权逻辑
- 您修复了关键的安全漏洞
- 您以不兼容的方式迁移了数据库架构

❌ **不应该增加时**：
- 您向 DTO 添加了新的可选字段
- 您添加了新的端点（而未移除旧端点）
- 您修复了不影响 API 契约的 bug
- 您仅更改了 UI 代码

### 版本号策略

使用语义化版本控制：`MAJOR.MINOR.PATCH` (主版本。次版本。修订号)

- **MAJOR **(主版本): 破坏性变更（需要强制更新）
- **MINOR **(次版本): 新功能（向后兼容）
- **PATCH **(修订号): Bug 修复（向后兼容）

示例：
```json
"SupportedAppVersions": {
    "MinimumSupportedWebAppVersion": "2.0.0",  // 主版本变更
    "MinimumSupportedAndroidAppVersion": "1.5.0", // Android 可以滞后
    "MinimumSupportedIosAppVersion": "1.5.0"     // iOS 可以滞后
}
```

---

## 高级场景

### 逐步 rollout (灰度发布)

您可以为不同平台设置不同的最低版本，以实现逐步发布：

```json
"SupportedAppVersions": {
    "MinimumSupportedWebAppVersion": "2.0.0",      // 已经发布
    "MinimumSupportedAndroidAppVersion": "1.5.0",  // 仍在使用旧版本
    "MinimumSupportedIosAppVersion": "1.5.0"       // 仍在使用旧版本
}
```

这允许您：
1. 首先将破坏性变更部署到 Web 应用
2. 在强制移动用户更新之前先与 Web 用户进行测试
3. 给移动用户更多时间更新（应对应用商店审批延迟）

---
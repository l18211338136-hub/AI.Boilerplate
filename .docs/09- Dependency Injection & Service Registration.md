# 第九阶段：依赖注入与服务注册

本阶段解释了依赖注入 (DI) 在 AI.Boilerplate 项目中的结构，以及针对不同平台和环境在哪里注册服务。

## 概述

本项目遵循**模块化服务注册模式**，服务根据其作用域和适用性在不同的扩展方法中注册。这种方法确保了：

- **关注点分离**：每个项目只注册其需要的服务
- **特定于平台的实现**：不同平台可以提供自己的服务实现
- **共享服务**：通用服务只需注册一次，即可在所有地方使用

## 服务注册架构

### 扩展方法文件

服务注册通过解决方案中的 `*ServiceCollectionExtensions.cs` 和 `*.Services.cs` 文件进行组织：

1. **`ISharedServiceCollectionExtensions.cs`** ([`src/Shared/Extensions/`](/src/Shared/Extensions/ISharedServiceCollectionExtensions.cs))
   - 注册**服务器端和客户端**项目共用的服务
   - 核心服务如本地化、授权、配置和日期/时间提供者
   - 包含定义授权策略的关键方法 `ConfigureAuthorizationCore()`
   
   **注册的关键服务：**
   ```csharp
   services.AddScoped<HtmlRenderer>();
   services.AddScoped<CultureInfoManager>();
   services.AddScoped<IDateTimeProvider, DateTimeProvider>();
   services.AddSingleton<SharedSettings>();
   services.AddLocalization();
   services.AddMemoryCache();
   ```

2. **`IClientCoreServiceCollectionExtensions.cs`** ([`src/Client/AI.Boilerplate.Client.Core/Extensions/`](/src/Client/AI.Boilerplate.Client.Core/Extensions/IClientCoreServiceCollectionExtensions.cs))
   - 为**所有客户端平台** (Web, MAUI, Windows) 注册服务
   - 在预渲染、Blazor Server 和 Blazor WebAssembly 期间可用的服务
   - 大多数客户端基础设施服务在此处注册
   
   **注册的关键服务：**
   ```csharp
   services.AddScoped<ThemeService>();
   services.AddScoped<CultureService>();
   services.AddScoped<LazyAssemblyLoader>();
   services.AddScoped<IAuthTokenProvider, ClientSideAuthTokenProvider>();
   services.AddScoped<IExternalNavigationService, DefaultExternalNavigationService>();
   
   // 基于会话的服务（在 Hybrid 中为 Singleton，在 Server/WASM 中为 Scoped）
   services.AddSessioned<PubSubService>();
   services.AddSessioned<PromptService>();
   services.AddSessioned<SnackBarService>();
   ```

3. 每个项目中的 **`Program.Services.cs`** 文件
   - 特定于平台的服务注册
   - 每个平台注册其共享接口的自有实现
   
   | 文件 | 用途 |
   |------|---------|
   | `AI.Boilerplate.Server.Api/Program.Services.cs` | API 服务器服务 (DbContext, Identity, Email, SMS, Push Notifications, AI, Hangfire) |
   | `AI.Boilerplate.Server.Web/Program.Services.cs` | Blazor Server/SSR 服务 (结合 API + 客户端服务) |
   | `AI.Boilerplate.Client.Web/Program.Services.cs` | Blazor WebAssembly 特定服务 |
   | `AI.Boilerplate.Client.Maui/MauiProgram.Services.cs` | MAUI 服务 (Android, iOS, macOS, Windows via MAUI) |
   | `AI.Boilerplate.Client.Windows/Program.Services.cs` | Windows Forms Blazor Hybrid 服务 |

4. **特定于平台的扩展** (仅限 MAUI)
   - 每个移动/桌面平台可以注册其专用的服务
   
   | 扩展文件 | 平台 |
   |---------------|----------|
   | [`IAndroidServiceCollectionExtensions.cs`](/src/Client/AI.Boilerplate.Client.Maui/Platforms/Android/Extensions/IAndroidServiceCollectionExtensions.cs) | Android 特定服务 (例如 `AndroidPushNotificationService`) |
   | [`IIosServiceCollectionExtensions.cs`](/src/Client/AI.Boilerplate.Client.Maui/Platforms/iOS/Extensions/IIosServiceCollectionExtensions.cs) | iOS 特定服务 |
   | [`IMacServiceCollectionExtensions.cs`](/src/Client/AI.Boilerplate.Client.Maui/Platforms/MacCatalyst/Extensions/IMacServiceCollectionExtensions.cs) | macOS 特定服务 |
   | [`IWindowsServiceCollectionExtensions.cs`](/src/Client/AI.Boilerplate.Client.Maui/Platforms/Windows/Extensions/IWindowsServiceCollectionExtensions.cs) | Windows (MAUI) 特定服务 |

## 服务注册层级

服务以层级方式注册，每一层都建立在前一层的基础上：

```
Program.Services.cs (特定于平台)
    ↓
IClientCoreServiceCollectionExtensions.cs (所有客户端)
    ↓
ISharedServiceCollectionExtensions.cs (服务器 + 客户端)
```

### 注册流程示例 (Blazor WebAssembly):

1. **`Program.cs`** 调用 `ConfigureServices()`
2. **`Program.Services.cs`** 调用 `AddClientWebProjectServices()`
3. **`AddClientWebProjectServices()`** 调用 `AddClientCoreProjectServices()`
4. **`AddClientCoreProjectServices()`** 调用 `AddSharedProjectServices()`

每一层都添加特定于其作用域的服务，确保整个应用程序中服务的正确可用性。

### 注册流程示例 (Server API):

1. **`Program.cs`** 调用 `AddServerApiProjectServices()`
2. **`AddServerApiProjectServices()`** 内部调用辅助方法如 `AddIdentity()`
3. 每个辅助方法注册相关服务 (例如 Identity 服务、JWT 认证、外部认证提供商)

## 服务可用性矩阵

此矩阵显示了每种类型的服务注册在何处可用：

| 注册位置 | Web (WASM) | Web (Server/SSR) | MAUI (Android/iOS/Mac) | Windows Forms | Server API |
|----------------------|------------|------------------|------------------------|---------------|------------|
| `ISharedServiceCollectionExtensions` | ✅ | ✅ | ✅ | ✅ | ✅ |
| `IClientCoreServiceCollectionExtensions` | ✅ | ✅ | ✅ | ✅ | ❌ |
| `Program.Services.cs` (Client.Web) | ✅ | ✅ | ❌ | ❌ | ❌ |
| `MauiProgram.Services.cs` | ❌ | ❌ | ✅ | ❌ | ❌ |
| `Program.Services.cs` (Client.Windows) | ❌ | ❌ | ❌ | ✅ | ❌ |
| `Program.Services.cs` (Server.Api) | ❌ | ❌ | ❌ | ❌ | ✅ |
| `Program.Services.cs` (Server.Web) | ❌ | ✅ | ❌ | ❌ | ✅ (组合) |
| 特定于平台 (Android) | ❌ | ❌ | ✅ (仅 Android) | ❌ | ❌ |
| 特定于平台 (iOS) | ❌ | ❌ | ✅ (仅 iOS) | ❌ | ❌ |
| 特定于平台 (Mac) | ❌ | ❌ | ✅ (仅 Mac) | ❌ | ❌ |
| 特定于平台 (Windows/MAUI) | ❌ | ❌ | ✅ (仅 Windows/MAUI) | ❌ | ❌ |

**注意：** `Server.Web` 获取**两者** (Server API 服务和 Client Web 服务)，因为它托管 Blazor Server/SSR，这需要后端 API 和前端组件。

## `AddSessioned` 方法

本项目中最重要的概念之一是 **`AddSessioned`** 扩展方法，它根据应用程序模式智能地注册服务。

### 什么是 AddSessioned？

`AddSessioned` 是一个自定义扩展方法，可根据托管环境自动选择正确的服务生命周期：

- **Blazor Hybrid (MAUI, Windows Forms)**: 注册为 `Singleton` (单例)
- **Blazor WebAssembly / Blazor Server**: 注册为 `Scoped` (作用域)

### 为什么需要 AddSessioned？

某些服务必须是**每个用户会话/客户端应用唯一**的，但“会话”的定义因平台而异：

- **Blazor Server** 为每个连接的用户的客户端应用创建一个作用域。
- **Blazor WebAssembly** 仅为客户端应用创建一个作用域。
- **MAUI + Blazor Hybrid** 会为客户端应用创建两个作用域：一个用于原生部分，一个用于 Blazor 的 WebView。
- 在 **Blazor Hybrid** 中，我们将基于会话的服务注册为单例，以便在两个作用域之间共享它们。
- 在 **Blazor Server** 中，我们**必须**将基于会话的服务注册为作用域，以避免在不同用户之间共享它们。
- 在 **Blazor WebAssembly** 中，将基于会话的服务注册为作用域或单例无关紧要，因为每个客户端应用只有一个作用域。

### AddSessioned 实现

位于 [`IClientCoreServiceCollectionExtensions.cs`](/src/Client/AI.Boilerplate.Client.Core/Extensions/IClientCoreServiceCollectionExtensions.cs)：

```csharp
internal static IServiceCollection AddSessioned<TService, TImplementation>(this IServiceCollection services)
    where TImplementation : class, TService
    where TService : class
{
    if (AppPlatform.IsBlazorHybrid)
    {
        return services.AddSingleton<TService, TImplementation>();
    }
    else
    {
        return services.AddScoped<TService, TImplementation>();
    }
}
```

这些服务需要基于会话，因为它们维护特定于客户端的状态：
- **`PubSubService`**: 特定于客户端应用的发布/订阅消息
- **`AuthManager`**: 客户端应用用户的认证状态和令牌

## 服务注册的关键规则

### 1. 特定于平台的实现

当您需要每个平台有不同的实现时 (例如 `IPushNotificationService`)：

1. 在 `AI.Boilerplate.Shared` 或 `AI.Boilerplate.Client.Core` 中**定义接口**
2. 在每个平台项目中**创建特定于平台的实现**
3. 在每个平台的服务注册文件中**注册适当的实现**

**示例：**

| 平台 | 实现 | 注册位置 |
|----------|---------------|---------------|
| Android | `AndroidPushNotificationService` | `IAndroidServiceCollectionExtensions.cs` |
| iOS | `IosPushNotificationService` | `IIosServiceCollectionExtensions.cs` |
| Web | `WebPushNotificationService` | `Program.Services.cs` (Client.Web) |
| Windows Forms | `WindowsPushNotificationService` | `Program.Services.cs` (Client.Windows) |

这允许每个平台在共享同一接口的同时提供自己的原生实现。

## 示例：添加新服务

假设您想添加一个在所有客户端平台上都能工作的 `FeedbackService`：

### 步骤 1：创建服务

```csharp
// src/Client/AI.Boilerplate.Client.Core/Services/FeedbackService.cs
namespace AI.Boilerplate.Client.Core.Services;

public partial class FeedbackService
{
    [AutoInject] private IFeedbackController feedbackController = default!;
    [AutoInject] private ILogger<FeedbackService> logger = default!;

    public async Task SendFeedbackAsync(string message, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("正在发送用户反馈...");
        
        await feedbackController.SendFeedback(new FeedbackDto { Message = message }, cancellationToken);
        
        logger.LogInformation("反馈发送成功。");
    }
}
```

### 步骤 2：注册服务

```csharp
// src/Client/AI.Boilerplate.Client.Core/Extensions/IClientCoreServiceCollectionExtensions.cs
public static IServiceCollection AddClientCoreProjectServices(this IServiceCollection services, IConfiguration configuration)
{
    // ... 现有服务 ...
    
    // 注册为 Sessioned，因为反馈状态应该是每个用户独有的
    services.AddSessioned<FeedbackService>();
    
    return services;
}
```

### 步骤 3：在组件中使用服务

```csharp
// 在任何组件或页面中
public partial class FeedbackPage : AppPageBase
{
    [AutoInject] private FeedbackService feedbackService = default!;

    private async Task OnSubmitFeedback(string message)
    {
        try
        {
            await feedbackService.SendFeedbackAsync(message);
            SnackBarService.Success(Localizer[nameof(AppStrings.FeedbackSentSuccessfully)]);
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
    }
}
```

## 自有服务 (Owned Services)

### Blazor 组件中的默认服务生命周期

默认情况下，注入到 Blazor 组件中的服务在整个生命周期内都与应用程序作用域绑定：

- **Blazor Server**: 直到用户关闭浏览器标签页或浏览器断开连接。
- **Blazor WebAssembly / Blazor Hybrid**: 直到浏览器标签页或应用关闭。

这对于大多数服务（尤其是单例或无状态服务）来说完全没问题，但对于持有资源（定时器、事件订阅、原生句柄等）的服务，可能需要在关联组件销毁时进行处置。

### 使用组件作用域服务进行自动处置

为了在组件处置时实现自动处置，请通过 `ScopedServices` 注入服务，而不是使用 `[AutoInject]`。这会创建一个作用域服务实例，该实例会随组件一起被处置。

**示例：**

```csharp
Keyboard keyboard => field ??= ScopedServices.GetRequiredService<Keyboard>(); // ??= 意味着服务在访问时被解析，从而提高性能。

protected override async Task OnAfterFirstRenderAsync()
{
    await keyboard.Add(ButilKeyCodes.KeyF, () => searchBox.FocusAsync(), ButilModifiers.Ctrl); // 处理键盘快捷键

    await base.OnAfterFirstRenderAsync();
}
```

而不是：

```csharp
[AutoInject] private Keyboard keyboard = default!;

protected override async Task OnAfterFirstRenderAsync()
{
    await keyboard.Add(ButilKeyCodes.KeyF, () => searchBox.FocusAsync(), ButilModifiers.Ctrl); // 处理键盘快捷键

    await base.OnAfterFirstRenderAsync();
}

protected override async ValueTask DisposeAsync(bool disposing)
{
    await keyboard.DisposeAsync();
    await base.DisposeAsync(disposing);
}
```

---

### AI Wiki: 已回答的问题
* [HttpClient 如何在不同平台和 Blazor 托管模式下创建？](https://deepwiki.com/search/how-is-the-httpclient-created_0f4353a6-bf0e-47cc-afbc-bf96aaf97469)

在此处提出您自己的问题：[https://wiki.bitplatform.dev](https://wiki.bitplatform.dev)

---
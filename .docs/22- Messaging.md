# 第二十二阶段：消息传递

欢迎回到第二十二阶段！在本阶段，您将了解 AI.Boilerplate 项目中构建的综合**消息传递和实时通信系统**。该系统提供统一的消息架构，支持跨多个渠道和平台进行通信。

---

## 1. AppMessages - 集中式消息系统

### 概述

AI.Boilerplate 消息架构的核心是 **AppMessages** —— 一个集中式消息系统，它为应用程序不同部分之间的通信提供了一致的方式，无论通信发生在：

- 客户端的 C# 组件之间
- 通过 SignalR 从服务器到客户端
- 从 JavaScript 到 C# 代码
- 从 Web Service Worker 到 C# 代码

**位置**: [`src/Shared/Infrastructure/Services/SharedAppMessages.cs`](/src/Shared/Infrastructure/Services/SharedAppMessages.cs)

**位置**: [`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/ClientAppMessages.cs`](/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/ClientAppMessages.cs)

### 消息结构

**SharedAppMessages** (服务器 ↔ 客户端):

```csharp
public partial class SharedAppMessages
{
    // 数据变更通知
    public const string DASHBOARD_DATA_CHANGED = nameof(DASHBOARD_DATA_CHANGED);
    
    // 会话管理
    public const string SESSION_REVOKED = nameof(SESSION_REVOKED);
    
    // 个人资料更新
    public const string PROFILE_UPDATED = nameof(PROFILE_UPDATED);
    
    // 导航和 UI 变更
    public const string NAVIGATE_TO = nameof(NAVIGATE_TO);
    public const string CHANGE_CULTURE = nameof(CHANGE_CULTURE);
    public const string CHANGE_THEME = nameof(CHANGE_THEME);
    
    // ... 等等
}
```

**ClientAppMessages** (仅客户端):

**位置**: [`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/ClientAppMessages.cs`](/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/ClientAppMessages.cs)

```csharp
public partial class ClientAppMessages : SharedAppMessages
{    
    // 主题和文化
    public const string THEME_CHANGED = nameof(THEME_CHANGED);
    public const string CULTURE_CHANGED = nameof(CULTURE_CHANGED);
    
    // 诊断
    public const string SHOW_DIAGNOSTIC_MODAL = nameof(SHOW_DIAGNOSTIC_MODAL);
    
    // ... 等等
}
```

**注意**: `ClientAppMessages` 继承自 `SharedAppMessages`，因此客户端代码可以访问共享消息和仅客户端消息。

---

## 2. 通信渠道

AI.Boilerplate 项目提供多种通信渠道，所有渠道都使用相同的集中式消息系统。让我们探索每个渠道并了解何时使用它们。

### 渠道 1: PubSubService (客户端组件通信)

**PubSubService** 是客户端消息传递的基础。它实现了发布/订阅模式，用于组件间的解耦通信。

**位置**: [`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/PubSubService.cs`](/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/PubSubService.cs)

**使用时机**:
- Blazor 组件之间的通信
- 与非 Blazor 组件的通信（例如 MAUI XAML 页面）
- 广播 UI 状态变更
- 触发不相关组件的操作

**发布消息**:

```csharp
// 从任何组件或服务
PubSubService.Publish(ClientAppMessages.THEME_CHANGED, newTheme);
```

**订阅消息**:

```csharp
// 在组件代码中
private Action? unsubscribe;

protected override void OnInitialized()
{
    unsubscribe = PubSubService.Subscribe(ClientAppMessages.THEME_CHANGED, async payload =>
    {
        currentTheme = (string)payload;
        await InvokeAsync(StateHasChanged);
    });
}

protected override void Dispose(bool disposing)
{
    unsubscribe?.Invoke();
    base.Dispose(disposing);
}
```

**持久化消息**:

如果 `persistent = true`，消息将被存储并传递给**在消息发布后订阅**的处理程序：

```csharp
PubSubService.Publish(ClientAppMessages.PROFILE_UPDATED, user, persistent: true);
```

### 渠道 2: SignalR (服务器到客户端的实时通信)

**SignalR** 使服务器能够实时向客户端发送消息。在 AI.Boilerplate 项目中，SignalR 消息会自动桥接到 PubSubService，创造无缝体验。

**服务器端 Hub**: [`src/Server/AI.Boilerplate.Server.Api/Infrastructure/SignalR/AppHub.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/SignalR/AppHub.cs)

**使用时机**:
- 通知客户端数据变更
- 向所有用户或特定用户广播更新
- 推送实时通知
- 跨多个设备同步状态

**从服务器发布到所有已认证的客户端**:

```csharp
// 通知所有已认证用户仪表板数据已变更
await appHubContext.Clients.Group("AuthenticatedClients")
    .Publish(SharedAppMessages.DASHBOARD_DATA_CHANGED, null, cancellationToken);
```

**从服务器发布到特定用户的所有设备**:

```csharp
// 当用户在一个设备上更新个人资料时，通知其所有其他设备
await appHubContext.Clients.User(userId.ToString())
    .Publish(SharedAppMessages.PROFILE_UPDATED, userDto, cancellationToken);
```

**客户端接收**:

在客户端，SignalR 消息会自动桥接到 PubSubService（参见 `AppClientCoordinator.cs`）。这意味着任何组件都可以使用 PubSubService 订阅这些消息：

```csharp
// 组件以与仅客户端消息相同的方式订阅服务器发送的消息
unsubscribe = PubSubService.Subscribe(SharedAppMessages.DASHBOARD_DATA_CHANGED, async (_) =>
{
    await LoadDashboardData();
    await InvokeAsync(StateHasChanged);
});
```

### 渠道 3: AppJsBridge (JavaScript 到 C# 通信)

**AppJsBridge** 使 JavaScript 代码能够向 C# PubSubService 发布消息。

**位置**: [`src/Client/AI.Boilerplate.Client.Core/Components/Layout/AppJsBridge.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Layout/AppJsBridge.razor.cs)

**使用时机**:
- 集成第三方 JavaScript 库
- 处理需要 C# 处理的浏览器事件
- 从 JavaScript 调用 C# 代码

**从 JavaScript 发布**:

```javascript
// 从任何 JavaScript 代码
App.publishMessage('CUSTOM_EVENT', { data: 'some data' });

// 显示诊断模态框
App.showDiagnostic(); // 发布 SHOW_DIAGNOSTIC_MODAL 消息
```

**工作原理**:

```csharp
// AppJsBridge.razor.cs
[JSInvokable(nameof(PublishMessage))]
public async Task PublishMessage(string message, string? payload)
{
    // JavaScript 消息发布到 PubSubService
    PubSubService.Publish(message, payload);
}
```

### 渠道 4: window.postMessage (跨上下文 JavaScript 通信)

`window.postMessage` API 允许不同的 JavaScript 上下文（例如 iframes、service workers）之间进行通信。AI.Boilerplate 项目将其桥接到 PubSubService。

**位置**: [`src/Client/AI.Boilerplate.Client.Core/Scripts/events.ts`](/src/Client/AI.Boilerplate.Client.Core/Scripts/events.ts)

**使用时机**:
- 来自 iframes 的通信
- 与第三方脚本集成
- 跨源消息传递

**通过 window.postMessage 发布**:

```javascript
// 从任何 JavaScript 上下文（包括 iframes）
window.postMessage({ 
    key: 'PUBLISH_MESSAGE', 
    message: 'CUSTOM_EVENT', 
    payload: { data: 'value' } 
}, '*');
```

**工作原理**:

```typescript
// events.ts
window.addEventListener('message', handleMessage);

function handleMessage(e: MessageEvent) {
    if (e.data?.key === 'PUBLISH_MESSAGE') {
        // 通过 AppJsBridge 桥接到 C# PubSubService
        App.publishMessage(e.data?.message, e.data?.payload);
    }
}
```

### 渠道 5: Service Worker (后台消息处理)

Service workers 可以使用相同的消息系统与 Blazor 应用程序通信。这对于处理推送通知点击特别有用。

**位置**: [`src/Client/AI.Boilerplate.Client.Web/wwwroot/service-worker.js`](/src/Client/AI.Boilerplate.Client.Web/wwwroot/service-worker.js)

**使用时机**:
- 处理推送通知点击
- 后台同步
- 缓存管理通知

**从 service worker 发布**:

```javascript
// service-worker.js
self.addEventListener('notificationclick', (event) => {
    const pageUrl = event.notification.data?.pageUrl;
    // ...
    // 发送 NAVIGATE_TO 消息以在应用中打开特定页面
    client.postMessage({ 
        key: 'PUBLISH_MESSAGE', 
        message: 'NAVIGATE_TO', 
        payload: pageUrl 
    });
    return client.focus();
});
```

**工作原理**:

```typescript
// events.ts - 监听 service worker 消息
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.addEventListener('message', handleMessage);
}

function handleMessage(e: MessageEvent) {
    if (e.data?.key === 'PUBLISH_MESSAGE') {
        // 桥接到 C# PubSubService
        App.publishMessage(e.data?.message, e.data?.payload);
    }
}
```

---

## 3. SignalR 详情：SendAsync vs InvokeAsync

理解何时使用 `SendAsync`（或其包装器 `Publish`）与 `InvokeAsync` 对于可靠的服务器到客户端通信至关重要。

### 何时使用 InvokeAsync

当以下情况时使用 `InvokeAsync`：
- 向**特定 SignalR 连接 ID** 发送消息
- **需要知道**消息是否到达客户端
- 需要确认操作成功完成
- 正在等待客户端响应

**示例**:

```csharp
// 服务器需要知道消息是否成功显示给用户
var messageShown = await appHubContext.Clients.Client(userSession.SignalRConnectionId)
    .InvokeAsync<bool>(SharedAppMessages.SHOW_MESSAGE, message, null, cancellationToken);

if (messageShown)
{
    // 消息已成功显示给用户
}
```

**重要提示**: 要使 `InvokeAsync` 工作，客户端的 `hubConnection.On` 监听器必须在 `AppClientCoordinator.cs` 的 `SubscribeToSignalRSharedAppMessages` 方法中注册，并且**必须返回值**，即使是一个简单的 `true` 常量：

```csharp
// AppClientCoordinator.cs 中的客户端代码
hubConnection.On<string, Dictionary<string, string?>?, bool>(SharedAppMessages.SHOW_MESSAGE, async (message, data) =>
{
    logger.LogInformation("收到来自服务器的 SignalR 消息 {Message} 以显示。", message);
    
    await ShowNotificationOrSnack(message, data);
    
    return true;
});
```

### 何时使用 SendAsync (或 Publish)

当以下情况时使用 `SendAsync` 或 `Publish` 扩展方法：
- 向多个客户端广播（例如 `Clients.All()`、`Clients.Group()`、`Clients.User()`）
- 可以接受"发后即忘"的消息传递
- 不需要确认送达

**示例**:

```csharp
// 通知所有已认证客户端 - 无需等待确认
await appHubContext.Clients.Group("AuthenticatedClients").SendAsync(SharedAppMessages.PUBLISH_MESSAGE, SharedAppMessages.DASHBOARD_DATA_CHANGED, null, cancellationToken);

// 或者：使用 Publish 扩展方法简化（内部使用 SendAsync）
await appHubContext.Clients.Group("AuthenticatedClients").Publish(SharedAppMessages.DASHBOARD_DATA_CHANGED, null, cancellationToken);
```

**注意**: `Publish` 扩展方法内部使用 `SendAsync`，具有相同的"发后即忘"行为。`SendAsync` 和 `Publish` 都**无需在 `AppClientCoordinator.cs` 中进行特殊注册**即可工作 —— 它们通过 `PUBLISH_MESSAGE` 处理程序自动桥接到 PubSubService。

### SignalR 消息目标

服务器可以向不同的目标发送消息：

1. **`Clients.All()`**: 所有 SignalR 连接（无论是否已认证）
2. **`Clients.Group("AuthenticatedClients")`**: 所有已认证用户（他们的所有设备）
3. **`Clients.User(userId)`**: 特定用户的所有设备（Web、移动、桌面）
4. **`Clients.Client(connectionId)`**: 特定连接（一个浏览器标签页或应用实例）

---

## 4. AppClientCoordinator - 协调一切

**AppClientCoordinator** 负责在应用程序启动时初始化和协调所有消息服务。

**位置**: [`src/Client/AI.Boilerplate.Client.Core/Components/AppClientCoordinator.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/AppClientCoordinator.cs)

### 主要职责

1. **初始化 SignalR 连接**
2. **订阅 SignalR 事件**（通过 `SubscribeToSignalRSharedAppMessages` 方法）
3. **管理身份验证状态传播**
4. **处理推送通知订阅**
5. **协调遥测服务**

### SignalR 事件订阅

`SubscribeToSignalRSharedAppMessages` 方法注册 SignalR 消息的处理程序：

#### PUBLISH_MESSAGE 处理程序

将 SignalR 消息桥接到 PubSubService：

```csharp
hubConnection.On(SharedAppMessages.PUBLISH_MESSAGE, async (string message, object? payload) =>
{
    logger.LogInformation("收到来自服务器的 SignalR 消息 {Message} 以发布。", message);
    PubSubService.Publish(message, payload);
    return true;
});
```

这是使服务器端代码能够发布消息而客户端组件可以订阅的基础。

### 身份验证状态传播

当以下情况时调用 `PropagateAuthState` 方法：
1. 应用程序首次启动
2. 用户登录或登出

```csharp
public async Task PropagateAuthState(bool firstRun, Task<AuthenticationState> task)
{
    var user = (await task).User;
    var isAuthenticated = user.IsAuthenticated();
    var userId = isAuthenticated ? user.GetUserId() : (Guid?)null;
    
    if (lastPropagatedUserId == userId)
        return;
        
    // 更新遥测上下文
    TelemetryContext.UserId = userId;
    TelemetryContext.UserSessionId = isAuthenticated ? user.GetSessionId() : null;

    // 更新 App Insights
    if (isAuthenticated)
        _ = appInsights.SetAuthenticatedUserContext(user.GetUserId().ToString());
    else
        _ = appInsights.ClearAuthenticatedUserContext();

    // 启动 SignalR 连接
    await EnsureSignalRStarted();

    // 订阅推送通知
    await pushNotificationService.Subscribe(CurrentCancellationToken);

    // 更新用户会话信息
    if (isAuthenticated)
        await UpdateUserSession();

    lastPropagatedUserId = userId;
}
```

这确保所有服务在身份验证状态变更时得到更新。

---

## 5. 推送通知

### 推送通知架构

推送通知使您能够**即使在应用未运行时**也向用户发送消息。这对于吸引用户和及时传递信息至关重要。

AI.Boilerplate 项目支持跨所有平台的**推送通知**：

- **Web (浏览器)**: 使用 VAPID 的 Web Push API
- **Android**: Firebase Cloud Messaging (FCM)
- **iOS**: Apple Push Notification Service (APNS)
- **macOS**: Apple Push Notification Service (APNS)

### 关键特性：深度链接

此项目中推送通知最强大的功能之一是**深度链接**。发送推送通知时，您可以指定用户点击通知时发生什么。

**使用 `pageUrl` 参数**，您可以：
- 当用户点击通知时在应用中打开特定页面
- 引导用户访问相关内容（例如新产品、促销活动、公告）
- 创建有针对性的营销活动
- 宣布新功能并直接引导用户前往

**示例**:

```csharp
await pushNotificationService.RequestPush(
    title: "新产品上架！",
    message: "查看我们最新的产品",
    pageUrl: "/products/123",  // 打开产品详情页面
    cancellationToken: cancellationToken
);
```

当用户点击此通知时：
- **Web**: 应用打开并导航到 `/products/123`
- **移动 (Android/iOS)**: 原生应用打开并导航到 `/products/123`
- **桌面 (Windows/macOS)**: 浏览器打开并导航到 `/products/123`

这对以下用途**极其有用**：
- **营销活动**: "电子产品限时特卖 - 五折优惠！" → 打开特卖页面
- **新功能**: "试用我们新的 AI 助手！" → 打开 AI 助手页面
- **用户参与**: "您有 3 条未读消息" → 打开消息页面
- **提醒**: "完善您的个人资料以解锁功能" → 打开个人资料页面

### 推送通知订阅

**客户端接口**: [`src/Client/AI.Boilerplate.Client.Core/Services/Contracts/IPushNotificationService.cs`](/src/Client/AI.Boilerplate.Client.Core/Services/Contracts/IPushNotificationService.cs)

**基础实现**: [`src/Client/AI.Boilerplate.Client.Core/Services/PushNotificationServiceBase.cs`](/src/Client/AI.Boilerplate.Client.Core/Services/PushNotificationServiceBase.cs)

```csharp
public async Task Subscribe(CancellationToken cancellationToken)
{
    if (await IsAvailable(cancellationToken) is false)
    {
        Logger.LogWarning("此平台/设备不支持/不允许通知。");
        return;
    }

    var subscription = await GetSubscription(cancellationToken);

    if (subscription is null)
        return;

    await pushNotificationController.Subscribe(subscription, cancellationToken);
}
```

每个平台都有其自己的实现：
- `WebPushNotificationService.cs`
- `AndroidPushNotificationService.cs`
- `iOSPushNotificationService.cs`
- `WindowsPushNotificationService.cs`
- `MacCatalystPushNotificationService.cs`

### 服务器端推送通知服务

**位置**: [`src/Server/AI.Boilerplate.Server.Api/Services/PushNotificationService.cs`](/src/Server/AI.Boilerplate.Server.Api/Services/PushNotificationService.cs)

---

## 6. Bit.Butil.Notification - 浏览器通知 API

该项目使用 **Bit.Butil.Notification** 访问浏览器的原生通知 API。

**扩展帮助器**: [`src/Client/AI.Boilerplate.Client.Core/Extensions/NotificationExtensions.cs`](/src/Client/AI.Boilerplate.Client.Core/Extensions/NotificationExtensions.cs)

```csharp
public static async Task<bool> IsNotificationAvailable(this Notification notification)
{
    var isPresent = await notification.IsSupported();
    if (isPresent)
    {
        if (await notification.GetPermission() is NotificationPermission.Granted)
            return true;
    }
    return false;
}
```

### 可用方法

- **`IsSupported()`**: 检查浏览器是否支持通知
- **`GetPermission()`**: 获取当前权限状态（`Granted`、`Denied`、`Default`）
- **`RequestPermission()`**: 向用户请求通知权限
- **`Show(string title, NotificationOptions options)`**: 显示原生通知

### 使用示例

```csharp
[AutoInject] private Notification notification = default!;

private async Task ShowNotification()
{
    // 首先请求权限
    var permission = await notification.RequestPermission();
    
    if (permission is NotificationPermission.Granted)
    {
        // 显示通知
        await notification.Show("My App", new NotificationOptions
        {
            Body = "这是一条通知消息",
            Icon = "/images/icon.png",
            Data = new Dictionary<string, string> { { "pageUrl", "/dashboard" } }
        });
    }
}
```

**重要提示**:
- 原生通知即使在标签页非活动状态下也能工作
- 它们显示为系统通知（不在浏览器窗口内）
- 用户必须授予权限才能显示通知
- `Data` 属性可以包含用于处理点击的自定义数据

---

## 7. 测试推送通知 - 理解四种场景

测试推送通知时，至关重要的是要理解基于**发送通知时的应用状态**和**用户点击通知时的应用状态**，存在**四种不同的场景**。AI.Boilerplate 项目在所有平台上处理所有四种场景。

✅ **场景 1**: 完全关闭应用 → 发送推送通知 → 点击通知 → 验证应用打开到正确的页面

✅ **场景 2**: 关闭应用 → 发送推送通知 → 手动打开应用（不点击通知）→ 现在点击通知 → 验证导航有效

✅ **场景 3**: 保持应用打开 → 发送推送通知 → 关闭应用 → 点击通知 → 验证应用打开到正确的页面

✅ **场景 4**: 保持应用打开 → 发送推送通知 → 立即点击通知 → 验证导航有效而无需重启应用

### 关键要点

- 代码库包含对所有四种推送通知场景的专门处理
- 根据应用状态使用不同的入口点（例如 Android 上的 `OnCreate` 与 `OnNewIntent`）
- Web 平台上的 Service workers 使用 `clients.matchAll()` 自动处理场景检测

---

### AI Wiki: 已回答的问题
* [描述 bit AI.Boilerplate 的 AI 聊天功能的工作流程并提供高级概述。](https://deepwiki.com/search/describe-the-workflow-of-bit-b_822b9510-8e1d-456f-99bf-fb1778374a9a)

在此处提出您自己的问题 [here](https://wiki.bitplatform.dev)
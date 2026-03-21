# 第六阶段：异常处理与错误管理

欢迎来到第六阶段！在本阶段，您将了解 AI.Boilerplate 项目中构建的全面异常处理和错误管理系统。

## 概述

本项目实现了一个强大的异常处理架构，该架构能够：

- 在发生异常时**不会**导致应用程序崩溃
- 自动处理 Blazor 组件和页面中的异常
- 向最终用户显示友好的错误消息
- 为开发人员记录详细的诊断信息
- 区分已知异常和未知异常
- 为 Server、Web、MAUI 和 Windows 提供多平台异常处理器

---

## 已知异常 vs 未知异常

本项目区分两种类型的异常：

### 已知异常 (Known Exceptions)

已知异常是继承自 `KnownException` 基类的异常，位于 `src/Shared/Infrastructure/Exceptions/KnownException.cs`。

这些是预期的、业务逻辑相关的异常，代表可预测的错误场景。

**特征：**
- 它们的消息会直接显示给用户（用户友好）
- 使用 `IStringLocalizer<AppStrings>` 进行本地化
- 被记录为非关键错误
- **不**表示代码中存在 Bug

**项目中已知异常的示例：**

1. **DomainLogicException** (`src/Shared/Infrastructure/Exceptions/DomainLogicException.cs`)
   - 用于违反业务规则的情况
   - 示例：“无法删除包含产品的类别”

2. **ResourceNotFoundException** (`src/Shared/Infrastructure/Exceptions/ResourceNotFoundException.cs`)
   - 当请求的资源不存在时抛出
   - HTTP 状态码：404 (Not Found)

3. **BadRequestException** (`src/Shared/Infrastructure/Exceptions/BadRequestException.cs`)
   - 用于无效的客户端请求
   - HTTP 状态码：400 (Bad Request)

4. **UnauthorizedException** (`src/Shared/Infrastructure/Exceptions/UnauthorizedException.cs`)
   - 当需要身份验证时抛出
   - HTTP 状态码：401 (Unauthorized)

5. **ServerConnectionException** (`src/Shared/Infrastructure/Exceptions/ServerConnectionException.cs`)
   - 指示客户端和服务器之间的连接问题

**异常继承关系：**
- 映射到 HTTP 状态码的异常（如 `BadRequestException`, `UnauthorizedException`, `ResourceNotFoundException`）继承自 `RestException` (`src/Shared/Infrastructure/Exceptions/RestException.cs`)，而 `RestException` 又继承自 `KnownException`，并为 REST API 提供 HTTP 状态码映射。
- 不需要 HTTP 状态码的异常（如 `DomainLogicException`, `ServerConnectionException`）直接继承自 `KnownException`。

### 未知异常 (Unknown Exceptions)

未知异常是所有其他异常（例如 `NullReferenceException`, `InvalidOperationException`, `ArgumentException` 等）。

**特征：**
- 它们代表代码中的 Bug 或意外错误
- 它们的详细消息**不会**显示给最终用户（为了安全）
- 在**生产环境**中，用户看到通用消息：“发生未知错误”
- 在**开发环境**中，用户看到完整的异常详细信息（堆栈跟踪等）
- 被记录为关键错误

**示例：**

```csharp
// 这会抛出 NullReferenceException (未知异常)
string? name = null;
int length = name.Length; // 代码中的 Bug！
```

**在生产环境中，用户会看到：**
> “发生未知错误”

**在开发环境中，用户会看到：**
> "System.NullReferenceException: Object reference not set to an instance of an object. at MyComponent.OnInitAsync()..."

这种行为由 `SharedExceptionHandler` 控制：

```csharp
// 来自 src/Shared/Infrastructure/Services/SharedExceptionHandler.cs
protected string GetExceptionMessageToShow(Exception exception)
{
    if (exception is KnownException)
        return exception.Message; // 对已知异常显示实际消息

    if (AppEnvironment.IsDevelopment())
        return exception.ToString(); // 在开发环境中显示完整详情

    return Localizer[nameof(AppStrings.UnknownException)]; // 在生产环境中显示通用消息
}
```

---

## 安全地抛出异常

**重要提示：抛出异常不会导致应用程序崩溃**

在本项目中，抛出异常是安全的，**不会**导致应用程序崩溃。异常处理系统会在多个层级自动捕获和处理异常。

**示例：**

```csharp
protected override async Task OnInitAsync()
{
    var user = await UserController.GetUserById(userId, CurrentCancellationToken);
    
    if (user == null)
    {
        // 这不会导致应用崩溃！
        throw new ResourceNotFoundException("未找到用户");
    }
    
    // 继续处理...
}
```

异常会被自动：
1. 由 `AppComponentBase` 的增强生命周期方法捕获
2. 记录完整的上下文和遥测数据
3. 通过消息框或 Snack Bar 显示给用户
4. 组件保持功能正常（无崩溃）

---

## 使用 WithData() 添加异常数据

`WithData()` 扩展方法允许开发人员将额外的上下文信息附加到异常上，以便更好地记录和调试。

**位置**: `src/Shared/Infrastructure/Extensions/ExceptionExtensions.cs`

### 语法

```csharp
// 单个键值对
exception.WithData("key", value)

// 多个键值对
exception.WithData(new()
{
    { "UserId", userId },
    { "ProductId", productId },
    { "Timestamp", DateTimeOffset.UtcNow }
})
```

### 项目中的真实示例

来自 `src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.EmailConfirmation.cs`:

```csharp
var user = await userManager.FindByEmailAsync(request.Email!)
    ?? throw new BadRequestException(Localizer[nameof(AppStrings.UserNotFound)]).WithData("Email", request.Email);

if (await userManager.IsEmailConfirmedAsync(user))
    throw new BadRequestException(Localizer[nameof(AppStrings.EmailAlreadyConfirmed)]).WithData("UserId", user.Id);
```

### 优势

- **增强日志记录**：数据会自动包含在日志条目中
- **遥测集成**：与 Application Insights、Sentry 等无缝协作
- **调试辅助**：帮助在不向用户暴露敏感数据的情况下诊断生产环境问题
- **可追溯性**：将异常连接到特定实体（用户、产品、订单等）

**注意**：在服务器上使用 `WithData()` 添加的数据**不会**发送给客户端——仅用于日志记录。若要向客户端发送数据，请使用 `WithExtensionData()`（见下文）。

---

## 使用 WithExtensionData() 向客户端发送额外数据

`WithExtensionData()` 扩展方法允许您随错误响应一起向客户端发送额外的上下文数据。当客户端需要特定信息来适当处理错误时，这非常有用。

**位置**: `src/Server/AI.Boilerplate.Server.Api/Infrastructure/Extensions/KnownExceptionExtensions.cs`

### 重要提示：仅适用于已知异常

**关键**：`WithExtensionData()` **仅**对继承自 `KnownException` 的异常有效。

### RFC 7807 合规性

本项目向客户端发送符合 **RFC 7807** 标准的错误响应。这是 HTTP API 错误响应的标准化格式：

```json
{
  "type": "AI.Boilerplate.Shared.Exceptions.TooManyRequestsException",
  "title": "请等待 1 分钟后再请求另一个重置密码令牌",
  "status": 429,
  "instance": "POST /api/identity/SendResetPasswordToken",
  "extensions": {
    "key": "WaitForResetPasswordTokenRequestResendDelay",
    "traceId": "00-abc123...",
    "TryAgainIn": "00:01:00"
  }
}
```

### 语法

```csharp
// 单个键值对
exception.WithExtensionData("key", value)

// 多个键值对
exception.WithExtensionData(new Dictionary<string, object?>
{
    { "TryAgainIn", TimeSpan.FromMinutes(1) },
    { "RetryAfter", DateTimeOffset.UtcNow.AddMinutes(1) }
})
```

### 项目中的真实示例

来自 `src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.cs`:

```csharp
if (signInResult.IsLockedOut)
{
    var tryAgainIn = (user.LockoutEnd! - DateTimeOffset.UtcNow).Value;
    
    throw new BadRequestException(
        Localizer[nameof(AppStrings.UserLockedOut), 
        tryAgainIn.Humanize(culture: CultureInfo.CurrentUICulture)])
        .WithData("UserId", user.Id)  // 仅用于服务器日志记录
        .WithExtensionData("TryAgainIn", tryAgainIn);  // 发送给客户端
}
```

在这些示例中：
- `.WithData("UserId", user.Id)`：在服务器上记录，**不**发送给客户端
- `.WithExtensionData("TryAgainIn", tryAgainIn)`：在错误响应中发送给客户端

### 在客户端读取扩展数据

客户端可以使用 `TryGetExtensionDataValue<T>()` 读取扩展数据：

来自 `src/Client/AI.Boilerplate.Client.Core/Components/Pages/Identity/SignIn/SignInPanel.razor.cs`:

```csharp
catch (KnownException e)
{
    // 读取从服务器发送的 TryAgainIn 值
    if (e.TryGetExtensionDataValue<TimeSpan>("TryAgainIn", out var tryAgainIn))
    {
        // 禁用登录按钮直到锁定期间结束
        // 显示倒计时器等
    }
}
```

### 使用场景

使用 `WithExtensionData()` 的常见场景：

1. **速率限制**：告诉客户端在重试前需要等待多久
   ```csharp
   throw new TooManyRequestsException("请求过多")
       .WithExtensionData("RetryAfter", TimeSpan.FromMinutes(5));
   ```

2. **账户锁定**：提供锁定持续时间
   ```csharp
   throw new BadRequestException("账户已锁定")
       .WithExtensionData("TryAgainIn", lockoutDuration);
   ```

3. **业务逻辑错误**：提供可操作的信息
   ```csharp
   throw new ConflictException("产品缺货")
       .WithExtensionData("AvailableQuantity", 0)
       .WithExtensionData("NextRestockDate", restockDate);
   ```

---

## Blazor 中的自动异常处理

本项目通过增强的生命周期方法在 Blazor 组件和页面中提供自动异常处理。

### 增强的生命周期方法

当您的组件继承自 `AppComponentBase` 或页面继承自 `AppPageBase` 时，您可以访问自动捕获异常的增强生命周期方法：

**位置**: `src/Client/AI.Boilerplate.Client.Core/Components/AppComponentBase.cs`

**可用的增强方法：**
- `OnInitAsync()` - 替代 `OnInitializedAsync()`
- `OnParamsSetAsync()` - 替代 `OnParametersSetAsync()`
- `OnAfterFirstRenderAsync()` - `OnAfterRenderAsync(firstRender: true)` 的增强版本

**示例：**

```csharp
public partial class MyComponent : AppComponentBase
{
    protected override async Task OnInitAsync()
    {
        // 此处抛出的任何异常都会被自动捕获和处理！
        var data = await ApiClient.GetData(CurrentCancellationToken);
        
        if (data == null)
        {
            throw new DomainLogicException("无可用数据");
            // 这不会导致应用崩溃 - 它会向用户显示错误消息
        }
    }
    
    protected override async Task OnParamsSetAsync()
    {
        // 自动捕获异常
        await LoadUserProfile();
    }
    
    protected override async Task OnAfterFirstRenderAsync()
    {
        // 自动捕获异常
        await JSRuntime.InvokeVoidAsync("initializeChart");
    }
}
```

**当异常发生时会发生什么？**
1. 异常被自动捕获
2. 异常被记录完整的上下文（组件名称、文件路径、行号等）
3. 异常由适当的异常处理器处理
4. 用户看到友好的错误消息（消息框或 Snack Bar）
5. 组件保持功能正常（无崩溃）

---

## Razor 中的事件处理器：WrapHandled()

对于 `.razor` 文件中的事件处理器（按钮点击、表单提交等），您必须使用 `WrapHandled()` 方法来启用自动异常处理。

### 语法

```xml
<!-- 简单的方法调用 -->
<BitButton OnClick="WrapHandled(MyMethod)">点击我</BitButton>

<!-- Lambda 表达式 -->
<BitButton OnClick="WrapHandled(async () => await SaveData())">保存</BitButton>

<!-- 带参数 -->
<BitButton OnClick="WrapHandled((MouseEventArgs e) => HandleClick(e))">点击</BitButton>
```

### 项目中的真实示例

来自 `src/Client/AI.Boilerplate.Client.Core/Components/Pages/Identity/SignIn/SignInPanel.razor`:

```xml
<EditForm Model="model" OnSubmit="WrapHandled(DoSignIn)" novalidate>
    <AppDataAnnotationsValidator @ref="validatorRef" />
    
    <!-- 表单字段 -->
    
    <BitButton ButtonType="BitButtonType.Submit" AutoLoading IsEnabled="isWaiting is false">
        @Localizer[nameof(AppStrings.SignIn)]
    </BitButton>
</EditForm>

<BitButton OnClick="WrapHandled(PasswordlessSignIn)" 
           IconName="@BitIconName.Fingerprint"
           IsEnabled="isWaiting is false">
    无密码登录
</BitButton>
```

### 为什么需要 WrapHandled()？

**如果不使用 `WrapHandled()`**，事件处理器中未处理的异常会：
- 触发错误边界（整个 UI 部分崩溃）
- 提供糟糕的用户体验

**使用 `WrapHandled()`**：
- 异常被捕获并记录完整的上下文（文件路径、行号、成员名称）
- 用户看到友好的错误消息
- 组件保持功能正常
- 不会触发错误边界

---

## 错误显示 UI

当异常发生时，本项目通过两种机制显示用户友好的错误消息：

### 1. 中断性错误 (消息框)

对于需要用户确认的关键错误，会显示消息框。

**使用时机：**
- 未知异常 (Bug)
- 关键业务逻辑错误

**示例：**
```
┌─────────────────────────────────────┐
│ 错误                                │
├─────────────────────────────────────┤
│ 保存产品失败。                      │
│ 请重试。                            │
│                                     │
│              [确定]                 │
└─────────────────────────────────────┘
```
**由**：`ClientExceptionHandlerBase` 中的 `BitMessageBoxService` 处理

### 2. 非中断性错误 (Snack Bar)

对于不需要立即操作的次要错误，会显示 Snack Bar（Toast 通知）。

**示例：**
```
┌─────────────────────────────────────────┐
│ ⚠️ 您当前处于离线状态                   │
└─────────────────────────────────────────┘
```
**由**：`ClientExceptionHandlerBase` 中的 `SnackBarService` 处理

### 3. 错误边界 (最后手段)

如果异常**未被**增强的生命周期方法或 `WrapHandled()` 捕获，则会触发错误边界。

**位置**: `src/Client/AI.Boilerplate.Client.Core/Components/AppErrorBoundary.razor`

**错误边界显示：**
- 标题：“出错了”
- 异常详细信息（如果在开发模式下）
- 操作：首页、刷新、恢复、诊断

**示例：**
```
┌─────────────────────────────────────────┐
│ 出错了                                  │
├─────────────────────────────────────────┤
│ 发生了意外错误。                        │
│                                         │
│ [首页] [刷新] [恢复] [🔧]              │
└─────────────────────────────────────────┘
```

**使用时机：**
- 您应该**避免**触发错误边界
- 对事件处理器使用 `WrapHandled()`
- 使用增强的生命周期方法 (`OnInitAsync()` 等)

---

## 项目中的异常处理器

本项目包含多个针对不同平台的异常处理器，它们都继承自 `SharedExceptionHandler`。

### 1. ServerExceptionHandler

**位置**: `src/Server/AI.Boilerplate.Server.Api/Infrastructure/Services/ServerExceptionHandler.cs`

**目的**: 处理服务器端（API 控制器）的异常。

**主要职责：**
- 将异常转换为符合 RFC 7807 标准的 `ProblemDetails` 响应
- 附加 `Request-Id` 头以便日志关联
- 将异常映射到 HTTP 状态码 (404, 400, 500 等)
- 记录带有完整遥测数据的异常（活动 ID、用户 ID、客户端 IP 等）
- 区分开发和生产环境

**生成的错误响应示例：**
```json
{
  "type": "AI.Boilerplate.Shared.Exceptions.ResourceNotFoundException",
  "title": "未找到用户",
  "status": 404,
  "instance": "GET /api/user/123",
  "extensions": {
    "key": "UserNotFound",
    "traceId": "00-abc123...",
    "Email": "user@example.com"
  }
}
```

### 2. SharedExceptionHandler

**位置**: `src/Shared/Infrastructure/Services/SharedExceptionHandler.cs`

**目的**: 提供服务器和客户端共享的异常处理逻辑。

**主要职责：**
- `GetExceptionMessageToShow()`: 确定向用户显示什么消息
  - 已知异常：显示实际消息
  - 未知异常（生产环境）：显示通用的“未知错误”
  - 未知异常（开发环境）：显示完整堆栈跟踪
- `GetExceptionMessageToLog()`: 格式化异常以进行日志记录（包括内部异常）
- `UnWrapException()`: 解包 `AggregateException` 和 `TargetInvocationException`
- `IgnoreException()`: 确定是否应记录异常
- `GetExceptionData()`: 提取附加到异常的所有数据

### 3. ClientExceptionHandlerBase

**位置**: `src/Client/AI.Boilerplate.Client.Core/Services/ClientExceptionHandlerBase.cs`

**目的**: 客户端异常处理器的基类。

**主要职责：**
- 记录带有遥测上下文的异常（文件路径、行号、成员名称等）
- 确定如何显示错误：
  - `ExceptionDisplayKind.Interrupting`: 消息框
  - `ExceptionDisplayKind.NonInterrupting`: Snack Bar
  - `ExceptionDisplayKind.None`: 无 UI（仅记录，在开发环境中断点调试）
- 自动忽略 `TaskCanceledException`, `OperationCanceledException`, `TimeoutException`

### 4. WebClientExceptionHandler

**位置**: `src/Client/AI.Boilerplate.Client.Web/Services/WebClientExceptionHandler.cs`

**目的**: Blazor WebAssembly (浏览器) 的异常处理器。

**平台特定行为：**
- 继承 `ClientExceptionHandlerBase` 的所有行为
- 可扩展以添加浏览器特定的错误跟踪（例如 Google Analytics）

### 5. MauiExceptionHandler

**位置**: `src/Client/AI.Boilerplate.Client.Maui/Services/MauiExceptionHandler.cs`

**目的**: .NET MAUI (Android, iOS, macOS) 的异常处理器。

**平台特定行为：**
- 可与 Firebase Crashlytics 集成以进行 Android/iOS 崩溃报告
- 可与平台特定的错误跟踪服务集成

**示例：**
```csharp
protected override void Handle(Exception exception, ExceptionDisplayKind displayKind, Dictionary<string, object> parameters)
{
    // 记录到 Firebase Crashlytics
    FirebaseCrashlytics.Instance.RecordException(exception);
    
    base.Handle(exception, displayKind, parameters);
}
```

### 6. WindowsExceptionHandler

**位置**: `src/Client/AI.Boilerplate.Client.Windows/Services/WindowsExceptionHandler.cs`

**目的**: Windows Forms Blazor Hybrid 应用的异常处理器。

**平台特定行为：**
- 可与 Windows 特定的错误报告集成（例如 Windows Error Reporting API）

---

### AI Wiki: 已回答的问题
* [告诉我关于 ServerConnectionException 的一切](https://wiki.bitplatform.dev)

在此处提出您自己的问题。
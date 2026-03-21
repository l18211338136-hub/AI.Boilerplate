# 第四阶段：后台作业与 CancellationToken 管理

欢迎参加 AI.Boilerplate 项目教程的**第四阶段**！在本阶段，我们将探讨本项目如何处理用于请求取消的 **CancellationToken（取消令牌）**，以及如何使用 Hangfire 进行**后台作业处理**。

---

## 目录

1. [API 请求中的 CancellationToken](#cancellationtoken-in-api-requests)
   - [自动请求取消](#automatic-request-cancellation)
   - [客户端集成](#client-side-integration)
   - [用户放弃场景](#user-abandonment-scenarios)
2. [关键操作的导航锁 (Navigation Lock)](#navigation-lock-for-critical-operations)
3. [何时使用后台作业](#when-to-use-background-jobs)
4. [使用 Hangfire 实现后台作业](#background-job-implementation-with-hangfire)
   - [PhoneServiceJobsRunner 示例](#phoneservicejobsrunner-example)
   - [Hangfire 集成的主要优势](#key-benefits-of-hangfire-integration)

---

## API 请求中的 CancellationToken

### 自动请求取消

本项目中的所有 API 方法都接收一个 `CancellationToken` 参数，当发生以下情况时，该参数会**自动取消操作**：

- **用户离开**当前页面
- **浏览器/应用被关闭**
- **新请求取代了旧请求**（例如：在第 1 页的数据网格仍在加载时，用户导航到了第 2 页）
- Blazor 中的**组件被释放 (Disposed)**

**重要提示**：对于返回 `IQueryable` 的 API 方法，取消是**隐式**发生的——您无需手动将令牌传递给 EF Core 查询，因为 OData 和 Entity Framework Core 会自动处理它。

这确保了服务器资源不会被浪费在处理用户不再需要的请求上。

#### 服务器端示例

让我们看看项目中的一个真实控制器——[`TodoItemController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Todo/TodoItemController.cs)：

```csharp
[HttpPost]
public async Task<TodoItemDto> Create(TodoItemDto dto, CancellationToken cancellationToken)
{
    var entityToAdd = dto.Map();

    entityToAdd.UserId = User.GetUserId();

    entityToAdd.Date = DateTimeOffset.UtcNow;

    await DbContext.TodoItems.AddAsync(entityToAdd, cancellationToken);

    await DbContext.SaveChangesAsync(cancellationToken);

    return entityToAdd.Map();
}

[HttpPut]
public async Task<TodoItemDto> Update(TodoItemDto dto, CancellationToken cancellationToken)
{
    var entityToUpdate = await DbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == dto.Id, cancellationToken)
        ?? throw new ResourceNotFoundException(Localizer[nameof(AppStrings.ToDoItemCouldNotBeFound)]);

    dto.Patch(entityToUpdate);

    await DbContext.SaveChangesAsync(cancellationToken);

    return entityToUpdate.Map();
}

[HttpDelete("{id}")]
public async Task Delete(Guid id, CancellationToken cancellationToken)
{
    DbContext.TodoItems.Remove(new() { Id = id });

    var affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

    if (affectedRows < 1)
        throw new ResourceNotFoundException(Localizer[nameof(AppStrings.ToDoItemCouldNotBeFound)]);
}
```

注意每个异步操作（`AddAsync`, `SaveChangesAsync`, `FirstOrDefaultAsync`）都接收了 `cancellationToken` 参数。这使得如果用户放弃请求，Entity Framework Core 能够取消数据库操作。

---

### 客户端集成

#### 实现方式

取消令牌系统通过服务器端和客户端组件的结合来工作：

**服务器端**：API 方法接受 `CancellationToken cancellationToken` 参数（如上所示）。

**客户端**：组件继承自 `AppComponentBase`，该类提供了一个 `CurrentCancellationToken` 属性。在进行 API 调用时，此令牌会自动传递。

#### CurrentCancellationToken 属性

让我们看看 `CurrentCancellationToken` 在 [`AppComponentBase.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/AppComponentBase.cs) 中是如何实现的：

```csharp
private CancellationTokenSource? cts = new();

protected CancellationToken CurrentCancellationToken
{
    get
    {
        // ...
    }
}
```

当组件被释放（用户导航离开、关闭标签页等）时，`CancellationTokenSource` 会被释放，从而自动取消所有正在进行的操作。

#### 客户端使用示例

以下是来自 [`TodoPage.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/TodoPage.razor.cs) 的真实示例：

```csharp
private async Task LoadTodoItems()
{
    allTodoItems = await todoItemController.Get(CurrentCancellationToken);
}

private async Task AddTodoItem()
{
    var addedTodoItem = await todoItemController.Create(new() { Title = newTodoTitle }, CurrentCancellationToken);
    // ... 其余代码
}

private async Task DeleteTodoItem()
{
    await todoItemController.Delete(deletingTodoItem.Id, CurrentCancellationToken);
    // ... 其余代码
}

private async Task SaveTodoItem(TodoItemDto todoItem)
{
    (await todoItemController.Update(todoItem, CurrentCancellationToken)).Patch(todoItem);
    // ... 其余代码
}
```

---

### 用户放弃场景

#### 逻辑取消

如果用户点击“保存”以更新待办事项，然后**立即**：

- 导航到另一个页面
- 关闭浏览器/应用
- 执行任何触发导航的操作

保存操作将被**自动取消**。

#### 为什么这是可以的

用户没有等待结果，而结果可能是：
- 错误（例如：验证失败、网络错误、产品名称重复）
- 成功确认

既然他们没有等待，取消操作就是**符合逻辑的行为**。用户已经通过导航离开表明他们不再关心结果。

---

## 关键操作的导航锁 (Navigation Lock)

### 目的

对于您希望**防止**自动取消的操作，请使用 `NavigationLock`。

这在以下情况非常有用：
- 您正在执行**必须完成**的关键操作
- 您想在用户离开前**警告**他们
- 您需要**确认**用户是否真的想离开
- 您有未保存的更改，如果离开将会丢失

**重要**：仅对**短暂的关键操作**使用此功能。对于长时间运行的任务，请改用后台作业。

### 在保存期间防止导航

让我们看一个如何使用 `NavigationLock` 防止在保存操作进行时导航的示例。

#### Razor 组件 (`.razor` 文件)

```xml
@inherits AppPageBase

<NavigationLock OnBeforeInternalNavigation="HandleNavigation" />

<EditForm Model="product" OnValidSubmit="WrapHandled(SaveProduct)">
    <BitTextField @bind-Value="product.Name" Label="产品名称" />
    <BitTextField @bind-Value="product.Price" Label="价格" />
    
    <BitButton IsLoading="isSaving" ButtonType="BitButtonType.Submit">
        @Localizer[nameof(AppStrings.Save)]
    </BitButton>
</EditForm>
```

#### 代码隐藏 (`.razor.cs` 文件)

```csharp
public partial class EditProductPage
{
    [AutoInject] IProductController productController = default!;
    
    private bool isSaving = false;
    private ProductDto product = new();

    private async Task SaveProduct()
    {
        if (isSaving) return; // 防止多次提交

        isSaving = true; // 在开始操作之前设置标志

        try
        {
            // 此操作必须完成 - 不允许导航
            await productController.Update(product, CurrentCancellationToken);
            
            SnackBarService.Success("产品保存成功！");
        }
        finally
        {
            isSaving = false; // 始终重置标志
        }
    }

    private void HandleNavigation(LocationChangingContext context)
    {
        // 如果保存操作正在进行，防止导航
        if (isSaving)
        {
            context.PreventNavigation();
        }
    }
}
```

**工作原理：**

1. **NavigationLock 组件**：当模态框/页面打开时，渲染 `<NavigationLock OnBeforeInternalNavigation="HandleNavigation" />`
2. **isSaving 标志**：当保存操作开始时设置为 `true`
3. **HandleNavigation 方法**：在任何导航发生之前由 `NavigationLock` 调用
   - 如果 `isSaving` 为 `true`，则调用 `context.PreventNavigation()` 阻止导航
   - 这确保在用户离开之前保存操作已完成
4. **用户体验**：用户在按钮上看到加载指示器 (`IsLoading="isSaving"`)，并且在操作完成前无法导航离开

**导航被阻止时：**
- 用户点击浏览器后退按钮 → **被阻止**
- 用户点击菜单项 → **被阻止**
- 用户刷新页面 → **出现浏览器的原生“离开站点？”对话框**

**允许导航时：**
- 在 `finally` 块中将 `isSaving` 重置为 `false` 后
- 操作完成（成功或失败）且标志被重置后

### 使用模式

此模式可用于：
- 带有未保存更改的编辑表单
- 多步骤向导
- 关键数据录入屏幕
- 支付处理表单
- 任何不应被中断的操作

---

## 何时使用后台作业

### 问题所在

如果操作**耗时较长**怎么办？例如：

- 发送带有验证码的短信
- 生成大型 PDF 报告
- 处理上传的文件
- 发送批量电子邮件

**用户不应该：**
- 等待操作完成
- 保持页面打开
- 一直在线直到任务结束

对于长时间运行的任务，`NavigationLock` **并不合适**，因为它强制用户等待。

### 解决方案：使用 Hangfire 进行后台作业

与其让用户等待，不如将任务**加入队列**并让其在后台运行。用户可以：
- 立即导航离开
- 关闭浏览器
- 稍后回来检查结果

**主要优势：**
- 操作被排队并异步处理
- 服务器重启或崩溃不会丢失作业
- 作业持久化在数据库中并自动恢复

---

## 使用 Hangfire 实现后台作业

### PhoneServiceJobsRunner 示例

让我们看看发送短信是如何作为后台作业实现的。

#### 步骤 1：服务将作业加入队列

在 [`PhoneService.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Services/PhoneService.cs) 中：

```csharp
[AutoInject] private readonly IBackgroundJobClient backgroundJobClient = default!;

public virtual async Task SendSms(string messageText, string phoneNumber)
{
    if (hostEnvironment.IsDevelopment())
    {
        LogSendSms(phoneLogger, messageText, phoneNumber);
    }

    if (appSettings.Sms?.Configured is false) return;

    var from = appSettings.Sms!.FromPhoneNumber!;

    // 将作业加入队列 - 这会立即返回
    backgroundJobClient.Enqueue<PhoneServiceJobsRunner>(x => x.SendSms(phoneNumber, from, messageText));
}
```

**关键点：**
- `backgroundJobClient.Enqueue<T>()` 调度作业在后台运行
- 方法**立即返回**——用户无需等待
- Hangfire 会自动向作业运行器方法提供 `PerformContext` 和 `CancellationToken` 参数

#### 步骤 2：作业运行器执行任务

在 [`PhoneServiceJobsRunner.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Services/PhoneServiceJobsRunner.cs) 中：

```csharp
public partial class PhoneServiceJobsRunner
{
    [AutoInject] private ServerExceptionHandler serverExceptionHandler = default!;

    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [30])]
    public async Task SendSms(string phoneNumber, string from, string messageText,
        PerformContext context = null!,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var messageOptions = new CreateMessageOptions(new(phoneNumber))
            {
                From = new(from),
                Body = messageText
            };

            var smsMessage = MessageResource.Create(messageOptions);

            if (smsMessage.ErrorCode is not null)
                throw new InvalidOperationException(smsMessage.ErrorMessage).WithData(new() { { "Code", smsMessage.ErrorCode } });
        }
        catch (Exception exp)
        {
            serverExceptionHandler.Handle(exp, new()
            {
                { "PhoneNumber", phoneNumber },
                { "JobId", context.BackgroundJob.Id }
            });
            if (exp is not KnownException && cancellationToken.IsCancellationRequested is false)
                throw; // 重试作业
        }
    }
}
```

**主要特性：**

1. **AutomaticRetry (自动重试)**：如果作业失败，Hangfire 会自动重试
   - `Attempts = 3`：最多尝试 3 次
   - `DelaysInSeconds = [30]`：每次重试之间等待 30 秒
   - 这对于 2 分钟后过期的短信验证码非常完美

2. **Hangfire 提供的参数**：该方法接受两个由 Hangfire 自动提供的特殊参数：
   - `PerformContext context`：提供对作业元数据（如 `JobId`, `BackgroundJob` 等）的访问
   - `CancellationToken cancellationToken`：指示何时应取消作业（例如服务器关闭）
   - 这些参数具有默认值（`null!` 和 `default`），以便 Hangfire 可以调用该方法

3. **异常处理**：
   - 记录带有上下文（`PhoneNumber`, `JobId`）的错误
   - 对于未知异常，重新抛出以触发重试
   - 对于已知异常（业务逻辑错误），不重试

4. **CancellationToken**：即使是后台作业也支持取消（例如，如果服务器正在关闭）

**重要**：在后台作业内部，**没有** `IHttpContextAccessor` 或 `User` 对象可用。因此，如果需要用户上下文，必须将其作为参数传递给作业方法。

#### 步骤 3：服务注册

在 [`Program.Services.cs`](/src/Server/AI.Boilerplate.Server.Api/Program.Services.cs) 中：

```csharp
services.AddScoped<PhoneServiceJobsRunner>();
```

作业运行器被注册为作用域 (Scoped) 服务，因此它可以访问与普通控制器相同的所有依赖项（DbContext, IStringLocalizer 等）。

---

### Hangfire 集成的主要优势

1. **持久性**：作业存储在数据库中
   - 如果服务器崩溃，作业不会丢失
   - 当服务器重启时，挂起的作业会恢复

2. **可靠性**：内置重试机制
   - 瞬态故障（网络问题、临时服务中断）会自动处理
   - 您可以为每个作业配置重试策略

3. **可扩展性**：作业可以在不同的服务器上处理
   - 添加更多服务器以更快地处理作业
   - 后台处理不会阻塞 Web 请求

4. **监控**：Hangfire 仪表板
   - 查看所有作业（挂起、处理中、成功、失败）
   - 手动重试失败的作业
   - 查看作业执行历史和统计信息

5. **自动清理**：旧的作业记录会自动删除
   - 保持数据库大小可控
   - 可配置的保留策略

---

### AI Wiki: 已回答的问题
* [AppComponentBase 中的 Abort() 方法与组件释放有何不同，开发人员何时应该显式调用它，而不是依赖 DisposeAsync 生命周期？](https://deepwiki.com/search/how-does-the-abort-method-in-a_187b27a3-091b-4e36-9905-0bf5b128b4aa)

在此处提出您自己的问题：[https://wiki.bitplatform.dev](https://wiki.bitplatform.dev)

---
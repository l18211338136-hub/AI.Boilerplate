# 第十七阶段：自动化测试 (Unitigration 测试)

欢迎回到**第十七阶段**！在本阶段，您将了解本项目内置的综合测试基础设施。本项目采用了一种名为 "**Unitigration 测试**" 的混合方法——这种测试结合了**编写单元测试的便捷性**与**集成测试的真实服务器行为**。

## 什么是 Unitigration 测试？

**Unitigration 测试** = 编写单元测试的**便捷性** + 集成测试的**真实**服务器行为

这是一种务实的测试方法：测试被编写为具有完整真实服务器行为的集成测试（包括 UI 测试和基于 HTTP 客户端的测试），但在**需要时**可以像单元测试一样灵活地伪造（Fake/Mock）服务器的特定部分，从而使测试编写变得简单得多。

**关键特征**：
- 针对**真实应用程序**运行，包含实际依赖项（数据库、服务、中间件）
- 允许在需要时**选择性模拟**特定服务
- 提供像集成测试一样的**高置信度**（真实的 HTTP 调用、真实的数据库）
- 同时支持 **API 测试**（后端逻辑）和 **UI 测试**（使用 Playwright 进行端到端测试）

**重要提示**：如果您愿意，开发人员完全可以编写纯单元测试或纯集成测试，但建议在大多数场景下使用 Unitigration 测试。因为这种方法优于传统的一切皆模拟的单元测试——它能捕捉到真实的集成问题，同时保持可维护性和易写性。

---

## 测试项目结构

测试项目位于 [`src/Tests/AI.Boilerplate.Tests.csproj`](/src/Tests/AI.Boilerplate.Tests.csproj)，包含以下内容：

### 关键文件：
- **[`TestsAssemblyInitializer.cs`](/src/Tests/Infrastructure/TestsAssemblyInitializer.cs)**: 程序集级别的设置，在所有测试运行前执行一次。
- **[`AppTestServer.cs`](/src/Tests/Infrastructure/AppTestServer.cs)**: 核心测试基础设施，用于启动 Web 应用程序。
- **[`IntegrationTests.cs`](/src/Tests/Features/Identity/IntegrationTests.cs)**: API/后端测试示例。
- **[`UITests.cs`](/src/Tests/Features/Identity/UITests.cs)**: 使用 Playwright 进行 UI 测试的示例。
- **[`TestData.cs`](/src/Tests/Features/Identity/TestData.cs)**: 共享测试数据常量。
- **[`.runsettings`](/src/Tests/.runsettings)**: 测试配置（Playwright 设置、并行执行、环境变量）。

### 支持文件夹：
- **`Infrastructure/`**: 核心测试基础设施
  - **`Services/`**: 特定于测试的服务实现
    - [`TestAuthTokenProvider.cs`](/src/Tests/Infrastructure/Services/TestAuthTokenProvider.cs): 用于 API 测试的内存令牌提供者。
    - [`TestStorageService.cs`](/src/Tests/Infrastructure/Services/TestStorageService.cs): 用于 API 测试的内存存储。
  - **`Extensions/`**: 辅助扩展
    - [`WebApplicationBuilderExtensions.cs`](/src/Tests/Infrastructure/Extensions/WebApplicationBuilderExtensions.cs): 测试服务注册。
    - [`PlaywrightVideoRecordingExtensions.cs`](/src/Tests/Infrastructure/Extensions/PlaywrightVideoRecordingExtensions.cs): 失败测试的视频录制。
- **`Features/`**: 按领域组织的特定功能测试
  - **`Identity/`**: 身份认证/授权测试。

---

## 使用的技术

### 测试框架：
- **MSTest v4**: 现代 MSTest 框架，原生支持异步，性能更佳，诊断功能更强。
- **Microsoft.Playwright.MSTest.v4**: 用于 UI 测试的端到端浏览器自动化。
- **Aspire.Hosting.Testing**: 与 .NET Aspire 集成，用于运行依赖项（数据库、邮件、S3）。
- **FakeItEasy**: 模拟库（当需要选择性模拟时使用）。

### 关键特性：
- **并行测试执行**: 测试并行运行以加快反馈速度（在 `.runsettings` 中配置）。
- **视频录制**: 失败的 UI 测试自动录制视频以便调试。
- **真实依赖项**: 通过 Aspire，测试可以针对真实的 SQL Server、邮件服务器等运行。

---

## 核心测试基础设施

### 1. AppTestServer - 测试的核心

[`AppTestServer`](/src/Tests/AppTestServer.cs) 类负责在进程中启动 Web 应用程序以供测试：

```csharp
public partial class AppTestServer : IAsyncDisposable
{
    public WebApplication WebApp { get; }
    public readonly Uri WebAppServerAddress = new(GenerateServerUrl());

    public AppTestServer Build(
        Action<IServiceCollection>? configureTestServices = null,
        Action<ConfigurationManager>? configureTestConfigurations = null)
    {
        // 创建具有特定于测试配置的 WebApplication
        // 允许覆盖服务和配置
        // 返回配置好的测试服务器
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        await WebApp.StartAsync(cancellationToken);
    }
}
```

**关键特性**：
- **动态端口分配**: 每个测试获得一个唯一的端口以避免冲突。
- **服务覆盖**: 用测试替身替换生产服务。
- **配置覆盖**: 为测试场景修改 appsettings。
- **完整的应用栈**: 所有中间件、认证、授权等工作方式与生产环境完全一致。

### 2. TestsAssemblyInitializer - 程序集设置

[`TestsAssemblyInitializer`](/src/Tests/Infrastructure/TestsAssemblyInitializer.cs) 使用 `[AssemblyInitialize]` 在所有测试之前运行**一次**：

```csharp
[TestClass]
public partial class TestsAssemblyInitializer
{
    [AssemblyInitialize]
    public static async Task Initialize(TestContext testContext)
    {
        await RunAspireHost(testContext);
        await using var testServer = new AppTestServer();
        await testServer.Build().Start(testContext.CancellationToken);
        await InitializeDatabase(testServer);
    }
}
```

**它做什么**：
1. **启动 .NET Aspire Host**: 启动 SQL Server、邮件服务器、S3 等（通过 Docker 容器）。
2. **获取连接字符串**: 从 Aspire 获取数据库和服务连接字符串。
3. **初始化数据库**: 使用 EF Core 创建数据库架构。
4. **设置环境变量**: 使连接字符串对所有测试可用。

**重要提示**: Aspire 应用运行时**不包含 Web 项目**——仅包含基础设施依赖项。实际的 Web 应用程序是使用 `AppTestServer` 按测试启动的。

---

## API 测试示例

让我们检查 [`IntegrationTests.cs`](/src/Tests/Features/Identity/IntegrationTests.cs) 以了解 API 测试：

```csharp
[TestClass]
public partial class IntegrationTests
{
    [TestMethod]
    public async Task SignInTest()
    {
        await using var server = new AppTestServer();

        await server.Build(services =>
        {
            // 用测试替身替换生产服务
            services.Replace(ServiceDescriptor.Scoped<IExampleService, TestExampleService>());
        }).Start(TestContext.CancellationToken);

        await using var scope = server.WebApp.Services.CreateAsyncScope();

        var authenticationManager = scope.ServiceProvider.GetRequiredService<AuthManager>();

        // 执行登录
        await authenticationManager.SignIn(new()
        {
            Email = TestData.DefaultTestEmail,
            Password = TestData.DefaultTestPassword
        }, TestContext.CancellationToken);

        var userController = scope.ServiceProvider.GetRequiredService<IUserController>();

        // 验证已登录用户
        var user = await userController.GetCurrentUser(TestContext.CancellationToken);

        Assert.AreEqual(Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"), user.Id);
    }
}
```

**关键概念**：

### 服务替换
```csharp
services.Replace(ServiceDescriptor.Scoped<IStorageService, TestStorageService>());
```
- **为什么？** API 测试中不存在浏览器存储，所以我们使用内存实现。
- **TestStorageService**: 简单的 `Dictionary<string, string?>`，模仿浏览器存储。
- **选择性模拟**: 仅模拟必要的部分；其他一切都是真实的。

### 服务解析
```csharp
await using var scope = server.WebApp.Services.CreateAsyncScope();
var authManager = scope.ServiceProvider.GetRequiredService<AuthManager>();
```
- 像在生产环境中一样创建 DI 作用域。
- 从实际的应用程序容器中解析服务。
- 服务可以访问真实的 DbContext、配置等。

### TestData 常量
```csharp
public const string DefaultTestEmail = "761516331@qq.com";
public const string DefaultTestPassword = "123456";
```
- 在 [`TestData.cs`](/src/Tests/TestData.cs) 中集中管理测试数据。
- 种子数据在 `AppDbContext.OnModelCreating()` 中为开发/测试创建。
- 测试使用已知数据进行可预测的断言。

---

## UI 测试示例 (Playwright)

让我们检查 [`UITests.cs`](/src/Tests/Features/Identity/UITests.cs) 以了解端到端 UI 测试：

```csharp
[TestClass]
public partial class UITests : PageTest
{
    [TestMethod]
    public async Task SignIn_Should_WorkAsExpected()
    {
        await using var server = new AppTestServer();
        await server.Build().Start(TestContext.CancellationToken);

        // 导航到登录页面
        await Page.GotoAsync(new Uri(server.WebAppServerAddress, PageUrls.SignIn).ToString());

        // 验证页面标题
        await Expect(Page).ToHaveTitleAsync(AppStrings.SignInPageTitle);

        // 填写凭据
        await Page.GetByPlaceholder(AppStrings.EmailPlaceholder).FillAsync(TestData.DefaultTestEmail);
        await Page.GetByPlaceholder(AppStrings.PasswordPlaceholder).FillAsync(TestData.DefaultTestPassword);
        
        // 点击登录按钮
        await Page.GetByRole(AriaRole.Button, new() { Name = AppStrings.Continue, Exact = true }).ClickAsync();

        // 验证成功登录
        await Expect(Page).ToHaveURLAsync(server.WebAppServerAddress.ToString());
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = TestData.DefaultTestFullName })).ToBeVisibleAsync();
    }

    // 为失败的测试启用视频录制
    public override BrowserNewContextOptions ContextOptions() => 
        base.ContextOptions().EnableVideoRecording(TestContext);

    [TestCleanup]
    public async ValueTask Cleanup() => 
        await Context.FinalizeVideoRecording(TestContext);
}
```

**关键概念**：

### PageTest 基类
- 继承自 `PageTest` (Microsoft.Playwright.MSTest.v4)。
- 自动提供 `Page` 和 `Context` 属性。
- 处理浏览器生命周期（启动/停止）。

### Locator 最佳实践
```csharp
await Page.GetByPlaceholder(AppStrings.EmailPlaceholder).FillAsync(email);
await Page.GetByRole(AriaRole.Button, new() { Name = AppStrings.Continue }).ClickAsync();
```
- **使用资源字符串**: 与应用中使用的字符串相同，确保如果 UI 文本更改，测试会失败。
- **语义定位器**: `GetByRole`, `GetByPlaceholder` 比 CSS 选择器更具弹性。
- **无障碍友好**: 测试强制执行正确的 ARIA 角色和标签。

### 视频录制
```csharp
public override BrowserNewContextOptions ContextOptions() => 
    base.ContextOptions().EnableVideoRecording(TestContext);
```
- 视频录制在 [`TestResults/Videos/`](/src/Tests/TestResults/Videos/) 中。
- **仅失败的测试**保留其视频（成功的测试会删除它们以节省空间）。
- 视频对于调试间歇性故障非常有价值。

### 断言
```csharp
await Expect(Page).ToHaveTitleAsync(AppStrings.SignInPageTitle);
await Expect(Page.GetByRole(AriaRole.Button, new() { Name = userFullName })).ToBeVisibleAsync();
```
- Playwright 的 `Expect` API，内置重试逻辑。
- 自动等待条件满足（无需显式的 `Task.Delay`）。
- 清晰、可读的断言。

---

## 测试配置 (.runsettings)

[`.runsettings`](/src/Tests/.runsettings) 文件配置测试执行：

```xml
<RunSettings>
    <RunConfiguration>
        <EnvironmentVariables>            
            <!-- 为测试覆盖 appsettings -->
            <ConnectionStrings__sqlite>Data Source=AI.BoilerplateDb.db;Mode=Memory;Cache=Shared;</ConnectionStrings__sqlite>
        </EnvironmentVariables>
    </RunConfiguration>
</RunSettings>
```

**关键设置**：

### 环境覆盖
- **SQLite 内存数据库**: 默认用于快速、隔离的测试。
- **可覆盖**: 如果需要，可以指向真实的 SQL Server。
- **层级**: `.runsettings` → `appsettings.json` → 环境变量。

---

## 运行测试

### 从 Visual Studio:
1. 打开 **测试资源管理器** (测试 → 测试资源管理器)。
2. 点击 **全部运行** 以执行所有测试。
3. 右键单击单个测试以运行/调试特定测试。

### 从命令行:
```powershell
# 导航到 Tests 项目目录
cd src/Tests

# 运行所有测试
dotnet test

# 运行 UI 测试
dotnet test --filter "TestCategory=UITest"

# 运行并输出详细信息
dotnet test --logger "console;verbosity=detailed"
```

### 从 VS Code:
1. 安装 **C# Dev Kit** 扩展。
2. 测试出现在 **测试** 侧边栏中。
3. 点击播放按钮运行测试。

### Playwright 特定命令:
```powershell
# 安装 Playwright 浏览器 (仅首次)
pwsh src/Tests/bin/Debug/net10.0/playwright.ps1 install

# 更新 Playwright 浏览器
pwsh src/Tests/bin/Debug/net10.0/playwright.ps1 install --force
```

---

## 持续集成 (CI)

该项目包含 GitHub Actions 工作流，可自动运行测试：

### 每次推送/PR 时:
- 测试在 CI 服务器上并行运行。
- Playwright 浏览器被缓存以提高速度。
- 测试结果作为制品上传。
- 失败测试的视频被保存以供审查。

### `.github/workflows/` 中的配置:
```yaml
- name: Run Tests
  run: dotnet test src/Tests/AI.Boilerplate.Tests.csproj --configuration Release

- name: Upload Test Videos (on failure)
  if: failure()
  uses: actions/upload-artifact@v4
  with:
    name: test-videos
    path: src/Tests/TestResults/Videos/
```

---
# 第十六阶段：CI/CD 流水线与环境配置

欢迎回到第十六阶段！在本阶段，您将了解驱动本项目的综合 CI/CD 流水线设置和环境配置系统。该系统为所有平台提供感知环境的构建和部署。

---

## 📋 环境配置系统

### 理解 AppEnvironment

本项目使用统一的环境配置系统，在所有平台上（从 ASP.NET Core 后端到原生移动应用）都能一致地工作。

**位置**: [`/src/Shared/Infrastructure/Services/AppEnvironment.cs`](/src/Shared/Infrastructure/Services/AppEnvironment.cs)

**为什么这很重要**：
与使用环境变量（可在运行时设置）的 ASP.NET Core 不同，Android、iOS、Windows 和 macOS 并不完全支持相同的概念。`AppEnvironment` 提供了一种在任何地方都有效的统一抽象。

**关键代码**：
```csharp
public static partial class AppEnvironment
{
    public static string Current { get; private set; } =
#if Development            // dotnet publish -c Debug
        Development;
#elif Test                 // dotnet publish -c Release -p:Environment=Test
        Test;
#elif Staging              // dotnet publish -c Release -p:Environment=Staging
        Staging;
#else                      // dotnet publish -c Release
        Production;
#endif

    public static bool IsDevelopment() => Is(Development);
    public static bool IsTest() => Is(Test);
    public static bool IsStaging() => Is(Staging);
    public static bool IsProduction() => Is(Production);
}
```

**主要优势**：
1. **跨平台一致性**：在服务器、Web、Android、iOS、Windows 和 macOS 上工作方式完全相同。
2. **构建时配置**：环境在构建/发布时使用 MSBuild 属性确定。
3. **编译时常量**：使用 C# 预处理器指令，实现零运行时开销。
4. **类型安全**：支持 IntelliSense 和编译时检查。

**使用示例**：
```csharp
// 在任何 C# 文件中检查当前环境
if (AppEnvironment.IsDevelopment())
{
    // 特定于开发的代码
}

// 获取环境名称
string env = AppEnvironment.Current; // "Development", "Production" 等

// 设置环境（服务器项目在启动时自动完成）
AppEnvironment.Set(builder.Environment.EnvironmentName);
```

**Razor 组件用法**：
```xml
@if (AppEnvironment.IsDevelopment())
{
    <BitMessageBar MessageBarType="BitMessageBarType.Warning">
        正在开发模式下运行
    </BitMessageBar>
}
```

### MSBuild 环境配置

**位置**: [`/src/Directory.Build.props`](/src/Directory.Build.props)

构建系统根据构建配置自动配置环境：

```xml
<!-- 默认环境映射 -->
<Environment Condition="'$(Environment)' == '' AND '$(Configuration)' == 'Release'">Production</Environment>
<Environment Condition="'$(Environment)' == '' AND $(Configuration.Contains('Debug'))">Development</Environment>

<!-- 环境成为编译时常量 -->
<DefineConstants>$(DefineConstants);$(Environment);$(Configuration)</DefineConstants>
```

**这意味着什么**：
- 当您使用 `-c Debug` 构建时，会自动定义 `Development` 预处理器常量。
- 当您使用 `-c Release` 构建时，默认定义 `Production` 常量。
- 您可以使用 `-p:Environment=Staging` 或 `-p:Environment=Test` 进行覆盖。

**主要优势**：

1. **随处可用的环境信息**：您可以在以下位置访问环境信息：
   - **C# 代码**（通过 `AppEnvironment.Current`）
   - **MSBuild 脚本**（通过 `$(Environment)` 属性）
   - **Razor 组件**（通过 `@AppEnvironment.Current`）

2. **特定于环境的代码**：编写根据环境不同而编译不同的条件代码：
```csharp
#if Development
    // 此代码仅存在于开发构建中
    services.AddDeveloperTools();
#endif

#if Production
    // 仅用于生产的优化
    services.AddResponseCompression();
#endif
```

3. **平台检测**：类似的平台特定代码常量：
```csharp
#if Android
    var dataPath = FileSystem.AppDataDirectory;
#elif iOS
    var dataPath = NSFileManager.DefaultManager.GetUrls(
        NSSearchPathDirectory.DocumentDirectory, 
        NSSearchPathDomain.User)[0].Path;
#elif Windows
    var dataPath = Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData);
#endif
```

---

## 🔄 CI/CD 工作流概览

本项目包含使用 GitHub Actions 设置的完整 CI/CD 流水线，共有 **4 个工作流文件**：

### 1. 持续集成 (CI) - `ci.yml`

**文件**: [`/.github/workflows/ci.yml`](/.github/workflows/ci.yml)

**触发条件**：
- 指向任何分支的拉取请求 (Pull Request)
- 手动触发工作流分发

**执行内容**：
```
✓ 检出代码
✓ 设置 .NET SDK (来自 global.json) 和 Node.js 24
✓ 恢复工作负载 (dotnet workload restore)
✓ 构建整个解决方案 (AI.Boilerplate.slnx)
✓ 安装带有依赖项的 Playwright 浏览器
✓ 运行所有测试 (单元测试 + 集成测试 + UI 测试)
✓ 如果测试失败，将测试结果作为制品上传
```

**关键配置**：
- **运行器**: Ubuntu 24.04
- **SDK 版本**: 从 `global.json` 自动检测
- **Node 版本**: 24
- **测试制品**: 失败时保留 14 天

**重要提示**：CI 工作流确保在合并之前验证所有代码更改。它是代码质量的守门人。

---

### 2. 生产环境部署 - `cd-production.yml`

**文件**: [`/.github/workflows/cd-production.yml`](/.github/workflows/cd-production.yml)

**触发条件**：
- 推送到 `main` 分支 (自动)
- 手动触发工作流分发

**执行内容**：
```yaml
calls: cd-template.yml
with:
  ENV_NAME: "Production"
secrets: inherit
```

这是一个轻量级包装器，它使用生产环境配置触发可重用的部署模板。

---

### 3. 测试环境部署 - `cd-test.yml`

**文件**: [`/.github/workflows/cd-test.yml`](/.github/workflows/cd-test.yml)

**触发条件**：
- 推送到 `test` 分支 (自动)
- 手动触发工作流分发

**执行内容**：
```yaml
calls: cd-template.yml
with:
  ENV_NAME: "Test"
secrets: inherit
```

这触发相同的可重用模板，但使用测试环境配置。

**模式**：您可以为 Staging 或您需要的任何其他环境创建类似的文件（例如 `cd-staging.yml`）。

---

### 4. 可重用部署模板 - `cd-template.yml`

**文件**: [`/.github/workflows/cd-template.yml`](/.github/workflows/cd-template.yml)

这是处理构建和部署所有平台的**核心部署工作流**。它是一个可重用的工作流，由生产环境、测试环境（以及您创建的任何其他特定环境的工作流）调用。

**关键特性**：
- **与环境无关**：接受 `ENV_NAME` 参数（Production, Test, Staging 等）
- **多平台**：构建服务器后端、Blazor WebAssembly、Android、iOS、macOS 和 Windows
- **并行作业**：所有平台构建并行运行以提高速度
- **制品存储**：每个平台生成一个可以独立部署的制品

**作业概览**：
1. **build_api_blazor** → **deploy_api_blazor**: 服务器后端 + Blazor WebAssembly
2. **build_blazor_hybrid_windows**: Windows 桌面应用 (.exe 安装程序)
3. **build_blazor_hybrid_android**: Android 应用包 (.aab 用于 Google Play)
4. **build_blazor_hybrid_iOS**: iOS 应用包 (.ipa 用于 App Store) 和 macOS 应用

---

## 🏗️ 详细的构建和部署流水线

### 作业 1: 构建 API + Blazor WebAssembly

**平台**: Ubuntu 24.04  

**逐步过程**：

1. **环境设置**
   ```yaml
   - 检出源代码
   - 设置 .NET SDK (来自 global.json)
   - 设置 Node.js 24
   ```

2. **使用 Bit.ResxTranslator 进行本地化**
   ```bash
   dotnet tool install --global Bit.ResxTranslator
   bit-resx-translate
   ```
   - 自动翻译所有缺少值的 `.resx` 资源文件

3. **配置替换**
   ```yaml
   - 使用环境变量更新 appsettings*.json 文件
   - ServerAddress: 指向特定于环境的 API URL
   - BlazorMode: 设置为 'BlazorWebAssembly'
   - VAPID 密钥: 用于 Web 推送通知
   ```

4. **构建过程**
   ```bash
   # 安装 WebAssembly 工具
   dotnet workload install wasm-tools
   
   # 从 TypeScript 和 SCSS 生成 CSS/JS
   dotnet build -t:BeforeBuildTasks -c Release -p:Version="1.0.0"
   
   # 发布自包含的 Linux 二进制文件 (使用 Linux 是可选的)
   dotnet publish -c Release --self-contained -r linux-x64 \
     -p:Version="1.0.0" -p:Environment=Production
   ```

5. **上传制品**

---

### 作业 2: 部署 API + Blazor WebAssembly

**依赖于**: `Build API + Blazor WebAssembly` 作业

**平台**: Ubuntu 24.04

**逐步过程**：

1. **下载构建制品**
   ```yaml
   - 从前一个作业下载 server-bundle
   - 包含完整的自包含应用程序
   ```

2. **Azure Web App 部署** (可选 - 您可以使用任何托管服务)
   ```yaml
   - 部署到 Azure Web App
   - 使用来自 secrets 的发布配置文件
   - 部署到生产槽位
   - 返回 Web 应用 URL
   ```

3. **CDN 缓存清除**
   ```yaml
   - 清除 Cloudflare 缓存 (如果已配置)
   - 确保用户立即获得最新版本
   - 无过时的缓存响应
   ```

**重要提示**：
- ✅ **Azure 是可选的**：您可以部署到 AWS、Google Cloud、您自己的服务器、Docker、Kubernetes、Windows/IIS 等。
- ✅ **Cloudflare 是可选的**：仅当您使用 CDN 时才需要清除 CDN 缓存。
- ⚠️ **尚未兼容 Aspire**：后端部署工作流尚未与 .NET Aspire 编排集成。

---

### 作业 3: 构建 Windows 桌面应用

**平台**: Windows 2025  

**逐步过程**：

1. **环境设置与配置**
   ```yaml
   - 设置 .NET SDK 和 Node.js
   - 翻译资源文件 (bit-resx-translate)
   - 更新 appsettings.json:
     - ServerAddress: 特定于环境的 API URL
     - WindowsUpdate.FilesUrl: 自动更新端点
   ```

2. **使用 Velopack 构建与打包**
   ```bash
   # 生成 CSS/JS 文件
   dotnet build -t:BeforeBuildTasks -c Release
   
   # 为 Windows x86 发布 (32 位以获得更广泛的兼容性)
   dotnet publish -c Release -r win-x86 --self-contained \
     -p:Version="1.0.0" -p:Environment=Production
   
   # 使用 Velopack 创建安装程序
   dotnet vpk pack \
     -u com.company.app \           # 应用程序 ID
     -v 1.0.0 \                     # 版本
     -p .\publish-result \          # 发布文件位置
     -e AI.Boilerplate.Client.Windows.exe \  # 主可执行文件
     -r win-x86 \                   # 运行时
     --framework webview2 \         # 包含 WebView2 运行时
     --icon .\wwwroot\favicon.ico \ # 应用图标
     --packTitle 'My App'           # 显示名称
   ```

3. **上传制品**
   ```yaml
   - 将 Windows 安装程序 (.exe) 上传到制品
   - 位于 Releases 文件夹中
   ```

**Velopack 特性**：
- **自动更新支持**：内置自动更新机制
- **WebView2 运行时**：将 Microsoft Edge WebView2 与安装程序打包
- **增量更新**：仅下载更新的更改文件
- **x86 构建**：32 位构建可在 32 位和 64 位 Windows 上运行

---

### 作业 4: 构建 Android 应用

**平台**: Ubuntu 24.04  

**逐步过程**：

1. **设置 Android 签名**
   ```yaml
   - 从 base64 编码的 secret 中提取 Android 签名密钥
   - 保存为项目目录中的 AI.Boilerplate.keystore
   - 配置 keystore 用于发布签名
   ```
   - Keystore 以 base64 格式存储为 GitHub secret 以确保安全

2. **构建 Android App Bundle (AAB)**
```bash
# 安装 MAUI Android 工作负载
dotnet workload install maui-android
   
# 安装 Android SDK 平台工具
${ANDROID_SDK_ROOT}/cmdline-tools/latest/bin/sdkmanager \
  --sdk_root=$ANDROID_SDK_ROOT "platform-tools"
   
# 生成 CSS/JS 文件
dotnet build -t:BeforeBuildTasks -c Release
   
# 发布签名的 AAB (或根据需要发布 APK)
dotnet publish -c Release \
  -p:ApplicationId=com.company.app \
  -p:AndroidPackageFormat=aab \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyStore="AI.Boilerplate.keystore" \
  -p:AndroidSigningKeyAlias=AI.Boilerplate \
  -p:AndroidSigningKeyPass="${{ secrets.ANDROID_RELEASE_KEYSTORE_PASSWORD }}" \
  -p:AndroidSigningStorePass="${{ secrets.ANDROID_RELEASE_SIGNING_PASSWORD }}" \
  -p:Version="1.0.0" \
  -p:Environment=Production \
  -f net10.0-android
```

3. **上传制品**
   ```yaml
   - 上传签名的 .aab 文件
   - 准备好提交到 Google Play Store
   ```

**Android 签名配置**：
- **包格式**: AAB (Android App Bundle) - Google Play Store 必需
- **Keystore**: 所有应用更新必须使用相同的签名密钥
- **别名**: Keystore 中密钥的标识符
- **密码**: Keystore 密码和密钥密码 (存储为 secrets)

**重要提示**：签名密钥**至关重要**。如果您丢失了它，将无法在 Google Play Store 上更新您的应用！

---

### 作业 5: 构建 iOS 和 macOS 应用

**平台**: macOS 26  

**逐步过程**：

1. **设置 Apple 开发环境**
   ```yaml
   - 设置 .NET SDK
   - 设置 Xcode 26.0 (最新版)
   - 设置 Node.js 24
   - 翻译资源 (bit-resx-translate)
   - 使用 ServerAddress 更新 appsettings.json
   ```

2. **Apple 代码签名设置**
   ```yaml
   # 导入分发证书
   - 从 base64 secret 导入 P12 证书
   - 从 secrets 获取证书密码
   
   # 下载配置描述文件
   - 使用 App Store Connect API
   - 需要: Issuer ID, API Key ID, 私钥
   - 为指定的 Bundle ID 下载描述文件
   ```

3. **构建 iOS 应用包 (IPA)**
   ```bash
   # 安装 MAUI 工作负载 (包括 iOS 支持)
   dotnet workload install maui
   
   # 生成 CSS/JS 文件
   dotnet build -t:BeforeBuildTasks -c Release
   
   # 发布并签名 IPA
   dotnet publish \
     -p:ApplicationId=com.company.app \
     -p:RuntimeIdentifier=ios-arm64 \     # 用于物理设备
     -c Release \
     -p:ArchiveOnBuild=true \             # 创建 .ipa
     -p:CodesignKey="iPhone Distribution" \  # 证书名称
     -p:CodesignProvision="MyApp Provisioning" \  # 描述文件名称
     -p:Version="1.0.0" \
     -p:Environment=Production \
     -f net10.0-ios
   ```

4. **上传制品**
   ```yaml
   - 上传签名的 .ipa 文件
   - 准备好通过 App Store Connect 提交到 App Store
   ```

**Apple 要求**：
- **分发证书**: 签署提交到 App Store 的应用所必需
- **配置描述文件**: 链接您的应用 ID、证书和设备
- **App Store Connect API**: 自动化描述文件下载
- **Bundle ID**: 必须与 App Store Connect 中的应用标识符匹配
- **Xcode**: iOS/macOS 构建所必需 (仅在 macOS 上可用)

**iOS 构建说明**：
- **运行时标识符**: `ios-arm64` 用于物理 iOS 设备 (iPhone, iPad)
- **签名**: 证书和配置描述文件都必须有效且匹配
- **macOS 同一作业**: 工作流可以使用类似的步骤扩展以构建 macOS 应用

---

## 🎯 两阶段部署架构 (最佳实践)

该工作流遵循**以安全为中心的两阶段部署**模式，将构建与部署分离：

### 第一阶段：构建
**目的**: 编译、捆绑和打包应用程序  
**运行器类型**: 功能丰富的构建代理

**特点**：
- 拥有完整的 SDK 安装 (.NET, Node.js, Android SDK, Xcode)
- 执行编译、转译、捆绑
- 运行测试和质量检查
- 将制品上传到 GitHub (或 Azure DevOps)
- **无生产访问权限** - 与生产系统隔离

### 第二阶段：部署
**目的**: 获取预构建的制品并部署它们  
**运行器类型**: 轻量级部署代理

**特点**：
- 不需要 SDK - 仅需部署工具
- **更安全**: 工具有限，攻击面最小
- **直接生产访问**: 可以连接到生产服务器
- 下载预构建的制品并部署
- 无编译或构建过程

### 为什么这种分离很重要

```
┌─────────────────────────────┐         ┌─────────────────────────────┐
│    构建代理 (重型)           │         │   部署代理 (轻型)            │
├─────────────────────────────┤         ├─────────────────────────────┤
│ ✓ .NET SDK                  │         │ ✗ 无 SDK                    │
│ ✓ Node.js & npm             │         │ ✓ 仅部署工具                │
│ ✓ Android SDK               │         │ ✓ 直接生产访问              │
│ ✓ Xcode (用于 iOS/macOS)     │         │ ✓ 最小攻击面                │
│ ✓ 构建工具和编译器          │         │ ✓ 安全加固                  │
│ ✗ 无生产访问权限            │         │ ✓ 可部署到服务器            │
└─────────────────────────────┘         └─────────────────────────────┘
```

**安全优势**：
1. **最小权限原则**: 拥有生产访问权限的代理安装的软件最少。
2. **减少攻击面**: 工具越少 = 潜在漏洞越少。
3. **关注点分离**: 构建失败不会影响生产；部署问题不会损坏构建制品。
4. **审计追踪**: 清晰区分构建内容和部署内容。

**工作流示例**：
```yaml
# 第一阶段：构建 (无生产访问权限)
build_api_blazor:
  runs-on: ubuntu-24.04
  steps:
    - uses: actions/checkout@v6
    - uses: actions/setup-dotnet@v5
    - uses: actions/setup-node@v6
    - run: dotnet publish ...
    - uses: actions/upload-artifact@v5  # 保存制品

# 第二阶段：部署 (拥有生产访问权限)
deploy_api_blazor:
  needs: build_api_blazor  # 依赖于第一阶段
  runs-on: ubuntu-24.04
  steps:
    - uses: actions/download-artifact@v6  # 获取预构建制品
    - uses: azure/webapps-deploy@v3      # 部署到生产环境
```

**结果**：即使攻击者破坏了构建代理（安装了许多工具），他们也无法访问生产环境。反之，如果部署代理被破坏，攻击者也无法将恶意代码注入构建过程。

---

## 🔐 所需的 Secrets 和 Variables

CI/CD 流水线需要在 GitHub 中配置各种 secrets 和 variables。这些是按环境（Production, Test, Staging 等）存储的。

### 如何在 GitHub 中配置

1. 导航到您的仓库 → **Settings** → **Environments**
2. 创建环境（例如 "Production", "Test"）
3. 添加 **Secrets** (加密，永不暴露) 和 **Variables** (纯文本，可见)

---

### 特定于环境的配置示例

**生产环境**：
```
Variables:
  SERVER_ADDRESS = https://api.myapp.com
  APP_VERSION = 1.0.0
  APP_TITLE = My App

Secrets:
  AZURE_PUBLISH_PROFILE = <生产发布配置文件>
  PUBLIC_VAPIDKEY = <生产 VAPID 密钥>
```

**测试环境**：
```
Variables:
  SERVER_ADDRESS = https://test-api.myapp.com
  APP_VERSION = 1.0.0
  APP_TITLE = My App (Test)

Secrets:
  AZURE_PUBLISH_PROFILE = <测试发布配置文件>
  PUBLIC_VAPIDKEY = <测试 VAPID 密钥>
```

这允许相同的工作流将不同的配置部署到不同的环境。

---

## 📝 构建时配置替换

在 CI/CD 过程中，工作流会在构建应用程序**之前**修改 `appsettings.json` 文件。这允许在不维护多个配置文件的情况下进行特定于环境的配置。

### 工作原理

工作流使用 `variable-substitution` action 来替换 JSON 文件中的值：

```yaml
- name: Update core appsettings.json
  uses: devops-actions/variable-substitution@v1.2 
  with:
    files: 'src/**/appsettings*json'  # 匹配所有 appsettings 文件
  env:
    ServerAddress: ${{ vars.SERVER_ADDRESS }}
    WebAppRender.BlazorMode: 'BlazorWebAssembly'
    AdsPushVapid.PublicKey: ${{ secrets.PUBLIC_VAPIDKEY }}
```

### 替换内容

**之前** (在仓库中)：
```json
{
  "ServerAddress": "https://localhost:5001",
  "WebAppRender": {
    "BlazorMode": "BlazorServer"
  },
  "AdsPushVapid": {
    "PublicKey": "dev-key"
  }
}
```

**之后** (在构建期间)：
```json
{
  "ServerAddress": "https://api.myapp.com",
  "WebAppRender": {
    "BlazorMode": "BlazorWebAssembly"
  },
  "AdsPushVapid": {
    "PublicKey": "production-vapid-key"
  }
}
```

### 为什么这很重要

**优势**：
1. **单一事实来源**: 仓库中只有一组 appsettings 文件。
2. **代码中无机密**: 生产值存储在 GitHub Secrets/Variables 中。
3. **特定于环境**: 相同的工作流为不同环境生成不同的构建。
4. **无需手动编辑**: 自动替换消除了人为错误。

---

### 预期的应用大小

根据 `dotnet new bit-bp` 和 `dotnet publish` 命令的参数，应用大小预计在以下范围内：

- **Web** => 3.5MB 到 7MB
  在 `dotnet publish` 命令中启用/禁用 LLVM 以及在 `dotnet new bit-bp` 命令中使用 `--offlineDb` 参数会有巨大影响。
---
- **Android** => 18MB 到 35MB
  在 `dotnet publish` 命令中启用/禁用 LLVM 影响最大。`dotnet new` 参数对此影响不大。
---
- **Windows** => 30MB 到 55MB
  在 `dotnet publish` 命令中启用/禁用 AOT 影响最大。`dotnet new` 参数或 x86/x64 对此影响不大。
---
- **iOS/macOS** => 120MB 到 130MB
---
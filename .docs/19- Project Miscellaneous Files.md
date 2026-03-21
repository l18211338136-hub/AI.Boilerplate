# 第十九阶段：项目杂项文件

欢迎回到入门指南的**第十九阶段**！在本阶段，我们将探索项目根目录下的各种配置和杂项文件。理解这些文件对于维护代码质量、管理依赖项以及配置开发环境至关重要。

---

## 1. 配置文件

### 1.1 `.editorconfig`

**位置**: [`/.editorconfig`](/.editorconfig)

**用途**: `.editorconfig` 文件确保在不同的编辑器和 IDE 之间保持**一致的编码风格**。这对于团队协作和维持代码质量至关重要。

**主要特性**:

- **缩进**: 所有文件使用 4 个空格
- **C# 约定**: 强制执行现代 C# 编码标准
  - `var` 偏好
  - 表达式主体成员
  - 模式匹配
  - 空检查偏好
  - 文件作用域命名空间（强制为警告）
- **格式化规则**: 
  - 换行偏好（大括号、else、catch、finally）
  - switch 语句的缩进
  - 运算符周围的空格偏好
- **命名约定**: 常量字段使用 PascalCase

**文件示例**:

```properties
[*.cs]
# 强制执行文件作用域命名空间
csharp_style_namespace_declarations = file_scoped:warning

# var 偏好
csharp_style_var_for_built_in_types = true:silent
csharp_style_var_when_type_is_apparent = true:silent

# 左大括号前换行
csharp_new_line_before_open_brace = all
```

**为什么重要**: 当您编写代码时，IDE 会自动应用这些规则，无需手动努力即可确保一致性。这防止了拉取请求中的“代码风格之争”。

---

### 1.2 `global.json`

**位置**: [`/global.json`](/global.json)

**用途**: 指定项目所需的 **.NET SDK 版本**。

**内容**:

```json
{
    "sdk": {
        "version": "10.0.100",
        "rollForward": "latestFeature"
    }
}
```

**关键属性**:

- **`version`**: 特定的 .NET SDK 版本（当前为 .NET 10 RC）
- **`rollForward`**: 设置为 `"latestFeature"` - 允许自动使用更新的特性发布版本

**为什么重要**: 这确保所有团队成员和 CI/CD 流水线使用相同的 SDK 版本，防止出现“在我机器上能运行”的问题。

```json
{
    "sdk": {
        "version": "10.0.100",
        "rollForward": "latestFeature"
    },
    "test": {
        "runner": "Microsoft.Testing.Platform"
    }
}
```

**其他属性**:

- **`test.runner`**: 配置 .NET 测试运行器使用 **Microsoft.Testing.Platform** (MSTest v4)，与旧版测试运行器相比，它提供了更好的性能和功能。

---

## 2. 解决方案文件

### 2.1 `AI.Boilerplate.sln`

**位置**: [`/AI.Boilerplate.sln`](/AI.Boilerplate.sln)

**用途**: Visual Studio 的**完整解决方案文件**，包含工作区中的**所有**项目。

**包含内容**:
- 服务器项目 (`AI.Boilerplate.Server.Web`, `AI.Boilerplate.Server.Api`, `AI.Boilerplate.Server.Shared`, `AI.Boilerplate.Server.AppHost`)
- 客户端项目 (`AI.Boilerplate.Client.Core`, `AI.Boilerplate.Client.Web`, `AI.Boilerplate.Client.Maui`, `AI.Boilerplate.Client.Windows`)
- 共享项目 (`AI.Boilerplate.Shared`)
- 测试项目 (`AI.Boilerplate.Tests`)
- 组织配置文件解决方案文件夹

**使用时机**: 当您需要同时跨所有平台和项目工作时，打开此文件。

---

### 2.2 `AI.Boilerplate.Web.slnf`

**位置**: [`/AI.Boilerplate.Web.slnf`](/AI.Boilerplate.Web.slnf)

**用途**: 一个**解决方案筛选器**，仅包含**Web 开发**所需的项目。

**存在原因**: 加载所有项目（包括 MAUI、Windows）可能很慢且消耗资源。当您只处理 Web 功能时，此筛选器可加快开发体验。

**包含的项目**:
- `AI.Boilerplate.Server.Web`
- `AI.Boilerplate.Server.Api`
- `AI.Boilerplate.Server.Shared`
- `AI.Boilerplate.Server.AppHost`
- `AI.Boilerplate.Client.Core`
- `AI.Boilerplate.Client.Web`
- `AI.Boilerplate.Shared`
- `AI.Boilerplate.Tests`

**使用时机**: 这是 VS Code 中的**默认解决方案**（见 `.vscode/settings.json`）。当您不处理移动/桌面功能时，使用它可加快构建速度。

---

### 2.3 `AI.Boilerplate.slnx`

**位置**: [`/AI.Boilerplate.slnx`](/AI.Boilerplate.slnx)

**用途**: Visual Studio 2025 引入的新的**基于 XML 的解决方案格式**。与传统的 `.sln` 格式相比，它更易于人类阅读且对 Git 更友好。

**优势**: 在源代码控制中更容易合并，更易于维护，并支持现代 Visual Studio 功能。

---

## 3. 构建配置文件

### 3.1 `Directory.Build.props`

**位置**: [`/src/Directory.Build.props`](/src/Directory.Build.props)

**用途**: 一个**共享的 MSBuild 属性文件**，自动应用于 `src/` 目录及其子目录中的**所有**项目。这避免了在各个 `.csproj` 文件中重复配置。

**关键配置**:

#### 通用属性
```xml
<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
```

#### 版本控制
```xml
<Version>1.0.0</Version>
<ApplicationDisplayVersion>$(Version)</ApplicationDisplayVersion>
```

**重要提示**: 在此处更改版本以更新所有项目的版本。

#### 环境配置
```xml
<Environment Condition="'$(Environment)' == '' AND '$(Configuration)' == 'Release'">Production</Environment>
<Environment Condition="'$(Environment)' == '' AND $(Configuration.Contains('Debug'))">Development</Environment>
```

这会创建编译时常量，如 `DEBUG`, `Development`, `Production`，您可以在 C# 中使用：

```csharp
#if DEBUG
    // 仅调试代码
#endif

#if Production
    // 仅生产代码
#endif
```

#### 平台检测
```xml
<DefineConstants Condition="$(TargetFramework.Contains('-android'))">$(DefineConstants);Android</DefineConstants>
<DefineConstants Condition="$(TargetFramework.Contains('-ios'))">$(DefineConstants);iOS</DefineConstants>
<DefineConstants Condition="$(TargetFramework.Contains('-windows'))">$(DefineConstants);Windows</DefineConstants>
```

这允许平台特定代码：

```csharp
#if Android
    // Android 特定代码
#endif
```

#### 全局 Using 指令
```xml
<Using Include="System.Net.Http" />
<Using Include="System.Text.Json" />
<Using Include="AI.Boilerplate.Shared.Dtos" />
<Using Include="AI.Boilerplate.Shared.Exceptions" />
<!-- ... 等等 -->
```

这些命名空间会在每个 C# 文件中**自动导入**，消除了重复 `using` 语句的需要。

---

### 3.2 `Directory.Packages.props`

**位置**: [`/src/Directory.Packages.props`](/src/Directory.Packages.props)

**用途**: 为 NuGet 包启用**中央包管理 (CPM)**。所有包版本都在一个地方定义。

**工作原理**:

不在每个 `.csproj` 中写：
```xml
<PackageReference Include="Bit.BlazorUI" Version="10.4.2" />
```

而是在 `.csproj` 中写：
```xml
<PackageReference Include="Bit.BlazorUI" />
```

版本在 `Directory.Packages.props` 中集中定义：
```xml
<PackageVersion Include="Bit.BlazorUI" Version="10.4.2" />
```

**优势**:
- 包版本的**单一事实来源**
- **易于更新**: 更新一次版本，影响所有项目
- **防止**项目间的版本冲突

**包含的包示例**:

```xml
<PackageVersion Include="Bit.BlazorUI" Version="10.4.2" />
<PackageVersion Include="Bit.Butil" Version="10.4.2" />
<PackageVersion Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.0" />
<PackageVersion Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
<PackageVersion Include="Hangfire.AspNetCore" Version="1.8.21" />
<PackageVersion Include="Riok.Mapperly" Version="4.3.0" />
<!-- ... 等等 -->
```

**更新包**: 只需在此文件中更改版本并重新构建。

---

## 4. 清理脚本

### 4.1 `Clean.bat` (Windows)

**位置**: [`/Clean.bat`](/Clean.bat)

**用途**: 用于 **Windows** 的基于 PowerShell 的清理脚本，用于删除构建产物和临时文件。

**执行内容**:
1. 删除未跟踪的 CSS、JS 和 map 文件（未纳入 Git 的生成文件）
2. 对所有 `.csproj` 文件运行 `dotnet clean`
3. 删除常见的构建文件夹：`bin`, `obj`, `node_modules`, `.vs` 等
4. 删除空目录

**使用时机**: 
- 当项目因缓存损坏而无法构建时
- 在切换到具有重大结构变更的分支之前
- 当您希望确保完全干净的构建时

**⚠️ 重要提示**: 运行此脚本前请关闭所有 IDE（Visual Studio, VS Code）以防止文件锁定。

---

### 4.2 `Clean.sh` (macOS/Linux)

**位置**: [`/Clean.sh`](/Clean.sh)

**用途**: `Clean.bat` 的 **macOS/Linux 等效版本**。

**用法**:
```bash
chmod +x Clean.sh  # 使其可执行（仅首次）
./Clean.sh         # 运行脚本
```

---

## 5. 本地化配置

### 5.1 `Bit.ResxTranslator.json`

**位置**: [`/Bit.ResxTranslator.json`](/Bit.ResxTranslator.json)

**用途**: **bit-resx** CLI 工具的配置，该工具使用 AI 自动翻译 `.resx` 资源文件。

**配置**:

```json
{
    "DefaultLanguage": "en",
    "SupportedLanguages": [ "nl", "fa", "sv", "hi", "zh", "es", "fr", "ar", "de" ],
    "ResxPaths": [ "/src/**/*.resx" ],
    "OpenAI": {
        "Model": "gpt-4.1-mini",
        "Endpoint": "https://models.inference.ai.azure.com",
        "ApiKey": null
    }
}
```

**工作原理**:

1. 您在 `AppStrings.resx` 中用英语编写字符串
2. 运行 `bit-resx` 工具
3. 它自动创建翻译版本：
   - `AppStrings.nl.resx` (荷兰语)
   - `AppStrings.fa.resx` (波斯语)
   - `AppStrings.sv.resx` (瑞典语)
   - `AppStrings.hi.resx` (印地语)
   - `AppStrings.zh.resx` (中文)
   - 等等...

**支持的语言**: 目前配置了 9 种语言（荷兰语、波斯语、瑞典语、印地语、中文、西班牙语、法语、阿拉伯语、德语）

**AI 提供商**: 使用 OpenAI 的 GPT-4.1-mini 模型（或 Azure OpenAI）

**了解更多**: 参见 [bit-resx 文档](https://github.com/bitfoundation/bitplatform/tree/develop/src/ResxTranslator)

**安装和使用**:

```bash
# 全局安装 bit-resx CLI 工具
dotnet tool install --global Bit.ResxTranslator

# 运行翻译器（从项目根目录）
bit-resx
```

**添加新语言**:
1. 将其 ISO 代码添加到 `SupportedLanguages`（例如意大利语用 `"it"`）
2. 更新 `Shared` 项目中的 `CultureInfoManager.SupportedCultures`
3. 如有需要，更新 `.Client.Maui/Platforms/Android/MainActivity.cs`
4. 运行 `bit-resx` 生成翻译

---

## 6. IDE 配置文件

### 6.1 `.vsconfig`

**位置**: [`/.vsconfig`](/.vsconfig)

**用途**: 指定本项目所需的 **Visual Studio 工作负载和扩展**。

**内容**:

```json
{
    "components": [
        "Microsoft.VisualStudio.Workload.NetWeb",
        "Microsoft.VisualStudio.Workload.NetCrossPlat",
        "Component.Android.SDK.MAUI"
    ],
    "extensions": [
        "https://marketplace.visualstudio.com/items?itemName=TomEnglert.ResXManager"
    ]
}
```

**如何帮助**: 当有人克隆仓库并在 Visual Studio 中打开它时，系统会提示他们自动安装所需的工作负载和扩展。

**所需工作负载**:
- **NetWeb**: ASP.NET 和 Web 开发
- **NetCrossPlat**: 用于跨平台应用的 .NET MAUI 开发
- **Android SDK**: Android 开发支持

**所需扩展**:
- **ResX Manager**: 一个 Visual Studio 扩展，用于通过用户友好的界面管理资源文件 (`.resx`)

---

### 6.2 `settings.VisualStudio.json`

**位置**: [`/settings.VisualStudio.json`](/settings.VisualStudio.json)

**用途**: Visual Studio（非 VS Code）的**项目特定设置**。

**关键设置**:

```json
{
    "languages.defaults.tabs.tabSize": 4,
    "languages.defaults.general.lineNumbers": true,
    "environment.documents.saveWithSpecificEncoding": true,
    "environment.documents.saveEncoding": "utf-8-nobom;65001",
    "debugging.hotReload.enableHotReload": true,
    "debugging.hotReload.applyOnFileSave": true,
    "debugging.general.disableJITOptimization": true,
    "debugging.hotReload.enableForNoDebugLaunch": true,
    "projectsAndSolutions.aspNetCore.general.hotReloadCssChanges": true,
    "copilot.general.completions.enableNextEditSuggestions": true,
    "copilot.general.editor.enableAdaptivePaste": true
}
```

**显著特性**:
- **热重载 (Hot Reload)**: 启用以加快开发速度（无需重启即可应用更改）
- **CSS 热重载**: CSS 更改立即生效
- **UTF-8 编码**: 所有文件保存为不带 BOM 的 UTF-8
- **GitHub Copilot**: 启用增强功能（下一个编辑建议、自适应粘贴）

---

### 6.3 `.vscode/` 文件夹

**位置**: [`/.vscode/`](/.vscode/)

**用途**: 包含 **VS Code 工作区设置**和配置。

#### 6.3.1 `.vscode/mcp.json`

**位置**: [`/.vscode/mcp.json`](/.vscode/mcp.json)

**用途**: **模型上下文协议 (MCP) 服务器**的配置，通过专业知识和工具扩展 GitHub Copilot 的功能。

**内容**:

```json
{
    "servers": {
        "DeepWiki": {
            "type": "sse",
            "url": "https://mcp.deepwiki.com/mcp"
        }
    }
}
```

**什么是 MCP？**:
- **模型上下文协议 (Model Context Protocol)**: 一种将 AI 助手（如 GitHub Copilot）连接到外部数据源和工具的标准方法
- **服务器发送事件 (SSE)**: 用于从 MCP 服务器流式传输数据的通信协议

**DeepWiki 服务器**:
- **用途**: 为 GitHub Copilot 提供关于特定仓库的深入知识
- **在本项目中**: 配置为访问 `bitfoundation/bitplatform` 和 `riok/mapperly` 仓库
- **如何帮助**: 当您询问 Copilot 关于 Bit.BlazorUI 组件、Mapperly 或其他 bitplatform 功能的问题时，它可以查询 DeepWiki 服务器以获取准确、最新的文档

**使用示例**:
- 问：*"如何使用 BitDataGrid 进行服务端分页？"*
- Copilot 使用 DeepWiki MCP 服务器从 bitplatform 仓库获取相关信息
- 您获得基于实际源代码和文档的准确答案

**添加更多 MCP 服务器**:
您可以添加额外的 MCP 服务器以进一步扩展 Copilot 的功能。例如：
- 数据库文档服务器
- 内部公司知识库
- API 文档服务器

**了解更多**: 访问 [modelcontextprotocol.io](https://modelcontextprotocol.io) 获取有关 MCP 的更多信息。

---

#### 6.3.2 `.vscode/settings.json`

```json
{
    "liveSassCompile.settings.watchOnLaunch": true,
    "dotnet.defaultSolution": "AI.Boilerplate.Web.slnf",
    "dotnet.unitTests.runSettingsPath": "src/Tests/.runsettings",
    "chat.tools.autoApprove": true,
    "github.copilot.chat.codesearch.enabled": true,
    "csharp.preview.improvedLaunchExperience": true,
    "explorer.fileNesting.enabled": true,
    "explorer.fileNesting.patterns": {
        "*.resx": "$(capture).*.resx",
        "*.razor": "$(capture).*.razor, $(capture).razor.cs, $(capture).razor.scss",
        "*.scss": "$(capture).*.scss, $(capture).css, $(capture).css.map",
        "*.json": "$(capture).*.json"
    }
}
```

**关键特性**:

- **实时 SASS 编译**: 启动时自动编译 SCSS 文件
- **默认解决方案**: 使用 `AI.Boilerplate.Web.slnf` 以加快加载速度
- **文件嵌套**: 在资源管理器中将相关文件分组在一起
  - `Component.razor`, `Component.razor.cs`, `Component.razor.scss` 嵌套在 `Component.razor` 下
  - `AppStrings.resx`, `AppStrings.fa.resx`, `AppStrings.nl.resx` 嵌套在 `AppStrings.resx` 下
- **GitHub Copilot**: 自动批准工具使用，启用代码搜索

#### 6.3.3 `.vscode/extensions.json`

**推荐扩展**:

```json
{
    "recommendations": [
        "GitHub.copilot",
        "glenn2223.live-sass",
        "GitHub.copilot-chat",
        "ms-dotnettools.csharp",
        "ms-dotnettools.csdevkit",
        "ms-dotnettools.dotnet-maui",
        "ms-azuretools.vscode-docker",
        "ms-vscode-remote.remote-containers",
        "ms-dotnettools.blazorwasm-companion",
        "ms-dotnettools.vscode-dotnet-runtime"
    ]
}
```

当您在 VS Code 中打开项目时，系统会提示您安装这些扩展。

#### 6.3.4 `.vscode/tasks.json`

**预配置任务**:

- **`before-build`**: 运行 TypeScript 编译和 SCSS 处理（打开文件夹时自动运行）
- **`build`**: 构建 `AI.Boilerplate.Server.Web` 项目
- **`generate-resx-files`**: 从 `.resx` 文件生成 C# 代码
- **`run`**: 启动应用程序
- **`run-tests`**: 运行所有测试

**如何使用**: 按 `Ctrl+Shift+P` → `Tasks: Run Task` → 选择任务

#### 6.3.5 `.vscode/launch.json`

**调试配置**:

```json
{
    "configurations": [
        {
            "name": "C#: AI.Boilerplate.Server.Web Debug",
            "type": "dotnet",
            "request": "launch",
            "projectPath": "${workspaceFolder}/src/Server/AI.Boilerplate.Server.Web/AI.Boilerplate.Server.Web.csproj"
        },
        {
            "name": "C#: AI.Boilerplate.Server.Api Debug",
            "type": "dotnet",
            "request": "launch",
            "projectPath": "${workspaceFolder}/src/Server/AI.Boilerplate.Server.Api/AI.Boilerplate.Server.Api.csproj"
        },
        {
            "name": ".NET MAUI",
            "type": "maui",
            "request": "launch",
            "preLaunchTask": "maui: Build"
        }
    ]
}
```

**如何使用**: 按 `F5` 或转到“运行和调试”面板 → 选择配置 → 开始调试

---

## 7. 源代码控制

### 7.1 `.gitignore`

**位置**: [`/.gitignore`](/.gitignore)

**用途**: 指定 Git 应**忽略**（不在版本控制中跟踪）的文件和文件夹。

**忽略的内容**:

- **构建产物**: `bin/`, `obj/`, `*.cache`
- **IDE 文件**: `.vs/`, `.idea/`
- **依赖项**: `node_modules/`, `packages/`
- **生成文件**: `*.css` (来自 SCSS), `*.js` (来自 TypeScript), `*.map`
- **用户特定文件**: `*.user`, `*.suo`
- **数据库文件**: `*.mdf`, `*.ldf`
- **环境文件**: `.env`
- **自定义 AI.Boilerplate 模式**:
  - `*Resource.designer.cs` (从 `.resx` 自动生成)
  - `/src/Client/AI.Boilerplate.Client.Core/Scripts/*.js` (从 TypeScript 生成)
  - `/src/Client/AI.Boilerplate.Client.Maui/Platforms/Android/google-services.json` (敏感的 Firebase 配置)

**为什么重要**: 保持仓库整洁，防止意外提交敏感数据或大型二进制文件。

---

## 8. 文档

### 8.1 `README.md`

**位置**: [`/README.md`](/README.md)

**用途**: 项目的**欢迎文件**，显示在 GitHub/Azure DevOps 上。

**包含内容**:
- 欢迎信息
- 模板创建命令（确切展示此项目是如何生成的）
- 指向 [bitplatform.dev/templates](https://bitplatform.dev/templates/overview) 综合文档的链接
- 关于所用模板版本的信息

**示例**:

````markdown
This project gets generated by bit-bp template v-10.4.2 using the following command
```bash
dotnet new bit-bp
    --name AI.Boilerplate
    --database SqlServer
    --filesStorage S3
    --module Admin
    --captcha reCaptcha
    --sample
    --sentry
    --appInsights
    --signalR
    --offlineDb
    --ads
```
````

这对于记住项目最初是如何配置的很有用。

---

## 9. 拼写检查

### 9.1 `vs-spell.dic`

**位置**: [`/vs-spell.dic`](/vs-spell.dic)

**用途**: 拼写检查器（Visual Studio, VS Code 扩展）的**自定义词典**，包含技术术语和项目特定词汇。

**包含的单词示例**:

```plaintext
webp
resx
nameof
Mapperly
Blazor
Json
editorconfig
Hangfire
rendermode
sqlite
Besql
appsettings
Butil
webauthn
scss
webassembly
odata
totp
aspnetcore
sqlserver
postgres
slnx
slnf
```

**为什么重要**: 防止对 `webauthn`, `odata`, `Blazor` 等技术术语产生误报的拼写检查警告。

**引用位置**: `.editorconfig` 中的 `spelling_exclusion_path = vs-spell.dic`

---

## 10. CI/CD 流水线

### 10.1 `.github/workflows/`

**位置**: [`/.github/workflows/`](/.github/workflows/)

**用途**: 包含用于持续集成和持续部署的 **GitHub Actions 工作流**。

**工作流文件**:

1. **`ci.yml`**: 持续集成
   - 每次推送/拉取请求时运行
   - 构建所有项目
   - 运行测试
   - 确保代码质量

2. **`cd-test.yml`**: 部署到测试环境
   - 手动触发或合并到 `develop` 分支时触发
   - 部署到测试/暂存服务器

3. **`cd-production.yml`**: 部署到生产环境
   - 手动触发或合并到 `main` 分支时触发
   - 部署到生产服务器

4. **`cd-template.yml`**: 用于创建自定义 CD 工作流的模板
   - 复制并根据您的特定部署需求进行定制

**注意**: 当前的工作流是为客户端平台（Android, iOS, Windows, macOS）配置的，但后端部署尚未完全兼容 Aspire。后端 CI/CD 可能需要额外配置。

**最佳实践**: 工作流使用**两阶段部署**:
1. **构建阶段**: 构建项目并将制品上传到 GitHub/Azure DevOps
2. **部署阶段**: 下载制品并部署到目标环境

**为什么分两阶段？**: 
- 为构建和部署使用不同的运行器
- 部署运行器可以是轻量级的（不需要 SDK）
- 更安全 - 部署代理不需要完整的构建工具

---

## 11. 开发容器

### 11.1 `.devcontainer/`

**位置**: [`/.devcontainer/`](/.devcontainer/)

**用途**: **VS Code 开发容器**和 **GitHub Codespaces** 的配置。

**功能**: 允许您在 Docker 容器内进行开发，所有依赖项都已预装，确保所有团队成员拥有**一致的开发环境**。

**优势**:
- 无需在本地安装 .NET SDK、Node.js 或其他工具
- 在 Windows、macOS 和 Linux 上工作方式完全相同
- 在 GitHub Codespaces 中一键设置
- 与主机系统隔离

**如何使用**:
1. 在 VS Code 中安装 Docker Desktop 和 "Dev Containers" 扩展
2. 在 VS Code 中打开项目
3. 出现提示时点击 "Reopen in Container"
4. 等待容器构建完成
5. 开始开发！

---
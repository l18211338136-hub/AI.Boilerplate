# 第十阶段：配置 (appsettings.json)

欢迎回到第十阶段！在本阶段，您将了解本项目如何使用 `appsettings.json` 文件在不同平台和环境之间管理配置。

## 配置架构概述

本项目采用分层配置系统，设置从共享配置文件级联到特定于平台的文件。这允许您一次性定义通用设置，并根据需要为特定平台或环境覆盖它们。

## 配置文件位置

解决方案中的每个项目都有自己的一套 `appsettings.json` 文件：

### 共享配置
- [`src/Shared/appsettings.json`](/src/Shared/appsettings.json) - 所有平台共享的基础配置
- [`src/Shared/appsettings.Development.json`](/src/Shared/appsettings.Development.json) - 开发环境覆盖
- [`src/Shared/appsettings.Production.json`](/src/Shared/appsettings.Production.json) - 生产环境覆盖

### 客户端核心配置
- [`src/Client/AI.Boilerplate.Client.Core/appsettings.json`](/src/Client/AI.Boilerplate.Client.Core/appsettings.json) - 客户端共享配置
- [`src/Client/AI.Boilerplate.Client.Core/appsettings.Development.json`](/src/Client/AI.Boilerplate.Client.Core/appsettings.Development.json) - 开发环境覆盖
- [`src/Client/AI.Boilerplate.Client.Core/appsettings.Production.json`](/src/Client/AI.Boilerplate.Client.Core/appsettings.Production.json) - 生产环境覆盖

### 特定于平台的客户端配置
- **Blazor WebAssembly**: 
  - [`src/Client/AI.Boilerplate.Client.Web/appsettings.json`](/src/Client/AI.Boilerplate.Client.Web/appsettings.json)
  - [`src/Client/AI.Boilerplate.Client.Web/appsettings.Development.json`](/src/Client/AI.Boilerplate.Client.Web/appsettings.Development.json)
  - [`src/Client/AI.Boilerplate.Client.Web/appsettings.Production.json`](/src/Client/AI.Boilerplate.Client.Web/appsettings.Production.json)
- **.NET MAUI**: 
  - [`src/Client/AI.Boilerplate.Client.Maui/appsettings.json`](/src/Client/AI.Boilerplate.Client.Maui/appsettings.json)
  - [`src/Client/AI.Boilerplate.Client.Maui/appsettings.Development.json`](/src/Client/AI.Boilerplate.Client.Maui/appsettings.Development.json)
  - [`src/Client/AI.Boilerplate.Client.Maui/appsettings.Production.json`](/src/Client/AI.Boilerplate.Client.Maui/appsettings.Production.json)
- **Windows**: 
  - [`src/Client/AI.Boilerplate.Client.Windows/appsettings.json`](/src/Client/AI.Boilerplate.Client.Windows/appsettings.json)
  - [`src/Client/AI.Boilerplate.Client.Windows/appsettings.Development.json`](/src/Client/AI.Boilerplate.Client.Windows/appsettings.Development.json)
  - [`src/Client/AI.Boilerplate.Client.Windows/appsettings.Production.json`](/src/Client/AI.Boilerplate.Client.Windows/appsettings.Production.json)

### 服务器配置
- **API 服务器**: 
  - [`src/Server/AI.Boilerplate.Server.Api/appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json)
  - [`src/Server/AI.Boilerplate.Server.Api/appsettings.Development.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.Development.json)
  - [`src/Server/AI.Boilerplate.Server.Api/appsettings.Production.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.Production.json)
- **Web 服务器**:
  - [`src/Server/AI.Boilerplate.Server.Web/appsettings.json`](/src/Server/AI.Boilerplate.Server.Web/appsettings.json)
  - [`src/Server/AI.Boilerplate.Server.Web/appsettings.Development.json`](/src/Server/AI.Boilerplate.Server.Web/appsettings.Development.json)
  - [`src/Server/AI.Boilerplate.Server.Web/appsettings.Production.json`](/src/Server/AI.Boilerplate.Server.Web/appsettings.Production.json)
- **AppHost**: 
  - [`src/Server/AI.Boilerplate.Server.AppHost/appsettings.json`](/src/Server/AI.Boilerplate.Server.AppHost/appsettings.json)
  - [`src/Server/AI.Boilerplate.Server.AppHost/appsettings.Development.json`](/src/Server/AI.Boilerplate.Server.AppHost/appsettings.Development.json)

## 配置优先级层级

配置系统按特定顺序加载设置，**后面的源会覆盖前面的源**。这在两个位置实现：

- Server.Web, Client.Web, Client.Windows & Client.Maui: [`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Extensions/IConfigurationBuilderExtensions.cs`](/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Extensions/IConfigurationBuilderExtensions.cs)
- Server.Api: [`src/Shared/Infrastructure/Extensions/IConfigurationBuilderExtensions.cs`](/src/Shared/Infrastructure/Extensions/IConfigurationBuilderExtensions.cs)

### 配置优先级（从低到高）

```
优先级顺序（每一层都可以覆盖前一层）：
┌─────────────────────────────────────────────────────────────┐
│ 1. Shared/appsettings.json                                  │ ← 最低优先级
├─────────────────────────────────────────────────────────────┤
│ 2. Shared/appsettings.{environment}.json                    │
├─────────────────────────────────────────────────────────────┤
│ 3. Client/Core/appsettings.json                             │
├─────────────────────────────────────────────────────────────┤
│ 4. Client/Core/appsettings.{environment}.json               │
├─────────────────────────────────────────────────────────────┤
│ 5. 特定于平台的配置:                                         │
│    ┌─────────────────────────────────────────────────────┐  │
│    │ 服务器 (Blazor Server/SSR + API):                   │  │
│    │   • Server/appsettings.json                         │  │
│    │   • Server/appsettings.{environment}.json           │  │
│    │   • ASP.NET Core 默认配置源                         │  │
│    │     (环境变量、命令行等)                            │  │
│    └─────────────────────────────────────────────────────┘  │
│    ┌─────────────────────────────────────────────────────┐  │
│    │ Blazor WebAssembly:                                 │  │
│    │   • Client.Web/appsettings.json                     │  │
│    │   • Client.Web/appsettings.{environment}.json       │  │
│    │   • Client.Web/wwwroot/appsettings.json             │  │
│    │   • Client.Web/wwwroot/appsettings.{environment}.json │
│    └─────────────────────────────────────────────────────┘  │
│    ┌─────────────────────────────────────────────────────┐  │
│    │ .NET MAUI:                                          │  │
│    │   • Client.Maui/appsettings.json                    │  │
│    │   • Client.Maui/appsettings.{environment}.json      │  │
│    └─────────────────────────────────────────────────────┘  │
│    ┌─────────────────────────────────────────────────────┐  │
│    │ Windows:                                            │  │
│    │   • Client.Windows/appsettings.json                 │  │
│    │   • Client.Windows/appsettings.{environment}.json   │  │
│    └─────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘ ← 最高优先级
```

### 优先级工作原理 - 实际示例

假设您有一个 `ServerAddress` 设置：

1. **Shared/appsettings.json** 定义：
   ```json
   {
     "ServerAddress": "https://api.production.com/"
   }
   ```

2. **Shared/appsettings.Development.json** 为所有平台的开发环境覆盖它：
   ```json
   {
     "ServerAddress": "http://localhost:5000/"
   }
   ```

3. **Client.Maui/appsettings.Development.json** 可以专门为 MAUI 的开发环境覆盖它：
   ```json
   {
     "ServerAddress": "http://10.0.2.2:5000/"
   }
   ```

**结果**: 
- 在所有平台的**生产环境**中：使用 `https://api.production.com/`
- 在 Web/Windows 的**开发环境**中：使用 `http://localhost:5000/`
- 在 MAUI 的**开发环境**中：使用 `http://10.0.2.2:5000/` (Android 模拟器的 localhost)

## 项目中的真实配置示例

### 示例 1：日志记录的共享配置

在 [`src/Shared/appsettings.json`](/src/Shared/appsettings.json) 中，您可以看到适用于所有平台的日志记录配置：

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
        "ApplicationInsights": {
            "LogLevel": {
                "Default": "Warning",
                "Microsoft.Hosting.Lifetime": "Information"
            }
        },
        "Sentry": {
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

此配置会自动被所有平台（Web, MAUI, Windows, Server）继承。

### 示例 2：客户端核心配置

在 [`src/Client/AI.Boilerplate.Client.Core/appsettings.json`](/src/Client/AI.Boilerplate.Client.Core/appsettings.json) 中，定义了特定于客户端的设置：

```json
{
    "ServerAddress": "http://localhost:5000/",
    "ServerAddress_Comment": "如果您正在运行 AI.Boilerplate.Server.Web 项目，那么对于 Blazor Server 和 WebAssembly，您也可以使用相对 URL，例如 /",
    "GoogleRecaptchaSiteKey": "6LdMKr4pAAAAAKMyuEPn3IHNf04EtULXA8uTIVRw"
}
```

这些设置适用于所有客户端平台（Web, MAUI, Windows），但可以在特定于平台的文件中被覆盖。

### 示例 3：特定于平台的配置 (MAUI)

在 [`src/Client/AI.Boilerplate.Client.Maui/appsettings.json`](/src/Client/AI.Boilerplate.Client.Maui/appsettings.json) 中：

```json
{
    "WebAppUrl": null,
    "WebAppUrl_Comment": "当 MAUI 应用向 API 服务器发送请求，且 API 服务器和 Web 应用托管在不同的 URL 上时，生成的链接（例如电子邮件确认链接）的来源将取决于 `WebAppUrl` 的值。"
}
```

此设置仅存在于 MAUI 项目中，因为它是特定于移动应用场景的。

### 示例 4：服务器 API 配置

在 [`src/Server/AI.Boilerplate.Server.Api/appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json) 中，您会发现全面的服务器设置：

```json
{
    "ConnectionStrings": {
        "mssqldb": "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=AI.BoilerplateDb;...",
        "s3": "Endpoint=http://localhost:9000;AccessKey=minioadmin;SecretKey=minioadmin;",
        "smtp": "Endpoint=smtp://smtp.ethereal.email:587;UserName=..."
    },
    "AI": {
        "ChatOptions": {
            "Temperature": 0
        },
        "OpenAI": {
            "ChatModel": "gpt-4.1-mini",
            "ChatApiKey": null,
            "ChatEndpoint": "https://models.inference.ai.azure.com"
        }
    },
    "Identity": {
        "JwtIssuerSigningKeySecret": "VeryLongJWTIssuerSigningKeySecret...",
        "Issuer": "AI.Boilerplate",
        "Audience": "AI.Boilerplate",
        "BearerTokenExpiration": "0.00:05:00",
        "RefreshTokenExpiration": "14.00:00:00"
    },
    "Email": {
        "DefaultFromEmail": "DoNotReply@bitplatform.dev"
    },
    "GoogleRecaptchaSecretKey": "6LdMKr4pAAAAANvngWNam_nlHzEDJ2t6SfV6L_DS"
}
```

## `__Comment` 模式

由于 JSON 原生不支持注释，本项目使用了一种特殊模式：以 `__Comment` 或 `_Comment` 结尾的属性。

### 工作原理

当您想在配置文件中添加解释性注释时，请在设置名称后附加 `__Comment` 或 `_Comment`：

```json
{
    "ServerAddress": "http://localhost:5000/",
    "ServerAddress_Comment": "如果您正在运行 AI.Boilerplate.Server.Web 项目，那么对于 Blazor Server 和 WebAssembly，您也可以使用相对 URL，例如 /",
    
    "BearerTokenExpiration": "0.00:05:00",
    "BearerTokenExpiration_Comment": "格式：D.HH:mm:ss",
    
    "MaxPrivilegedSessionsCount": 3,
    "MaxPrivilegedSessionsCount_Comment": "是用户可以拥有的最大并发特权会话数。",
    
    "ConnectionStrings": {
        "Aspire__Comment": "运行 AI.Boilerplate.Server.AppHost 会在运行时 `覆盖` 以下连接字符串。",
        "mssqldb": "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=AI.BoilerplateDb;...",
        "smtp": "Endpoint=smtp://smtp.ethereal.email:587;...",
        "smtp_Comment": "您也可以使用 https://ethereal.email/create 进行测试。"
    }
}
```

**来自 `src/Server/AI.Boilerplate.Server.AppHost/appsettings.Development.json`:**
```json
{
    "Parameters": {
        "sqlserver__Comment": "用户名默认为 `sa`",
        "s3__Comment": "用户名默认为 `minioadmin`"
    }
}
```

**来自 `src/Client/AI.Boilerplate.Client.Core/appsettings.json`:**
```json
{
    "AdUnitPath__Comment": "来自 Google Ad Manager 面板的 Google 广告的廣告單元路徑。"
}
```

**为什么使用这种模式？**
- JSON 不允许传统的 `//` 或 `/* */` 注释
- 注释属性会被配置系统忽略（它们永远不会绑定到设置类）
- 为开发人员提供文件内文档
- 与 JSON 模式验证和工具兼容
- 易于理解配置值的用途和格式

## 实际配置场景

### 场景 1：添加新的共享设置

如果您想添加一个适用于所有平台的设置：

1. 将其添加到 [`src/Shared/appsettings.json`](/src/Shared/appsettings.json)：
   ```json
   {
     "MyNewFeature": {
       "EnabledByDefault": true,
       "EnabledByDefault_Comment": "控制是否对所有平台启用新功能"
     }
   }
   ```

2. 此设置将自动在以下位置可用：
   - 所有客户端平台（Web, MAUI, Windows）
   - 服务器项目（API, Web）

3. 如有需要，为特定环境或平台覆盖它

### 场景 2：特定于平台的覆盖

如果您需要为不同平台使用不同的值：

1. 在 [`src/Shared/appsettings.json`](/src/Shared/appsettings.json) 中定义基础值
2. 在特定于平台的文件中覆盖：
   - 对于 MAUI：[`src/Client/AI.Boilerplate.Client.Maui/appsettings.json`](/src/Client/AI.Boilerplate.Client.Maui/appsettings.json)
   - 对于 Web：[`src/Client/AI.Boilerplate.Client.Web/appsettings.json`](/src/Client/AI.Boilerplate.Client.Web/appsettings.json)
   - 对于 Windows：[`src/Client/AI.Boilerplate.Client.Windows/appsettings.json`](/src/Client/AI.Boilerplate.Client.Windows/appsettings.json)

### 场景 3：特定于环境的设置

为了在开发与生产环境中使用不同的值：

1. 在 `appsettings.json` 中定义生产值
2. 在 `appsettings.Development.json` 中为本地开发覆盖
3. 如果生产环境需要显式覆盖，使用 `appsettings.Production.json`

来自 [`src/Shared/appsettings.Development.json`](/src/Shared/appsettings.Development.json) 的示例：
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore*": "Warning",
            "System.Net.Http.HttpClient.*.LogicalHandler": "Warning"
        }
    }
}
```

这增加了开发环境中的日志详细程度（从 "Warning" 到 "Information"），而不影响生产环境。

## 在代码中访问配置

### 在控制器和服务中

配置通过 `IConfiguration` 自动注入：

```csharp
public class MyController : AppControllerBase
{
    [AutoInject] private IConfiguration configuration = default!;
    
    public IActionResult GetSettings()
    {
        var serverAddress = configuration["ServerAddress"];
        var timeout = configuration.GetValue<int>("ApiTimeout");
        
        return Ok(new { serverAddress, timeout });
    }
}
```

### 使用强类型设置类

本项目使用设置类（例如 `ServerApiSettings`, `ClientCoreSettings`），它们会自动绑定到配置部分：

```csharp
public partial class ServerApiSettings : ServerSharedSettings
{
    public IdentitySettings Identity { get; set; } = default!;
    public EmailSettings Email { get; set; } = default!;
    public SmsSettings Sms { get; set; } = default!;
    // ... 更多设置 ...
}
```

这些在服务配置中注册：

```csharp
services.AddOptions<ServerApiSettings>()
    .Bind(configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

然后在需要的地方注入：

```csharp
public class EmailService
{
    [AutoInject] private IOptionsSnapshot<ServerApiSettings> settings = default!;
    
    public async Task SendEmailAsync()
    {
        var fromEmail = settings.Value.Email.DefaultFromEmail;
        // 使用配置...
    }
}
```

## 配置与环境

本项目默认支持四种环境：
- **Development** - 本地开发（使用 `appsettings.Development.json`）
- **Test** - 测试环境（使用 `appsettings.Test.json`）
- **Staging** - 预生产测试环境
- **Production** - 线上生产环境（使用 `appsettings.Production.json`）

环境由 `AppEnvironment.Current` 确定，它在构建时根据 `-p:Environment` msbuild 开关进行设置。

请参阅 [`Directory.Build.props`](/src/Directory.Build.props) 了解环境配置，以及 [`src/Shared/Infrastructure/Services/AppEnvironment.cs`](/src/Shared/Infrastructure/Services/AppEnvironment.cs) 了解环境服务。
# 第七阶段：ASP.NET Core Identity - 认证与授权

欢迎参加 AI.Boilerplate 项目教程的**第七阶段**！在本阶段，您将深入探索项目中构建的全面**认证和授权系统**。这套生产就绪的身份系统包括基于 JWT 的认证、角色与权限管理、会话处理、双因素认证（2FA）以及更多功能。

---

## 目录

1. [理解认证方法](#understanding-authentication-methods)
   - [两种基本方法](#the-two-fundamental-methods)
   - [双因素认证 (2FA)](#two-factor-authentication-2fa)
   - [其他登录方法均基于 OTP](#other-sign-in-methods-are-built-on-otp)
2. [认证架构](#authentication-architecture)
   - [基于 JWT 令牌的认证](#jwt-token-based-authentication)
   - [会话管理](#session-management)
   - [外部身份支持](#external-identity-support)
3. [授权与访问控制](#authorization-and-access-control)
   - [基于角色和基于权限的授权](#role-based-and-permission-based-authorization)
   - [基于策略的授权](#policy-based-authorization)
   - [自定义声明类型](#custom-claim-types)
4. [Identity 配置](#identity-configuration)
5. [安全最佳实践](#security-best-practices)
6. [一次性令牌系统](#one-time-token-system)
7. [高级主题](#advanced-topics)
   - [使用 PFX 证书签署 JWT 令牌](#jwt-token-signing-with-pfx-certificates)
   - [Keycloak 集成](#keycloak-integration)
8. [动手探索](#hands-on-exploration)
9. [视频教程](#video-tutorial)

**重要提示**：所有关于 WebAuthn、Passkeys（通行密钥）和无密码认证的主题均在 [第 24 阶段](/.docs/24-%20WebAuthn%20and%20Passwordless%20Authentication%20(Advanced).md) 中解释。

---

## 理解认证方法

在深入技术架构之前，重要的是要理解 AI.Boilerplate 项目从根本上只支持**两种认证方法**。所有其他登录选项都是建立在这两种核心方法之上的。

### 两种基本方法

**1. 标识符 + 密码**
- **标识符**：用户名、电子邮件或电话号码
- **密码**：用户的秘密密码
- 这是传统的认证方法

**2. 标识符 + OTP (一次性密码)**
- **标识符**：用户名、电子邮件或电话号码
- **OTP**：发送给用户的一个 6 位数字代码
- 当通过电子邮件交付时，也称为“魔术链接”认证

> **关于用户名字段的说明**：在默认 UI 中，用户名字段被注释掉以保持界面简洁。如果您的业务需要基于用户名的认证，您可以轻松地在登录/注册组件中重新启用它。

### 双因素认证 (2FA)

根据用户设置，在初始登录后可能会触发**双因素认证**。

### 其他登录方法均基于 OTP

AI.Boilerplate 中的所有替代认证方法最终都会生成一个 6 位数的 OTP 代码，并在后台执行**自动 OTP 登录**：

| 方法 | 工作原理 |
|--------|-------------|
| **外部提供商** (Google, Facebook, GitHub 等) | OAuth 流程成功后，生成 OTP 并自动登录 |
| **WebAuthn / Passkeys** (指纹、Face ID) | 生物识别验证后，生成 OTP 并自动登录 |
| **魔术链接** (电子邮件链接) | 点击链接会触发带有嵌入令牌的 OTP 登录 |

这种统一的方法意味着：
- ✅ 无论用户如何登录，**始终尊重 2FA**
- ✅ 所有登录方法的**会话管理工作方式相同**
- ✅ 通过单一认证管道**简化代码库**

---

## 认证架构

### 基于 JWT 令牌的认证

本项目使用 **JWT (JSON Web Tokens)** 进行无状态认证。其工作原理如下：

#### 关键特征

- **访问令牌 (Access Tokens)**：短寿命令牌（默认：**5 分钟**），通过 `Authorization: Bearer <token>` 头包含在每个 API 请求中。
- **刷新令牌 (Refresh Tokens)**：长寿命令牌（默认：**14 天**），安全地存储在客户端，用于获取新的访问令牌，而无需用户再次登录。
- **令牌生成**：使用 ASP.NET Core Identity 内置的Bearer令牌认证，采用 **HS512 算法**。
- **安全性**：令牌使用 [`appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json) 中配置的密钥进行签名。

#### 令牌配置

您可以在 [`appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json) 中配置令牌过期时间：

```json
"Identity": {
    "BearerTokenExpiration": "0.00:05:00",  // 格式：D.HH:mm:ss (5分钟)
    "RefreshTokenExpiration": "14.00:00:00", // 14天
    "JwtIssuerSigningKeySecret": "VeryLongJWTIssuerSigningKeySecretThatIsMoreThan64BytesToEnsureCompatibilityWithHS512Algorithm"
}
```

---

### 会话管理

与传统的会话 Cookie 不同，本项目在使用 JWT 令牌的同时，实现了数据库中的**服务器端会话跟踪**。

#### UserSession 实体

用户会话通过 [`UserSession`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/Models/UserSession.cs) 实体持久化在数据库中：

```csharp
public partial class UserSession
{
    public Guid Id { get; set; }
    public string? IP { get; set; }
    public string? Address { get; set; }
    public bool Privileged { get; set; }
    public long StartedOn { get; set; }       // Unix 时间戳
    public long? RenewedOn { get; set; }      // Unix 时间戳
    public Guid UserId { get; set; }
    public string? SignalRConnectionId { get; set; }
    public string? DeviceInfo { get; set; }
    public AppPlatformType? PlatformType { get; set; }
    public string? CultureName { get; set; }
    public string? AppVersion { get; set; }
    // ... 其他属性
}
```

#### 关键功能

- **每个设备一个会话**：每个活跃的设备/浏览器都有自己的一条 `UserSession` 记录。
- **详细跟踪**：会话跟踪 IP 地址、设备信息、平台类型、应用版本和文化设置。
- **实时通信**：会话包含用于推送通知的 SignalR 连接 ID。
- **用户控制**：用户可以从“设置”页面查看和管理所有活跃会话。

#### 令牌中的会话 ID

会话 ID 作为声明 (`AppClaimTypes.SESSION_ID`) 嵌入到访问令牌和刷新令牌中。这使得：

- **会话撤销**：当用户撤销会话时，其关联的刷新令牌将失效。
- **强制重新认证**：已删除的会话要求用户在该设备上重新登录。

#### 会话创建示例

来自 [`IdentityController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.cs)：

```csharp
private async Task<UserSession> CreateUserSession(Guid userId, CancellationToken cancellationToken)
{
    var userSession = new UserSession
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        StartedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
        // 依赖 Cloudflare CDN 获取位置
        Address = $"{Request.Headers["cf-ipcountry"]}, {Request.Headers["cf-ipcity"]}"
    };

    await UpdateUserSessionPrivilegeStatus(userSession, cancellationToken);
    return userSession;
}
```

---

### 外部身份支持

本项目支持**外部认证提供商**，用于单点登录 (SSO) 和社交登录。

#### 支持的提供商

以下外部身份提供商已预先配置：

- **Google**
- **Microsoft/Azure Entra/Azure AD B2C**
- **Twitter (X)**
- **Apple**
- **GitHub**
- **Facebook**
- **Keycloak** (免费、开源的身份和访问管理)

#### 配置

外部提供商设置在 [`appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json) 的 `Authentication` 部分下配置：

```json
"Authentication": {
    "Google": {
        "ClientId": null,
        "ClientSecret": null
    },
    "GitHub": {
        "ClientId": null,
        "ClientSecret": null
    },
    "AzureAD": {
        "Instance": "https://login.microsoftonline.com/",
        "TenantId": null,
        "ClientId": null,
        "ClientSecret": null,
        "CallbackPath": "/signin-oidc"
    }
    // ... 其他提供商
}
```

#### 外部登录流程

来自 [`IdentityController.ExternalSignIn.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.ExternalSignIn.cs)：

```csharp
[HttpGet]
public async Task<ActionResult> ExternalSignIn(string provider, string? returnUrl = null, 
    int? localHttpPort = null, [FromQuery] string? origin = null)
{
    var redirectUrl = Url.Action(nameof(ExternalSignInCallback), "Identity", 
        new { returnUrl, localHttpPort, origin });
    var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    return new ChallengeResult(provider, properties);
}
```

当外部登录成功时，系统会：
1. 从外部提供商检索用户信息
2. 查找现有用户或创建新用户
3. 如果提供商提供了电子邮件/电话，则自动确认
4. 生成用于自动登录的魔术链接（如果启用了 2FA，则支持 2FA）

---

## 授权与访问控制

### 基于角色和基于权限的授权

本项目实现了一种结合角色和权限（声明）的**混合授权模型**。

#### 用户和角色

- 基于 **ASP.NET Core Identity** 内置的 `User` 和 `Role` 实体
- 用户可以被分配**多个角色**
- 角色通过管理面板中的**角色页面**进行管理
- 用户通过管理面板中的**用户页面**进行管理

#### 权限 (声明)

使用**声明**的细粒度权限系统：

- **用户声明**：直接分配给单个用户的权限
- **角色声明**：分配给角色的权限（该角色下的所有用户继承）
- 声明被添加到 **JWT 令牌负载**中，以便进行高效的授权检查（无需数据库查找）

---

### 基于策略的授权

本项目定义了可在整个应用程序中使用的**授权策略**。

#### 策略配置

策略在 [`ISharedServiceCollectionExtensions.cs`](/src/Shared/Infrastructure/Extensions/ISharedServiceCollectionExtensions.cs) 中定义：

```csharp
public static void ConfigureAuthorizationCore(this IServiceCollection services)
{
    services.AddAuthorizationCore(options =>
    {
        // 内置策略
        options.AddPolicy(AuthPolicies.TFA_ENABLED, 
            x => x.RequireClaim("amr", "mfa"));
        options.AddPolicy(AuthPolicies.PRIVILEGED_ACCESS, 
            x => x.RequireClaim(AppClaimTypes.PRIVILEGED_SESSION, "true"));
        options.AddPolicy(AuthPolicies.ELEVATED_ACCESS, 
            x => x.RequireClaim(AppClaimTypes.ELEVATED_SESSION, "true"));

        // 基于功能的策略（根据 `AppFeatures` 自动生成）
        foreach (var feat in AppFeatures.GetAll())
        {
            options.AddPolicy(feat.Value, policy => 
                policy.AddRequirements(new AppFeatureRequirement(
                    FeatureName: $"{feat.Group.Name}.{feat.Name}", 
                    FeatureValue: feat.Value)));
        }
    });
}
```

#### 内置授权策略

定义在 [`AuthPolicies.cs`](/src/Shared/Infrastructure/Services/AuthPolicies.cs) 中：

**1. TFA_ENABLED**
```csharp
public const string TFA_ENABLED = nameof(TFA_ENABLED);
```
- 要求用户**已启用双因素认证**
- 适用于需要增强安全性的页面

**2. PRIVILEGED_ACCESS**
```csharp
/// <summary>
/// 拥有此策略/声明意味着用户允许访问需要特权访问的页面。
/// 目前，此策略仅适用于 Todo 和管理面板特定页面（如仪表板）。
/// 但可以根据需要扩展以覆盖更多页面。
/// 
/// 默认情况下，每个用户限制为 3 个活跃会话。
/// 用户的最大特权会话值存储在 <see cref="AppClaimTypes.MAX_PRIVILEGED_SESSIONS"/> 声明中。
/// 
/// 重要：不要将此策略应用于设置页面，因为用户需要在那里管理和撤销他们的会话。
/// </summary>
public const string PRIVILEGED_ACCESS = nameof(PRIVILEGED_ACCESS);
```
- **限制每个用户的并发会话**（默认：**3 个会话**）
- 通过限制活跃会话防止资源滥用
- 应用于高价值页面，如仪表板、产品、类别
- 用户可以从“设置”页面管理会话

**3. ELEVATED_ACCESS**
```csharp
/// <summary>
/// 允许用户执行潜在有害的操作，如删除账户。
/// 此限时策略在通过安全的 6 位代码成功验证后激活，
/// 或在启用 2FA 的用户登录会话的最初几分钟内激活。
/// </summary>
public const string ELEVATED_ACCESS = nameof(ELEVATED_ACCESS);
```
- 敏感操作需要**最近重新认证**
- 用于危险操作，如**删除账户**
- 通过输入 6 位代码或在启用 2FA 的情况下初始登录来激活
- **限时**提升（短时间后过期）

#### 特权会话逻辑

来自 [`IdentityController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.cs)：

```csharp
private async Task UpdateUserSessionPrivilegeStatus(UserSession userSession, 
    CancellationToken cancellationToken)
{
    var userId = userSession.UserId;

    // 检查用户是否有自定义的最大会话限制声明
    var maxPrivilegedSessionsClaimValues = await userClaimsService
        .GetClaimValues<int?>(userId, AppClaimTypes.MAX_PRIVILEGED_SESSIONS, cancellationToken);

    // -1 表示无限会话
    var hasUnlimitedPrivilegedSessions = maxPrivilegedSessionsClaimValues.Any(v => v == -1);

    var maxPrivilegedSessionsCount = hasUnlimitedPrivilegedSessions 
        ? -1 
        : maxPrivilegedSessionsClaimValues.Max() ?? AppSettings.Identity.MaxPrivilegedSessionsCount;

    // 确定此会话是否为特权会话
    var isPrivileged = hasUnlimitedPrivilegedSessions ||
        userSession.Privileged is true || // 一旦是特权，保持特权
        await DbContext.UserSessions.CountAsync(
            us => us.UserId == userSession.UserId && us.Privileged == true, 
            cancellationToken) < maxPrivilegedSessionsCount;

    // 添加声明到令牌
    userClaimsPrincipalFactory.SessionClaims.Add(
        new(AppClaimTypes.PRIVILEGED_SESSION, isPrivileged ? "true" : "false"));
    userClaimsPrincipalFactory.SessionClaims.Add(
        new(AppClaimTypes.MAX_PRIVILEGED_SESSIONS, 
            maxPrivilegedSessionsCount.ToString(CultureInfo.InvariantCulture)));

    userSession.Privileged = isPrivileged;
}
```

#### 基于功能的策略

定义在 [`AppFeatures.cs`](/src/Shared/Infrastructure/Services/AppFeatures.cs) 中：

```csharp
public class AppFeatures
{
    public class Management
    {
        public const string ManageAiPrompt = "1.0";
        public const string ManageRoles = "1.1";
        public const string ManageUsers = "1.2";
    }

    public class System
    {
        public const string ManageLogs = "2.0";
        public const string ManageJobs = "2.1";
    }

    public class AdminPanel
    {
        public const string Dashboard = "3.0";
        public const string ManageProductCatalog = "3.1";
    }

    public class Todo
    {
        public const string ManageTodo = "4.0";
    }
}
```

本项目会为 `AppFeatures` 中定义的所有功能**自动创建策略**。每个功能值（例如 `"1.0"`）成为一个策略名称。

功能值较短的原因是它们存储在 JWT 令牌中，为了保持 JWT 令牌负载较小，因此分配了这些简短且唯一的值。

#### 策略使用示例

来自项目中的实际页面：

**TodoPage.razor**:
```csharp
@attribute [Authorize(Policy = AuthPolicies.PRIVILEGED_ACCESS)]
@attribute [Authorize(Policy = AppFeatures.Todo.ManageTodo)]
```

**UsersPage.razor**:
```csharp
@attribute [Authorize(Policy = AuthPolicies.PRIVILEGED_ACCESS)]
@attribute [Authorize(Policy = AppFeatures.Management.ManageUsers)]
```

**DashboardPage.razor**:
```csharp
@attribute [Authorize(Policy = AuthPolicies.PRIVILEGED_ACCESS)]
@attribute [Authorize(Policy = AppFeatures.AdminPanel.Dashboard)]
```

**重要**：当使用多个 `[Authorize]` 属性时，用户必须满足**所有策略**才能访问页面。

---

### 自定义声明类型

#### 系统声明类型

定义在 [`AppClaimTypes.cs`](/src/Shared/Infrastructure/Services/AppClaimTypes.cs) 中：

```csharp
/// <summary>
/// 这些声明可能不会被添加到 RoleClaims/UserClaims 表中。
/// 系统本身将根据 AuthPolicies 将这些声明分配给用户。
/// </summary>
public class AppClaimTypes
{
    public const string SESSION_ID = "s-id";
    public const string PRIVILEGED_SESSION = "p-s";
    public const string MAX_PRIVILEGED_SESSIONS = "mx-p-s";
    public const string ELEVATED_SESSION = "e-s";
    public const string FEATURES = "features";
}
```

**重要**：这些声明由**系统自动添加**，**不应**手动添加到数据库：

- **`SESSION_ID`**：当前用户会话的唯一标识符 => 存储在 UserSessions 表中的 Guid 值
- **`MAX_PRIVILEGED_SESSIONS`**：该用户允许的最大特权会话数 => -1 (无限) 或正数
- **`PRIVILEGED_SESSION`**：指示此会话是否为特权会话 => "true" 或 "false"
- **`ELEVATED_SESSION`**：指示用户最近已为敏感操作进行认证 => "true" 或 "false"
- **`FEATURES`**：包含授予用户的功能/权限值列表 => AppFeature 值的数组，例如 ["1.1", "2.1"]

---

## Identity 配置

### appsettings.json 中的 IdentitySettings

**位置**: [`/src/Server/AI.Boilerplate.Server.Api/appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json)

**配置选项**:

```json
"Identity": {
    "JwtIssuerSigningKeySecret": "VeryLongJWTIssuerSigningKeySecretThatIsMoreThan64BytesToEnsureCompatibilityWithHS512Algorithm",
    "Issuer": "AI.Boilerplate",
    "Audience": "AI.Boilerplate",
    "BearerTokenExpiration": "0.00:05:00",
    "RefreshTokenExpiration": "14.00:00:00",
    "EmailTokenLifetime": "0.00:02:00",
    "PhoneNumberTokenLifetime": "0.00:02:00",
    "ResetPasswordTokenLifetime": "0.00:02:00",
    "TwoFactorTokenLifetime": "0.00:02:00",
    "OtpTokenLifetime": "0.00:02:00",
    "MaxPrivilegedSessionsCount": 3,
    "Password": {
        "RequireDigit": "false",
        "RequiredLength": "6",
        "RequireNonAlphanumeric": "false",
        "RequireUppercase": "false",
        "RequireLowercase": "false"
    },
    "SignIn": {
        "RequireConfirmedAccount": true
    }
}
```

#### 关键设置说明

- **BearerTokenExpiration**: 访问令牌的有效期（默认：5 分钟）
- **RefreshTokenExpiration**: 刷新令牌的有效期（默认：14 天）
- **EmailTokenLifetime**: 电子邮件确认令牌的有效期（默认：2 分钟）
- **TwoFactorTokenLifetime**: 2FA 代码的有效期（默认：2 分钟）
- **MaxPrivilegedSessionsCount**: 每个用户的特权会话默认限制
- **Password Requirements**: 配置密码复杂度规则
- **RequireConfirmedAccount**: 登录是否需要电子邮件/电话确认

---

## 安全最佳实践

身份系统遵循**行业标准的安全最佳实践**：

### 1. 密码哈希
- 使用 **PBKDF2 with HMAC-SHA512**
- **100,000 次迭代**，提供强大的暴力破解防护

### 2. 并发戳 (Concurrency Stamps)
- **防止并发修改冲突**
- 每个实体都有一个 `ConcurrencyStamp`，每次更新时都会更改

### 3. 安全戳 (Security Stamps)
- **更改时使所有令牌失效**
- 在配置 2FA、更改密码等操作后自动更新

### 4. 账户锁定
- **防止暴力破解攻击**
- 具有递增锁定时间的指数退避

### 5. 一次性令牌
- **所有令牌只能使用一次**
- 令牌在配置的生命周期后过期
- 详见下一节的详细解释

### 6. 令牌过期
- **短寿命访问令牌**（5 分钟）最大限度地减少令牌被盗的风险窗口
- 刷新令牌允许无缝的用户体验，同时不牺牲安全性

### 7. Google reCAPTCHA
- **保护注册端点**免受机器人攻击
- 可在 appsettings.json 中配置

---

## 一次性令牌系统

本项目实现了一个**安全的一次性令牌系统**，具有自动过期和失效功能。

### 工作原理

#### 令牌请求跟踪

每种令牌类型在 [`User`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/Models/User.cs) 实体中都有相应的 `RequestedOn` 时间戳：

```csharp
public partial class User : IdentityUser<Guid>
{
    /// <summary>
    /// 上次令牌请求的日期和时间。
    /// 确保只有最新生成的令牌有效，且只能使用一次。
    /// </summary>
    public DateTimeOffset? EmailTokenRequestedOn { get; set; }
    public DateTimeOffset? PhoneNumberTokenRequestedOn { get; set; }
    public DateTimeOffset? ResetPasswordTokenRequestedOn { get; set; }
    public DateTimeOffset? TwoFactorTokenRequestedOn { get; set; }
    public DateTimeOffset? OtpRequestedOn { get; set; }
    public DateTimeOffset? ElevatedAccessTokenRequestedOn { get; set; }
}
```

#### 令牌生成

生成令牌时，`RequestedOn` 时间戳设置为**当前时间**并**嵌入到令牌用途字符串**中。

来自 [`IdentityController.EmailConfirmation.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.EmailConfirmation.cs)：

```csharp
private async Task SendConfirmEmailToken(User user, string? returnUrl, 
    CancellationToken cancellationToken)
{
    // 设置请求时间戳
    user.EmailTokenRequestedOn = DateTimeOffset.Now;
    var result = await userManager.UpdateAsync(user);

    if (result.Succeeded is false)
        throw new ResourceValidationException(result.Errors
            .Select(e => new LocalizedString(e.Code, e.Description)).ToArray())
            .WithData("UserId", user.Id);

    var email = user.Email!;
    
    // 生成带有嵌入时间戳的令牌
    var token = await userManager.GenerateUserTokenAsync(
        user, 
        TokenOptions.DefaultPhoneProvider, 
        FormattableString.Invariant($"VerifyEmail:{email},{user.EmailTokenRequestedOn?.ToUniversalTime()}")
    );
    
    var link = new Uri(HttpContext.Request.GetWebAppUrl(), 
        $"{PageUrls.Confirm}?email={Uri.EscapeDataString(email)}&emailToken={Uri.EscapeDataString(token)}&culture={CultureInfo.CurrentUICulture.Name}");

    await emailService.SendEmailToken(user, email, token, link, cancellationToken);
}
```

#### 令牌验证

验证令牌时，系统会检查：

1. **过期**：自 `RequestedOn` 以来，令牌生命周期是否已过？
2. **一次性使用**：令牌是否与**最新**的 `RequestedOn` 时间戳匹配？
3. **失效**：`RequestedOn` 时间戳是否设置为 `null`（已失效）？

来自 [`IdentityController.EmailConfirmation.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.EmailConfirmation.cs)：

```csharp
[HttpPost, Produces<TokenResponseDto>()]
public async Task ConfirmEmail(ConfirmEmailRequestDto request, CancellationToken cancellationToken)
{
    var user = await userManager.FindByEmailAsync(request.Email!)
        ?? throw new BadRequestException(Localizer[nameof(AppStrings.UserNotFound)])
            .WithData("Email", request.Email);

    // 检查令牌是否过期
    var expired = (DateTimeOffset.Now - user.EmailTokenRequestedOn) 
        > AppSettings.Identity.EmailTokenLifetime;

    if (expired)
        throw new BadRequestException(nameof(AppStrings.ExpiredToken))
            .WithData("UserId", user.Id);

    // 验证带有嵌入时间戳的令牌
    var tokenIsValid = await userManager.VerifyUserTokenAsync(
        user, 
        TokenOptions.DefaultPhoneProvider, 
        FormattableString.Invariant($"VerifyEmail:{request.Email},{user.EmailTokenRequestedOn?.ToUniversalTime()}"), 
        request.Token!
    );

    if (tokenIsValid is false)
    {
        await userManager.AccessFailedAsync(user);
        throw new BadRequestException(nameof(AppStrings.InvalidToken))
            .WithData("UserId", user.Id);
    }

    // 确认电子邮件
    await userEmailStore.SetEmailConfirmedAsync(user, true, cancellationToken);
    await userManager.UpdateAsync(user);

    // 通过将 RequestedOn 设置为 null 来使令牌失效
    user.EmailTokenRequestedOn = null;
    var updateResult = await userManager.UpdateAsync(user);
    if (updateResult.Succeeded is false)
        throw new ResourceValidationException(updateResult.Errors
            .Select(e => new LocalizedString(e.Code, e.Description)).ToArray())
            .WithData("UserId", user.Id);

    // 确认后自动登录
    var token = await userManager.GenerateUserTokenAsync(user, 
        TokenOptions.DefaultPhoneProvider, 
        FormattableString.Invariant($"Otp_Email,{user.OtpRequestedOn?.ToUniversalTime()}"));

    await SignIn(new() { Email = request.Email, Otp = token }, cancellationToken);
}
```

### 主要优势

✅ 只有**最近请求的令牌**有效  
✅ 令牌在成功使用后**自动失效**  
✅ 令牌在配置的生命周期后**过期**  
✅ 如果用户请求新令牌，**所有以前的令牌都将失效**  
✅ 用户无法频繁请求令牌

### 示例场景

让我们 walkthrough 一个密码重置场景：

1. **上午 10:00**：用户请求密码重置
   - 生成令牌 A
   - `ResetPasswordTokenRequestedOn = 10:00`

2. **上午 10:05**：用户再次请求密码重置（也许他们没收到邮件）
   - 生成令牌 B
   - `ResetPasswordTokenRequestedOn = 10:05`

3. **令牌 A 现在无效**
   - 令牌 A 是在 10:00 生成的
   - 但当前的 `ResetPasswordTokenRequestedOn` 是 10:05
   - 令牌 A 验证将失败

4. **用户成功使用令牌 B**
   - 令牌 B 被验证
   - 密码被重置
   - `ResetPasswordTokenRequestedOn` 设置为 `null`

5. **令牌 B 再也不能使用**
   - 即使有人截获了令牌 B，它也已经失效

---

## 高级主题

### 使用 PFX 证书签署 JWT 令牌

默认情况下，Bit AI.Boilerplate 在 [`AppJwtSecureDataFormat`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/Services/AppJwtSecureDataFormat.cs) 类中使用基于字符串的密钥 (`JwtIssuerSigningKeySecret`) 来签署 JWT 令牌。虽然这种方法有效且安全，但在生产环境中，尤其是在以下情况下，使用 **PFX 证书**被视为最佳实践：

- 您需要在多个后端服务之间共享 JWT 验证
- 您希望遵循行业标准的加密实践
- 您部署到有严格安全要求的企业环境

**为什么我们默认不使用 PFX**

我们选择基于字符串的密钥作为默认值，因为：
- **部署更容易**：PFX 证书在共享托管提供商上需要额外的配置
- **开发更简单**：开发人员可以立即开始，无需证书管理
- **良好的安全性**：使用 HS512 的基于字符串的密钥在加密上仍然是安全的

**如何迁移到 PFX 证书**

如果您想使用 PFX 证书，您需要修改 [`AppJwtSecureDataFormat`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/Services/AppJwtSecureDataFormat.cs) 以使用 `AsymmetricSecurityKey` 而不是 `SymmetricSecurityKey`：

```csharp
// 代替：
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Identity.JwtIssuerSigningKeySecret))

// 使用：
var certificate = new X509Certificate2("path/to/certificate.pfx", "password");
IssuerSigningKey = new X509SecurityKey(certificate)
```

**保护 ASP.NET Core 数据保护密钥**

此外，您应该保护存储在数据库中的数据保护密钥。在 [`Program.Services.cs`](/src/Server/AI.Boilerplate.Server.Api/Program.Services.cs) 中，更新以下代码：

```csharp
services.AddDataProtection()
   .PersistKeysToDbContext<AppDbContext>()
   .ProtectKeysWithCertificate(certificate); // 添加此行
```

**跨服务 JWT 验证**

使用 PFX 证书时，您可以将**公钥**共享给其他后端服务，以验证由您的 ASP.NET Core Identity 系统颁发的 JWT。其他服务可以使用 `AddJwtAuthentication` 方法来验证令牌，而无需私钥。

这使得以下场景成为可能：
- 多个微服务验证同一个 JWT
- 第三方服务可以验证您的令牌

---

### Keycloak 集成

Bit AI.Boilerplate 内置支持 **Keycloak**，这是一个免费的开源身份和访问管理解决方案。Keycloak 提供企业级功能，如：

- 集中式用户管理
- 跨多个应用的单点登录 (SSO)
- 细粒度授权
- 用户联合 (LDAP, Active Directory)

#### .NET Aspire 中的 Keycloak

当您启用 .NET Aspire 运行项目时（默认配置），Keycloak 会自动作为容器化服务启动。这为开发和测试提供了一个完整的身份服务器，无需任何手动设置。

#### 演示用户账户

Keycloak 实例预配置了以下演示账户（由 [src\Server\AI.Boilerplate.Server.AppHost\Infrastructure\Realms\dev-realm.json](..\src\Server\AI.Boilerplate.Server.AppHost\Infrastructure\Realms\dev-realm.json) 提供）：

| 用户名 | 密码 | 角色 | 描述 |
|----------|----------|------|-------------|
| test | 123456 | Admin | 完全管理访问权限 |
| bob | bob | Demo | 标准演示用户 |
| alice | alice | Demo | 标准演示用户 |

#### Keycloak 映射工作原理

AI.Boilerplate 模板通过 [`AppUserClaimsPrincipalFactory`](/src/Server/AI.Boilerplate.Server.Api/Features/Identity/Services/AppUserClaimsPrincipalFactory.cs) 中的自定义映射系统将 Keycloak 与 ASP.NET Core Identity 集成：

**1. 组 → 角色**
- Keycloak **组**映射到 ASP.NET Core Identity **角色**
- 用户从其组成员身份继承所有角色

**2. 属性 → 声明**
- Keycloak 中的**用户属性**变为单独的声明
- **组属性**也映射到声明

**3. Keycloak 角色 → 自定义映射** (当前项目设置不需要)
- Keycloak 内置的**角色**（不同于组）**不会自动映射**
- 这些角色的结构与 ASP.NET Core Identity 角色不同

#### 实时声明同步

`AppUserClaimsPrincipalFactory` 在每次 ASP.NET Core Identity 令牌刷新时从 Keycloak 检索最新的声明：

**安全优势：**
- **自动停用**：如果用户在 Keycloak 中被禁用或删除，访问权限会立即撤销
- **新鲜声明**：每次令牌刷新都会从 Keycloak 获取最新权限
- **会话验证**：过期或被撤销的 Keycloak 会话会触发 `UnauthorizedException`

#### JWT 令牌颁发流程

尽管使用 Keycloak 进行认证，最终的 JWT 令牌仍由 **ASP.NET Core Identity** 颁发：

1. 用户通过 Keycloak 登录（通过 OpenID Connect）
2. `AppUserClaimsPrincipalFactory` 从 Keycloak 检索声明
3. ASP.NET Core Identity 将 Keycloak 声明与本地声明合并
4. 使用配置的密钥（或 PFX 证书）颁发并签署新的 JWT
5. JWT 发送给客户端，并用于所有后续 API 请求

**验证：**
- 当 JWT 发回后端时，**ASP.NET Core Identity 会验证它**

#### 与其他后端服务共享 JWT

如果您想与其他后端服务（例如微服务）共享 JWT，请按照以下步骤操作：

1. **切换到 PFX 证书**（如上一节所述）
2. **共享公钥**给其他服务
3. 其他服务使用 `AddJwtAuthentication` 来验证令牌

---

## 动手探索

**运行项目**：要充分了解身份功能，请运行项目并测试以下内容：

### 1. 注册
- 使用电子邮件或电话号码创建新账户
- 测试电子邮件/电话确认流程
- 尝试使用现有的电子邮件/电话注册（应失败）

### 2. 登录
- 测试**基于密码的认证**
- 测试 **OTP 认证**（魔术链接）
- 测试多次失败尝试后的**账户锁定**

### 3. 双因素认证 (2FA)
- 在设置中使用身份验证器应用启用 2FA
- 测试**两步登录**过程
- 尝试输入错误的 2FA 代码
- 测试**恢复代码**

### 4. 会话管理
- 从多个设备/浏览器登录
- 在**设置页面**查看所有活跃会话
- **撤销特定会话**并观察强制重新认证

### 5. 密码重置
- 测试**忘记密码**流程
- 通过电子邮件/电话请求重置令牌
- 尝试使用过期的令牌
- 尝试使用已使用的令牌

### 6. 外部提供商
- 尝试使用配置的演示提供商进行**外部登录**
- 观察外部提供商的电子邮件确认工作方式

### 7. 权限和策略
- 创建具有不同角色的用户
- 测试对具有 **PRIVILEGED_ACCESS** 策略页面的访问
- 测试基于功能的策略（例如 `ManageUsers`, `Dashboard`）

### 8. 特权会话限制
- 从超过 3 个设备登录
- 观察哪些会话变为“特权”
- 测试从非特权会话访问具有 `PRIVILEGED_ACCESS` 策略的页面

---

## 视频教程

**强烈推荐**：要亲眼看到所有这些功能并了解完整的身份系统，请观看这个**15 分钟的视频教程**：

### 📺 [观看：全面的身份系统演练](https://youtu.be/0ySKLPJm-fw)

**本视频演示了**：
- 用户注册和电子邮件/电话确认
- 基于密码和 OTP 的认证
- 双因素认证的设置和使用
- 会话管理和撤销
- 密码重置流程
- 角色和权限管理
- 外部提供商集成
- 特权会话限制
- 敏感操作的提升访问

---

### AI Wiki: 已回答的问题
* [刷新令牌在 AI.Boilerplate 项目模板中如何工作？](https://deepwiki.com/search/how-does-a-refresh-token-funct_6a75fa66-ab98-4367-bd1a-24b081fbf88c)
* [当我使用 [AuthorizedApi] 时会发生什么？](https://deepwiki.com/search/what-would-happen-when-i-use-a_c525d59d-5c55-489b-8f95-69f6df7c743d)
* [给我双因素认证设置和使用流程的高级概述](https://deepwiki.com/search/give-me-high-level-overview-of_1883503f-2e34-41ca-821a-1246d332990f)

在此处提出您自己的问题：[https://wiki.bitplatform.dev](https://wiki.bitplatform.dev)

---
# 第二十四阶段：WebAuthn 与无密码认证（高级）

欢迎回到第二十四阶段！在本阶段，您将了解 AI.Boilerplate 项目中先进的 WebAuthn 实现，该实现支持跨所有平台使用指纹、Face ID 和 PIN 码等生物识别方法进行安全的无密码认证。

---

## 🔐 什么是 WebAuthn？

**WebAuthn** (Web Authentication) 是一种现代 Web 标准，利用公钥加密技术实现无密码认证。AI.Boilerplate 项目实现了一套全面的 WebAuthn 解决方案，适用于：

- **Web 浏览器** (Chrome, Edge, Firefox, Safari)
- **Android 设备** (指纹、面部解锁、PIN 码)
- **iOS 设备** (Touch ID, Face ID, 通行码)
- **Windows** (Windows Hello - 指纹、面部识别、PIN 码)
- **macOS** (Touch ID, 密码)

> **注意**：由于平台限制，Android 设备目前不支持 Face ID。

---

## 🏗️ 架构概览

本项目中的 WebAuthn 实现由几个协同工作的关键组件组成：

### 高层组件架构

```
┌─────────────────────────────────────────────────────────────┐
│                    客户端组件                                 │
│  (SignInPanel, PasswordlessTab - 用户界面)                   │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────────┐
│              WebAuthnServiceBase (抽象类)                     │
│  - 管理本地存储中的用户 ID                                     │
│  - 定义平台实现的接口                                         │
└────────────────────┬────────────────────────────────────────┘
                     │
        ┌────────────┼────────────┬
        ▼            ▼            ▼
┌──────────┐  ┌──────────┐  ┌──────────┐
│   Web    │  │   MAUI   │  │ Windows  │
│ 服务     │  │ 服务     │  │ 服务     │
└────┬─────┘  └────┬─────┘  └────┬─────┘
     │             │              │
     │             │              │  (Blazor Hybrid 需要特殊处理)
     │             │              │
     │             ▼              ▼
     │    ┌─────────────────────────────────┐
     │    │   ILocalHttpServer               │
     │    │   - 启动本地 HTTP 服务器            │
     │    │   - 提供 web-interop-app.html     │
     │    └──────────┬──────────────────────┘
     │               │
     │               ▼
     │    ┌─────────────────────────────────┐
     │    │ IExternalNavigationService       │
     │    │ - 打开应用内浏览器                 │
     │    └──────────┬──────────────────────┘
     │               │
     │               ▼
     │    ┌─────────────────────────────────┐
     │    │  web-interop-app.html            │
     │    │  (包含 app.js 的轻量级页面)         │
     │    │  - 在 localhost 源上运行           │
     │    │  - 调用 WebAuthn API              │
     │    │  - 通过 HTTP 返回结果              │
     │    └──────────┬──────────────────────┘
     │               │
     └───────────────┴──────────────────────┐
                                             │
                                             ▼
                              ┌─────────────────────────────┐
                              │   服务器 API 控制器            │
                              │  - UserController.WebAuthn   │
                              │  - IdentityController.WebAuthn│
                              └──────────┬──────────────────┘
                                         │
                                         ▼
                              ┌─────────────────────────────┐
                              │   Fido2NetLib (服务器端)      │
                              │  - 验证挑战 (challenges)       │
                              │  - 验证证明 (attestations)     │
                              └──────────┬──────────────────┘
                                         │
                                         ▼
                              ┌─────────────────────────────┐
                              │   数据库                      │
                              │  - WebAuthnCredential 表      │
                              │  - 存储公钥                   │
                              └─────────────────────────────┘
```

---

## 🔄 WebAuthn 流程：逐步解析

### 注册流程（创建凭据）

当用户想要**启用无密码认证**时，会发生以下步骤：

**1. 用户发起注册**
   - 用户导航到 设置 → 账户 → 无密码 标签页
   - 点击“启用无密码登录”

**2. 客户端请求选项**
   ```
   客户端 → 服务器：GET /api/User/GetWebAuthnCredentialOptions
   ```

**3. 服务器生成挑战** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/UserController.WebAuthn.cs`)
   - 创建加密挑战（随机字节）
   - 检索用户信息（ID、显示名称）
   - 使用 Fido2NetLib 生成凭据创建选项
   - 将选项存储在分布式缓存中（3 分钟后过期）
   - 将选项返回给客户端

**4. 平台特定的凭据创建**

   **Web 平台** (`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/WebAuthnService.cs`):
   - 通过 Bit.Butil 直接调用浏览器的 WebAuthn API
   - 浏览器显示原生生物识别提示
   
   **Blazor Hybrid** (`src/Client/AI.Boilerplate.Client.Maui/Infrastructure/Services/MauiWebAuthnService.cs`, `src/Client/AI.Boilerplate.Client.Windows/Infrastructure/Services/WindowsWebAuthnService.cs`):
   - **挑战**: WebView 使用基于 IP 的源 (`http://0.0.0.1`) - WebAuthn 需要**有效的源**
   - **解决方案**: 使用本地 HTTP 服务器 + 应用内浏览器
     1. 在用户设备的可用端口上启动本地 HTTP 服务器（例如 `http://localhost:54321`）
     2. 在**应用内浏览器**（不是 WebView）中导航到 `http://localhost:54321/web-interop-app.html?actionName=CreateWebAuthnCredential`
     3. `web-interop-app.html` 在正确的 `localhost` 源上运行
     4. 从本地服务器获取选项：`GET /api/GetCreateWebAuthnCredentialOptions`
     5. 调用 WebAuthn API：`Bit.Butil.webAuthn.createCredential(options)`
     6. 设备显示原生生物识别提示
     7. 将结果回传：`POST /api/CreateWebAuthnCredential`
     8. 本地服务器完成 TaskCompletionSource
     9. 应用内浏览器自动关闭

**5. 客户端发送证明 (Attestation) 到服务器**
   ```
   客户端 → 服务器：PUT /api/User/CreateWebAuthnCredential
   ```

**6. 服务器验证并存储** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/UserController.WebAuthn.cs`)
   - 使用挑战检索缓存的选项
   - 使用 Fido2NetLib 验证证明响应
   - 验证凭据的唯一性
   - 创建包含以下内容的 `WebAuthnCredential` 实体：
     - 用户 ID
     - 凭据 ID
     - 公钥
     - 传输方式 (USB, NFC, BLE, Internal)
     - 备份资格标志
   - 保存到数据库
   - 从缓存中移除选项

**7. 客户端更新本地存储**
   - 将用户 ID 存储在本地存储的 `bit-webauthn` 键下
   - 稍后用于在登录页面显示“使用无密码登录”选项

---

### 认证流程（使用凭据）

当用户使用无密码认证登录时：

**1. 用户发起登录**
- 客户端检查 WebAuthn 是否可用且已配置（见前几步）
- 显示“使用指纹/Face ID 登录”按钮

**2. 客户端请求断言选项**
   ```
   客户端 → 服务器：POST /api/Identity/GetWebAuthnAssertionOptions
   正文：{ "UserIds": [<guid>, ...] }
   ```

**3. 服务器生成断言选项** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.WebAuthn.cs`)
   - 查询数据库中属于这些用户 ID 的凭据
   - 创建允许的凭据列表（凭据 ID + 传输方式）
   - 生成带挑战的断言选项
   - 将选项存储在分布式缓存中（3 分钟后过期）
   - 将选项返回给客户端

**4. 平台特定的凭据检索**

   **Web 平台**:
   - 调用浏览器的 `navigator.credentials.get()`
   - 浏览器显示凭据选择器 + 生物识别提示
   
   **Blazor Hybrid**:
   - 同样的本地 HTTP 服务器 + 应用内浏览器模式
   - 导航到 `web-interop-app.html?actionName=GetWebAuthnCredential`
   - 获取选项并调用 WebAuthn API
   - 通过本地 HTTP 端点返回断言

**5. 客户端选择登录方式**

   用户可以通过两种方式完成认证：

   **选项 A: 直接登录** (2FA 禁用)
   ```
   客户端 → 服务器：POST /api/Identity/VerifyWebAuthAndSignIn
   正文：{ 
     "ClientResponse": <断言>,
     "TfaCode": "123456" (可选)
   }
   ```

   **选项 B: 请求 2FA 代码** (2FA 启用，需要代码)
   ```
   客户端 → 服务器：POST /api/Identity/VerifyWebAuthAndSendTwoFactorToken
   正文：{ "ClientResponse": <断言> }
   ```
   - 服务器验证断言
   - 通过配置的方法发送 2FA 代码
   - 用户随后使用代码登录

**6. 服务器验证断言** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.WebAuthn.cs`)
   - 使用挑战检索缓存的选项
   - 按凭据 ID 在数据库中查找凭据
   - 使用 Fido2NetLib 验证断言：
     - 使用存储的公钥验证签名
     - 检查签名计数器（防止重放攻击）
     - 验证用户句柄是否与凭据所有者匹配
   - 更新数据库中的签名计数器
   - 生成自动登录 OTP (6 位代码)
   - 完成登录或发送 2FA 令牌

**7. 用户已认证**
   - 签发 JWT 令牌
   - 在数据库中创建会话
   - 用户被重定向到应用程序

---

## 🧩 关键组件详解

### 1. WebAuthn 数据模型

**`WebAuthnCredential` 实体** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/Models/WebAuthnCredential.cs`):

在数据库中存储凭据信息：

```csharp
public class WebAuthnCredential
{
    public byte[] Id { get; set; }              // 凭据 ID (唯一标识符)
    public Guid UserId { get; set; }            // 此凭据的所有者
    public byte[]? PublicKey { get; set; }      // 用于签名验证的公钥
    public uint SignCount { get; set; }         // 防止重放攻击的计数器
    public AuthenticatorTransport[]? Transports { get; set; }  // 访问凭据的方式
    public bool IsBackupEligible { get; set; }  // 是否可以备份到云端
    public bool IsBackedUp { get; set; }        // 当前已备份
    public DateTimeOffset RegDate { get; set; } // 凭据创建时间
    // ... 其他元数据
}
```

### 2. 客户端服务层级

**`WebAuthnServiceBase`** (`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/WebAuthnServiceBase.cs`):

管理本地存储中用户 ID 的抽象基类：

```csharp
public abstract class WebAuthnServiceBase : IWebAuthnService
{
    // 平台特定实现必须提供：
    public abstract ValueTask<bool> IsWebAuthnAvailable();
    public abstract ValueTask<JsonElement> CreateWebAuthnCredential(JsonElement options);
    public abstract ValueTask<JsonElement> GetWebAuthnCredential(JsonElement options);

    // 所有平台的共享逻辑：
    public async ValueTask<Guid[]> GetWebAuthnConfiguredUserIds() { /* ... */ }
    public async ValueTask<bool> IsWebAuthnConfigured(Guid? userId) { /* ... */ }
    public async ValueTask SetWebAuthnConfiguredUserId(Guid userId) { /* ... */ }
    public async ValueTask RemoveWebAuthnConfiguredUserId(Guid? userId) { /* ... */ }
}
```

**平台实现**:

- **`WebAuthnService`** (Web): 直接使用 Bit.Butil WebAuthn 包装器
- **`MauiWebAuthnService`** (MAUI): 使用本地 HTTP 服务器模式
- **`WindowsWebAuthnService`** (Windows): 使用本地 HTTP 服务器模式

### 3. 本地 HTTP 服务器模式（仅限 Blazor Hybrid）

**为什么需要它？**

Blazor Hybrid 中的 WebView 使用基于 IP 的源，如 `http://0.0.0.1`。WebAuthn 需要有效的源（例如 `https://example.com` 或 `http://localhost`）。如果没有有效的源，WebAuthn API 将失败。

**工作原理:**

**`ILocalHttpServer` 接口** (`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/Contracts/ILocalHttpServer.cs`):

```csharp
public interface ILocalHttpServer : IAsyncDisposable
{
    int EnsureStarted();  // 启动服务器并返回端口
    int Port { get; }     // 服务器监听的端口
    string? Origin { get; } // 源字符串 (例如 "http://localhost:54321")
}
```

**`MauiLocalHttpServer` 实现**:

1. **查找可用端口**: 使用 TCP 监听器查找空闲端口
2. **启动 EmbedIO 服务器**: 嵌入在应用中的轻量级 HTTP 服务器
3. **注册端点**:
   - `GET /api/GetWebAuthnCredentialOptions` - 返回断言选项（用于登录）
   - `POST /api/WebAuthnCredential` - 接收断言结果（用于登录）
   - `GET /api/GetCreateWebAuthnCredentialOptions` - 返回凭据创建选项（用于注册）
   - `POST /api/CreateWebAuthnCredential` - 接收证明结果（用于注册）
   - `POST /api/LogError` - 接收 JavaScript 错误
   - `GET /*` - 提供静态文件 (web-interop-app.html, app.js 等)
4. **通信结果**: 使用 `TaskCompletionSource` 将结果传回调用代码

**`IExternalNavigationService` 接口**:

```csharp
public interface IExternalNavigationService
{
    Task NavigateToAsync(string url);  // 在应用内浏览器中打开 URL
}
```

平台实现：
- **MAUI**: 使用 `SFSafariViewController` (iOS) 和 `CustomTabs` (Android)
- **Windows**: 在单独窗口中使用 `WebView2`

### 4. web-interop-app.html

**位置**: `src/Client/AI.Boilerplate.Client.Web/wwwroot/web-interop-app.html`

一个轻量级的 HTML 页面，用于：

1. **在正确的源上运行** (Blazor Hybrid 中的 `localhost`)
2. **加载最小的 JavaScript** (仅 `app.js`，而非完整的 Blazor)
3. **处理 WebAuthn 操作**:
   - 从本地服务器获取选项
   - 调用 WebAuthn API
   - 将结果回传到本地服务器
   - 自动关闭

**关键脚本** (`src/Client/AI.Boilerplate.Client.Core/Scripts/WebInteropApp.ts`):

```typescript
export class WebInteropApp {
    public static async run() {
        const action = urlParams.get('actionName');
        
        switch (action) {
            case 'GetWebAuthnCredential':
                await WebInteropApp.getWebAuthnCredential();
                break;
            case 'CreateWebAuthnCredential':
                await WebInteropApp.createWebAuthnCredential();
                break;
        }
        
        window.close(); // 完成后自动关闭
    }
    
    private static async getWebAuthnCredential() {
        // 从本地服务器获取断言选项（用于登录）
        const options = await fetch('api/GetWebAuthnCredentialOptions').json();
        
        // 调用 WebAuthn API (这就是我们需要正确源的原因!)
        const credential = await Bit.Butil.webAuthn.getCredential(options);
        
        // 回传断言结果
        await fetch('api/WebAuthnCredential', {
            method: 'POST',
            body: JSON.stringify(credential)
        });
    }
    
    // createWebAuthnCredential 类似...
}
```

### 5. 服务器端控制器

**`UserController.WebAuthn.cs`** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/UserController.WebAuthn.cs`) - 凭据管理:

- `GetWebAuthnCredentialOptions()` - 生成凭据创建选项
- `CreateWebAuthnCredential()` - 验证并存储新凭据
- `DeleteWebAuthnCredential()` - 删除特定凭据
- `DeleteAllWebAuthnCredentials()` - 删除用户的所有凭据

**`IdentityController.WebAuthn.cs`** (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/IdentityController.WebAuthn.cs`) - 认证:

- `GetWebAuthnAssertionOptions()` - 生成用于登录的断言选项
- `VerifyWebAuthAndSignIn()` - 验证断言并完成登录
- `VerifyWebAuthAndSendTwoFactorToken()` - 验证断言并发送 2FA 代码

### 6. Fido2NetLib 集成

该项目使用 **Fido2NetLib** 库进行服务器端 WebAuthn 操作：

**配置** (`Program.Services.cs`):

```csharp
services.AddFido2(options =>
{
    options.ServerName = "AI.Boilerplate WebAuthn";
    options.ServerDomain = new Uri(serverAddress).Host;
    options.Origins = [serverAddress];
    options.TimestampDriftTolerance = serverSettings.Identity.BearerTokenExpiration;
});
```

**关键操作**:

- `RequestNewCredential()` - 生成凭据创建选项
- `MakeNewCredentialAsync()` - 验证证明响应
- `GetAssertionOptions()` - 生成断言选项
- `MakeAssertionAsync()` - 验证断言响应

---
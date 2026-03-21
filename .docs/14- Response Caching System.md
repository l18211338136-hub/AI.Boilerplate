# 第十四阶段：响应缓存系统

欢迎回到第十四阶段！在本阶段，您将了解本项目内置的综合**四层响应缓存系统**。这种先进的缓存架构显著提高了应用程序性能，减少了服务器负载，并提供了卓越的用户体验。

---

## 概述

本项目实施了一种复杂的缓存策略，跨越**四个不同的层级**，每一层都在整体性能优化策略中发挥着特定作用：

1. **客户端内存缓存 **(Client In-Memory Cache) - 最快，应用级缓存（同步，即时）
2. **浏览器 HTTP 缓存 **(Browser HTTP Cache) - 客户端 HTTP 缓存（快速，跨会话持久化）
3. **CDN 边缘缓存 **(CDN Edge Cache) - 边缘位置的分布式缓存 (Cloudflare)
4. **ASP.NET Core 输出缓存 **(ASP.NET Core Output Cache) - 服务器端响应缓存（内存或 Redis）

---

## 核心组件

### 关键优势：缓存内容零服务器开销

**实际影响**：
- 每次刷新缓存页面（如 https://sales.bitplatform.dev 的产品页面）都不会给服务器增加**任何开销**。
- 完整响应直接由 Cloudflare 的边缘服务器 (CDN) 提供。
- 这极大地减少了服务器负载、数据库查询和基础设施成本。
- 能够以极少的服务器资源处理数百万次请求。

**重要安全提示**：
- 出于安全/隐私原因，已认证/登录用户的响应**不会**缓存在 CDN 或输出缓存中。
- 用户特定数据仅缓存在用户自己的浏览器/内存中（安全）。

---

### 1. AppResponseCacheAttribute

`AppResponseCacheAttribute` 是配置缓存行为的主要接口。位于 `/src/Shared/Infrastructure/Attributes/AppResponseCacheAttribute.cs`，可应用于：
- **Blazor 页面**（例如 `HomePage.razor`, `AboutPage.razor`）
- **Web API 控制器操作**（例如控制器中的方法）
- **最小 API 端点**（例如站点地图端点）

此属性在多个缓存层中缓存 **HTML、JSON、XML 和其他响应类型**。

**关键属性**：

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AppResponseCacheAttribute : Attribute
{
    /// <summary>
    /// 指定缓存持续时间（秒）。此设置将响应缓存在：
    /// - ASP.NET Core 的输出缓存
    /// - CDN 边缘服务器
    /// - 浏览器的缓存
    /// - 应用的内存缓存
    /// 
    /// 注意：浏览器和内存缓存无法自动清除，请谨慎使用。
    /// </summary>
    public int MaxAge { get; set; } = -1;

    /// <summary>
    /// 指定共享缓存的缓存持续时间（秒）。此设置将响应缓存在：
    /// - ASP.NET Core 的输出缓存
    /// - CDN 边缘服务器
    /// 
    /// 可以使用 ResponseCacheService 随时清除此缓存。
    /// </summary>
    public int SharedMaxAge { get; set; } = -1;

    /// <summary>
    /// 如果响应不受认证用户影响，则设置为 true。
    /// 允许即使对于认证请求也在 CDN 边缘和输出缓存上缓存响应。
    /// 
    /// 警告：如果您的页面/API 包含用户特定数据（用户名、角色、租户），
    /// 将此设置为 true 可能会通过共享缓存将这些数据泄露给其他用户。
    /// 仅当响应对所有用户都完全相同时才设置为 true。
    /// </summary>
    public bool UserAgnostic { get; set; }
}
```

**使用示例**：

```csharp
// 示例 1: 缓存 Blazor 页面 (HomePage.razor)
@page "/"
@attribute [AppResponseCache(SharedMaxAge = 3600 * 24, MaxAge = 60 * 5)]

// SharedMaxAge = CDN/输出缓存 24 小时（可清除）
// MaxAge = 浏览器/内存缓存 5 分钟（不可清除），当用户导航回本地缓存页面时提升页面导航体验

// 注意：流式渲染 (StreamRendering) 与响应缓存不兼容。
// 当当前请求配置为响应缓存时，AppResponseCachePolicy 会自动禁用流式传输。
```

```csharp
// 示例 2: 缓存条款页面一周
@page "/terms"
@attribute [AppResponseCache(SharedMaxAge = 3600 * 24 * 7, MaxAge = 60 * 5)]

// SharedMaxAge = CDN/输出缓存 7 天
// MaxAge = 浏览器/内存缓存 5 分钟
```

```csharp
// 示例 3: 缓存最小 API 端点 (SiteMapsEndpoint.cs)
app.MapGet("/sitemap_index.xml", [AppResponseCache(SharedMaxAge = 3600 * 24 * 7)] async (context) =>
{
    // 生成站点地图 XML
    // 在 CDN 和输出缓存中缓存 7 天
})
.CacheOutput("AppResponseCachePolicy")
.WithTags("Sitemaps");
```

```csharp
// 示例 4: 具有公开、与用户无关数据的最小 API
app.MapGet("/api/minimal-api-sample/{routeParameter}", 
    [AppResponseCache(MaxAge = 3600 * 24)] 
    (string routeParameter, [FromQuery] string queryStringParameter) => new
    {
        RouteParameter = routeParameter,
        QueryStringParameter = queryStringParameter
    })
.WithTags("Test")
.CacheOutput("AppResponseCachePolicy");
```

---

### 2. AppResponseCachePolicy

`AppResponseCachePolicy` 类（位于 `/src/Server/AI.Boilerplate.Server.Shared/Infrastructure/Services/AppResponseCachePolicy.cs`）实现了实际的缓存逻辑。它是 ASP.NET Core `IOutputCachePolicy` 接口的实现。

**关键特性**：

- **智能缓存层选择**：根据上下文自动确定使用哪些缓存层
- **感知用户的缓存**：防止认证用户数据被缓存在共享缓存中
- **文化差异处理**：处理多语言缓存，使用特定于文化的缓存键
- **开发模式处理**：在开发中禁用客户端缓存以便于调试
- **请求类型检测**：针对 Blazor 页面与 API 请求的不同行为

注意：**多语言限制**：对于非不变全球化，预渲染的 Blazor 页面的客户端和边缘缓存被禁用。
这是因为免费版的 Cloudflare CDN 不支持此功能，需要支持多维度（文化 + URL）基于标签清除的企业版计划。
您可以切换到 AWS CloudFront 或 Azure Frontdoor，它们在较低/免费计划中支持此功能。
输出缓存在多语言场景中仍能正常工作。

**缓存持续时间逻辑**：

```csharp
public async ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
{
    var responseCacheAtt = context.HttpContext.GetResponseCacheAttribute();
    
    if (responseCacheAtt is null) return;

    // 默认：如果未指定，SharedMaxAge = MaxAge
    if (responseCacheAtt.SharedMaxAge == -1)
    {
        responseCacheAtt.SharedMaxAge = responseCacheAtt.MaxAge;
    }

    var clientCacheTtl = responseCacheAtt.MaxAge;      // 内存 + 浏览器
    var edgeCacheTtl = responseCacheAtt.SharedMaxAge;  // CDN 边缘
    var outputCacheTtl = responseCacheAtt.SharedMaxAge; // ASP.NET Core 输出缓存

    // 如果配置了则禁用 CDN 边缘
    if (settings.ResponseCaching?.EnableCdnEdgeCaching is false)
        edgeCacheTtl = -1;

    // 如果配置了则禁用输出缓存
    if (settings.ResponseCaching?.EnableOutputCaching is false)
        outputCacheTtl = -1;

    // 在开发中禁用客户端缓存
    if (env.IsDevelopment())
        clientCacheTtl = -1;

    // 安全：为用户特定响应禁用共享缓存
    if (context.HttpContext.User.IsAuthenticated() && responseCacheAtt.UserAgnostic is false)
    {
        edgeCacheTtl = -1;
        outputCacheTtl = -1;
    }

    // ... 设置缓存头和输出缓存策略
}
```

**重要安全提示**：

`UserAgnostic` 属性对安全至关重要。如果响应包含用户特定数据（例如用户名、角色或租户信息），则**绝不能**将其缓存在共享缓存（CDN 边缘或输出缓存）中。仅当响应对所有用户都完全相同时，设置 `UserAgnostic = true` 才是安全的。

---

### 3. ResponseCacheService

`ResponseCacheService`（位于 `/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Services/ResponseCacheService.cs`）提供了在数据更改时**清除/失效缓存响应**的方法。

**目的**：当您在服务器上更新数据时（例如在管理面板中编辑产品），您需要使显示该数据的页面/API 的缓存版本失效。否则，用户将继续看到过时的信息，直到缓存自然过期。

**真实示例 - 产品页面缓存**：

1. **初始状态**：像 `https://sales.bitplatform.dev/product/10036` 这样的产品页面被查看并缓存在 Cloudflare CDN 上。
2. **数据更新**：管理员在 `https://adminpanel.bitplatform.dev/add-edit-product/e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6` 更新了产品。
3. **缓存清除**：服务器自动向 Cloudflare 发送请求，从边缘缓存中清除/移除该页面。
4. **下次请求**：下一个访问该产品页面的用户将获得更新后的版本（然后再次被缓存）。

**关键方法**：

```csharp
public partial class ResponseCacheService
{
    /// <summary>
    /// 从 ASP.NET Core 输出缓存和 CDN 边缘缓存中清除特定 URL 路径的缓存
    /// </summary>
    public async Task PurgeCache(params string[] relativePaths)
    {
        // 从 ASP.NET Core 输出缓存清除
        foreach (var relativePath in relativePaths)
        {
            await outputCacheStore.EvictByTagAsync(relativePath, default);
        }
        
        // 从 Cloudflare CDN 清除
        await PurgeCloudflareCache(relativePaths);
    }

    /// <summary>
    /// 清除所有与产品相关缓存的便捷方法
    /// </summary>
    public async Task PurgeProductCache(int shortId)
    {
        await PurgeCache(
            "/",                                  // 首页（可能列出产品）
            $"/product/{shortId}",                // 产品详情页
            $"/api/ProductView/Get/{shortId}"     // 产品 API 端点
        );
    }
}
```

**在控制器中的使用**：

```csharp
[HttpPut]
public async Task<ProductDto> Update(ProductDto dto, CancellationToken cancellationToken)
{
    // ... 更新逻辑 ...
    await DbContext.SaveChangesAsync(cancellationToken);

    // 清除该产品的所有缓存
    await responseCacheService.PurgeProductCache(entityToUpdate.ShortId);

    return entityToUpdate.Map();
}

[HttpDelete("{id}/{version}")]
public async Task Delete(Guid id, string version, CancellationToken cancellationToken)
{
    // ... 删除逻辑 ...
    await DbContext.SaveChangesAsync(cancellationToken);

    // 清除该产品的所有缓存
    await responseCacheService.PurgeProductCache(entityToDelete.ShortId);
}
```

**重要提示**：
- 为了成功清除缓存，请求 URL 必须与传递给 `PurgeCache()` 的 URL**完全匹配**。
- 查询字符串和路由参数必须精确匹配。
- 这仅清除 **CDN 边缘缓存** 和 **ASP.NET Core 输出缓存**（可清除的层）。
- **浏览器缓存** 和 **客户端内存缓存** 无法远程清除（这就是为什么要谨慎使用 `MaxAge` 的原因）。

**针对不可清除缓存的缓存破坏策略**：

由于浏览器缓存和客户端内存缓存无法远程清除，请使用**版本化 URL**（缓存破坏）来确保用户看到更新的内容。此技术将版本参数附加到 URL，当数据更新时该参数会发生变化。

```csharp
// ProductDto.cs 中的示例 - 带版本参数的产品图片 URL
public string? GetPrimaryMediumImageUrl(Uri absoluteServerAddress)
{
    return HasPrimaryImage is false
        ? null
        : new Uri(absoluteServerAddress, 
            $"/api/Attachment/GetAttachment/{Id}/{AttachmentKind.ProductPrimaryImageMedium}?v={Version}")
            .ToString();
}
```

**工作原理**：
- `Version` 属性（用于乐观并发控制的 `long` 类型）在实体每次更新时都会变化。
- 当产品更新时，版本发生变化，创建一个**新 URL**，绕过所有缓存版本。
- 浏览器/客户端内存缓存将其视为一个全新的资源并获取新鲜数据。

此模式非常适合图片、文档或任何您希望积极缓存但需要在数据更改时立即更新的内容等资源。

---

### 4. 客户端内存缓存 (CacheDelegatingHandler)

`CacheDelegatingHandler`（位于 `/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/HttpMessageHandlers/CacheDelegatingHandler.cs`）实现了 HTTP 响应的客户端内存缓存。

**工作原理**：

```csharp
protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
{
    var cacheKey = $"{request.Method}-{request.RequestUri}";
    var useCache = AppEnvironment.IsDevelopment() is false && AppPlatform.IsBlazorHybridOrBrowser;

    // 尝试从缓存获取
    if (useCache && memoryCache.TryGetValue(cacheKey, out ResponseMemoryCacheItems? cachedResponse))
    {
        // 同步返回缓存响应（即时，无加载指示器！）
        memoryCacheStatus = "HIT";
        return CreateHttpResponseFromCache(cachedResponse);
    }

    // 发出实际请求
    var response = await base.SendAsync(request, cancellationToken);

    // 如果响应有 Cache-Control: max-age 头则缓存
    if (useCache && response.IsSuccessStatusCode && 
        response.Headers.CacheControl?.MaxAge is TimeSpan maxAge && maxAge > TimeSpan.Zero)
    {
        memoryCacheStatus = "MISS";
        var responseContent = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        memoryCache.Set(cacheKey, new ResponseMemoryCacheItems
        {
            Content = responseContent,
            StatusCode = response.StatusCode,
            ResponseHeaders = response.Headers.ToDictionary(),
            ContentHeaders = response.Content.Headers.ToDictionary()
        }, maxAge);
    }

    return response;
}
```

**关键特性**：
- 仅在**非开发**环境中激活
- 仅适用于 **Blazor Hybrid 和浏览器** 平台（不适用于服务器端渲染）
- **适用于所有客户端平台**：Web 浏览器、.NET MAUI 移动应用、Windows 桌面应用
- 尊重服务器响应中的 `Cache-Control: max-age` 头
- 存储整个 HTTP 响应（内容、状态码和头信息）
- **同步响应**：即时返回缓存数据，无任何异步延迟
- **无加载指示器**：防止出现旋转图标、闪烁效果和骨架屏 UI
- 为重复请求提供最快的响应时间

**真实示例**：
如果您在 `https://sales.bitplatform.dev` 上浏览不同产品：
1. **打开网站**：导航到 [https://sales.bitplatform.dev](https://sales.bitplatform.dev)
2. **首次访问** 产品 A：服务器请求，数据缓存在内存中
3. **导航** 到产品 B：服务器请求，数据缓存在内存中
4. **返回导航** 到产品 A：**即时加载** 自内存缓存 - 无加载指示器，无旋转图标，无闪烁 - 页面瞬间出现！

这创造了极其流畅的用户体验，因为应用感觉像原生应用一样响应迅速。

**重要提示**：
- **客户端内存缓存** 在应用关闭时清除（不跨会话持久化）。
- **浏览器 HTTP 缓存** 即使在关闭浏览器后也持久存在，但它是异步的（会短暂显示加载）。
- 两者的结合提供了最佳用户体验：
  - 当前会话期间的即时加载（客户端内存缓存）
  - 再次访问时的快速加载（浏览器缓存）

当从页面 A 导航回首页时，您可能会遇到加载指示器。这是预期行为：初始页面加载不向服务器发送任何 HTTP 请求，因为它从预渲染状态获取所有所需数据。因此，`CacheDelegatingHandler.cs` 不会为其缓存任何内容。

---

## 四层缓存架构

### 请求流程与缓存层顺序

当用户发出请求时，它按顺序流经这些层：

```
┌─────────────────────────────────────────────────────────────┐
│  客户端发出请求：GET /api/ProductView/Get/123               │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  1. 客户端内存缓存检查 (CacheDelegatingHandler)             │
│     - 最快 (微秒级 - 同步)                                   │
│     - 无加载指示器、旋转图标或闪烁效果                       │
│     - 仅在当前应用会话期间有效                               │
│     - 不可清除                                               │
└─────────────────────────────────────────────────────────────┘
        │ 未命中 (MISS)                  │ 命中 (HIT)
        ▼                               └──────► 从内存返回 (即时)
┌─────────────────────────────────────────────────────────────┐
│  2. 浏览器 HTTP 缓存检查 (标准浏览器缓存)                    │
│     - 非常快 (毫秒级 - 异步)                                 │
│     - 短暂显示加载指示器                                     │
│     - 跨应用会话/浏览器重启持久化                            │
│     - 服务器无法清除                                         │
└─────────────────────────────────────────────────────────────┘
        │ 未命中 (MISS)                  │ 命中 (HIT)
        ▼                               └──────► 从浏览器返回
┌─────────────────────────────────────────────────────────────┐
│  请求进入网络                                                │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  3. CDN 边缘缓存检查 (Cloudflare)                           │
│     - 快 (10-50ms)                                           │
│     - 可通过 ResponseCacheService 清除                       │
│     - 全球分发 (从最近的边缘节点服务)                        │
└─────────────────────────────────────────────────────────────┘
        │ 未命中 (MISS)                  │ 命中 (HIT)
        ▼                               └──────► 从 CDN 返回
┌─────────────────────────────────────────────────────────────┐
│  请求到达 ASP.NET Core 服务器                                │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  4. 输出缓存检查 (ASP.NET Core)                             │
│     - 中等速度 (50-100ms)                                    │
│     - 可通过 ResponseCacheService 清除                       │
│     - 可使用内存或 Redis 后端                                │
└─────────────────────────────────────────────────────────────┘
        │ 未命中 (MISS)                  │ 命中 (HIT)
        ▼                               └──────► 从输出缓存返回
┌─────────────────────────────────────────────────────────────┐
│  执行控制器操作 / 查询数据库                                 │
│  生成响应                                                    │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  响应流回所有缓存层                                          │
│  每一层根据其配置进行缓存                                    │
└─────────────────────────────────────────────────────────────┘
```

### 对比表

| 层级 | 位置 | 速度 | 范围 | 可清除 | 控制者 | 最佳用途 |
|-------|----------|-------|-------|-----------|---------------|----------|
| **1. 客户端内存缓存** | 客户端应用内存 | ⚡ 最快 (微秒级，**同步**) | 单个用户，仅限当前会话 | ❌ 否 | `MaxAge` | 用户在已访问页面间即时导航 |
| **2. 浏览器 HTTP 缓存** | 浏览器的 HTTP 缓存 | 🚀 非常快 (毫秒级，异步) | 单个用户，跨会话持久化 | ❌ 否 | `MaxAge` | 关闭/重新打开应用后返回页面 |
| **3. CDN 边缘** | Cloudflare/CDN 边缘 | 💨 快 (10-50ms) | 全球，所有用户共享 | ✅ 是 | `SharedMaxAge` | 向全球众多用户提供的公共内容 |
| **4. 输出缓存** | ASP.NET Core 服务器 | ⏱️ 中等 (50-100ms) | 服务器级，用户间共享 | ✅ 是 | `SharedMaxAge` | 预渲染页面，API 响应 |

### 重要安全提示

**用户特定内容保护**：
- 如果用户已认证且 `UserAgnostic = false`，则响应**不会**缓存在：
  - ❌ CDN 边缘缓存
  - ❌ ASP.NET Core 输出缓存
- 但它**仍然可以**缓存在：
  - ✅ 浏览器 HTTP 缓存（用户自己的浏览器）
  - ✅ 客户端内存缓存（用户自己的应用实例）

这防止了通过共享缓存意外地将用户 A 的数据服务给用户 B。

---

## 配置

### appsettings.json

```json
{
  "ResponseCaching": {
    "EnableOutputCaching": true,  // ASP.NET Core 输出缓存
    "EnableCdnEdgeCaching": true  // CDN 边缘缓存
  },
  "Cloudflare": {
    "ZoneId": "your-cloudflare-zone-id",
    "ApiToken": "your-cloudflare-api-token",
    "AdditionalDomains": [
      "https://sales.bitplatform.ai",
      "https://sales.bitplatform.com",
      "https://sales.bitplatform.uk"
    ]
  }
}
```

---

## FusionCache 库

本项目使用 **FusionCache** 库进行服务器端缓存：

- **输出缓存后端**：为 ASP.NET Core 输出缓存实现提供支持（第 4 层）
- **数据缓存**：通过 `IFusionCache` 接口提供数据缓存，用于缓存任意数据（数据库查询结果、计算值等），除了 HTTP 响应之外
- **灵活存储**：支持多种后端（内存、Redis、混合等），用于响应和数据缓存

---

## Redis 基础设施

本项目使用**两个独立的 Redis 实例**用于不同目的：

### 1. redis-cache 临时缓存
- **无持久化**（数据仅存储在内存中）
- **用途**：
  - **FusionCache** L2 分布式缓存和多服务器缓存同步的背板 (backplane)
  - **SignalR 背板** 用于跨服务器的实时消息传递
- **原因**：缓存数据可再生，无需磁盘 I/O 开销

### 2. redis-persistent 持久化存储
- **启用 AOF** 并同步写入磁盘以确保最大耐用性
- **用途**：
  - **Hangfire** 后台作业队列和状态
  - **分布式锁** 用于协调操作
- **原因**：不易再生的关键数据必须在重启后幸存

**优势**：分离使得临时缓存运行更快，同时确保关键基础设施数据永不丢失。

---

### 监控缓存头

系统添加自定义头以帮助调试缓存：

```
App-Cache-Response: Output:3600,Edge:3600,Client:3600
```

这显示了每个缓存层的 TTL（秒）。使用浏览器开发者工具的 Network 标签进行检查：

```
Cache-Control: public, max-age=300, s-maxage=3600
App-Cache-Response: Output:3600,Edge:3600,Client:300
```

解释：
- `max-age=300`: 浏览器和内存缓存 5 分钟
- `s-maxage=3600`: CDN 边缘和输出缓存 1 小时
- `public`: 可以在共享缓存 (CDN) 中缓存

---

### AI Wiki: 已回答的问题
* [bit AI.Boilerplate AttachmentController 如何与响应缓存交互？为什么即使用户没有调用 PurgeCache 且这些资产存储在无法自动清除的浏览器缓存中，他们总是能看到最新的头像？](https://deepwiki.com/search/how-does-the-bit-ai.boilerplate-a_4f042d5f-3ffb-4c14-b661-bb923825c21d)
* [为什么响应缓存不适用于 bit AI.Boilerplate 中的流式预渲染？](https://deepwiki.com/search/why-response-caching-doesnt-wo_2de1ba6c-1017-4c77-96f5-33c8ed001760)

在此处提出您自己的问题：[https://wiki.bitplatform.dev](https://wiki.bitplatform.dev)

---
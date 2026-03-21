# 第十二阶段：Blazor 模式、预渲染与 PWA

欢迎回到 AI.Boilerplate 项目教程的第十二阶段！在本阶段，您将了解 Blazor 渲染模式、预渲染功能、渐进式 Web 应用 (PWA) 特性，以及它们如何在项目中协同工作。

---

## 目录
1. [App.razor 和 index.html 文件](#apprazor-和-indexhtml-文件)
2. [Blazor 模式与预渲染配置](#blazor-模式与预渲染配置)
3. [PWA 与服务工作者](#pwa-与服务工作者)

---

## App.razor 和 index.html 文件

本项目根据托管模型的不同使用不同的文件：

### 关键文件

1. **[`/src/Server/AI.Boilerplate.Server.Web/Components/App.razor`](/src/Server/AI.Boilerplate.Server.Web/Components/App.razor)** - Blazor Server、WebAssembly、Auto 和静态 SSR 的主要入口点
2. **[`/src/Client/AI.Boilerplate.Client.Web/wwwroot/index.html`](/src/Client/AI.Boilerplate.Client.Web/wwwroot/index.html)** - Blazor WebAssembly 独立模式的入口点
3. **[`/src/Client/AI.Boilerplate.Client.Maui/wwwroot/index.html`](/src/Client/AI.Boilerplate.Client.Maui/wwwroot/index.html)** - Blazor Hybrid / MAUI 的入口点

### 重要提示：需要同步

⚠️ **对 `App.razor` 的更改通常需要在 `index.html` 文件中进行类似的更改。**

当您修改 `App.razor` 中的结构、脚本、样式表或元数据时，应在 `index.html` 中镜像这些更改，以确保两种托管模型下的行为一致。

**需要同步的示例场景：**
- 添加新的 CSS 文件或 JavaScript 库
- 更新元标签或 favicon
- 为第三方集成添加新脚本
- 修改应用容器结构

### 文件间的共同元素

**`App.razor` 和 `index.html` 文件都包含：**

**元标签和视口设置：**
```html
<meta charset="utf-8" />
<meta name="theme-color">
<meta name="viewport" content="width=device-width, initial-scale=1.0, viewport-fit=cover" />
```

**性能优化 - 预连接链接：**
```html
<link rel="preconnect" href="https://www.google.com">
<link rel="preconnect" href="https://www.gstatic.com" crossorigin>
<link rel="preconnect" href="https://js.monitor.azure.com" crossorigin>
```

**PWA 支持：**
```html
<link rel="icon" href="favicon.ico" type="image/x-icon" />
<link rel="apple-touch-icon" sizes="512x512" href="images/icons/bit-icon-512.png" />
<link rel="manifest" href="manifest.json" />
```

**Bit.BlazorUI 样式表：**
```html
<link href="_content/Bit.BlazorUI/styles/bit.blazorui.css" rel="stylesheet" />
<link href="_content/Bit.BlazorUI.Icons/styles/bit.blazorui.icons.css" rel="stylesheet" />
<link href="_content/Bit.BlazorUI.Assets/styles/bit.blazorui.assets.css" rel="stylesheet" />
<link href="_content/Bit.BlazorUI.Extras/styles/bit.blazorui.extras.css" rel="stylesheet" />
<link href="_content/AI.Boilerplate.Client.Core/styles/app.css" rel="stylesheet" />
```

**服务工作者和应用脚本：**
```html
<!-- Bit.Bswup 用于 PWA 服务工作者管理 -->
<script src="_content/Bit.Bswup/bit-bswup.js"></script>
<script src="_content/Bit.Bswup/bit-bswup.progress.js"></script>

<!-- 核心应用脚本 -->
<script src="_content/Bit.Butil/bit-butil.js"></script>
<script src="_content/Bit.Besql/bit-besql.js"></script>
<script src="_content/Bit.BlazorUI/scripts/bit.blazorui.js"></script>
<script src="_content/AI.Boilerplate.Client.Core/scripts/app.js"></script>
<script src="_content/Bit.BlazorUI.Extras/scripts/bit.blazorui.extras.js"></script>
```

**Application Insights 初始化：**
两个文件都包含用于遥测和监控的 Application Insights 代码片段。

### 文件间的关键区别

**App.razor (服务器端)：**
- 使用带有 `@` 指令的 Razor 语法
- 可访问 `HttpContext` 和服务器端服务
- 根据配置动态确定渲染模式
- 可根据预渲染设置有条件地显示 `LoadingComponent`

```csharp
@{
    var noPrerender = HttpContext.Request.Query["no-prerender"].Count > 0;
    var renderMode = noPrerender ? noPrerenderBlazorWebAssembly : serverWebSettings.WebAppRender.RenderMode;
    if (HttpContext.AcceptsInteractiveRouting() is false)
    {
        // 非交互场景下的静态 SSR
        renderMode = null;
    }
}
```

**index.html (客户端)：**
- 纯 HTML 文件
- 内联包含静态加载组件的 HTML/CSS
- 使用 `blazor.webassembly.js` 或 `blazor.webview.js` (用于 Hybrid)
- 无动态服务器端逻辑

---

## Blazor 模式与预渲染配置

本项目支持多种 Blazor 托管模型，所有配置均在一个位置完成。

### 配置位置

**文件**: [`/src/Server/AI.Boilerplate.Server.Api/appsettings.json`](/src/Server/AI.Boilerplate.Server.Api/appsettings.json)

```json
"WebAppRender": {
    "BlazorMode": "BlazorServer",
    "BlazorMode_Comment": "BlazorServer, BlazorWebAssembly 和 BlazorAuto。",
    "PrerenderEnabled": false,
    "PrerenderEnabled_Comment": "对于启用了预渲染的应用，请遵循 Client.Web/wwwroot/service-worker.published.js 文件中的说明"
}
```

### 可用的 Blazor 模式

Blazor Server、Auto、WebAssembly、Blazor WebAssembly 独立模式和 Blazor Hybrid。

这篇 [文章](https://www.reddit.com/r/Blazor/comments/1kq5eyu/this_is_not_yet_just_another_incorrect_comparison/) 是比较 Blazor 模式的好资源。
简而言之：
- 仅将 **Blazor Server** 用于开发目的。
- 将 **Blazor WebAssembly** 或 **Blazor WebAssembly 独立模式** 用于生产环境。
- 将 **Blazor Hybrid** 用于 Android、iOS、Windows 和 macOS 应用。

### 预渲染：更快的加载速度与更好的 SEO

**预渲染 (PreRendering)** 意味着服务器在将内容发送到浏览器之前生成初始 HTML 内容。这带来了两大好处：

1. **更快的感知加载时间**：即使在 Blazor 初始化期间，用户也能立即看到内容。
2. **更好的 SEO**：搜索引擎可以抓取完全渲染的 HTML 内容。

#### PrerenderEnabled 设置

```json
"PrerenderEnabled": true   // 立即显示内容 + SEO 优势
"PrerenderEnabled": false  // 应用初始化时显示加载屏幕
```

---

## PWA 与服务工作者

本项目在 Blazor Server、WebAssembly 和 Auto 模式下都是一个**功能齐全的渐进式 Web 应用 (PWA)**。

### 所有模式下均可用的 PWA 优势

✅ **可安装性** - 用户可以将应用安装到设备上（桌面端、移动端）  
✅ **推送通知** - 即使应用关闭也能发送通知  
✅ **类原生体验** - 在无浏览器界面的独立窗口中运行  
✅ **性能** - 服务工作者的缓存使后续加载更快  
✅ **后台同步** - 连接恢复时可同步数据  
✅ **添加到主屏幕** - 用户可以将应用图标添加到设备主屏幕  
✅ **离线支持** - 应用在没有网络连接的情况下继续工作（仅限 WebAssembly 和 WebAssembly 独立模式）

### 服务工作者文件

本项目使用 **Bit.Bswup** (BitPlatform Service Worker Update) 来管理工作者的生命周期。

**文件**: [`/src/Client/AI.Boilerplate.Client.Web/wwwroot/service-worker.published.js`](/src/Client/AI.Boilerplate.Client.Web/wwwroot/service-worker.published.js)

此文件控制：
- 资源缓存策略
- 离线行为
- 预渲染集成
- 更新检测与处理
- 推送通知处理

### 理解服务工作者模式

服务工作者有**四种不同的模式**以匹配应用的预渲染配置。让我们逐一探讨：

#### 模式 1: FullOffline (传统 PWA)

```javascript
// self.mode = 'FullOffline';
```

**工作原理：** 先下载**所有**资源，**然后**运行应用。

**行为：**
- 首次加载时，下载并缓存所有应用资源（JS、CSS、图片等）
- 显示下载进度指示器
- 一旦所有资源被缓存，应用启动
- 后续访问完全离线工作

**推荐用于：**
- 具有本地数据库 (IndexedDB, 通过 Bit.Besql 的 SQLite) 的离线优先应用
- **必须**在无网络连接下工作的应用
- 现场服务应用、医疗应用
- 保证离线导航至关重要的应用

**演示：** https://todo-offline.bitplatform.cc/offline-todo

**优点：**
- ✅ 保证离线功能
- ✅ 导航过程中网络丢失也不会出现页面损坏
- ✅ 首次加载后性能一致

**缺点：**
- ❌ 初始加载时间较长
- ❌ 首次访问带宽占用大
- ❌ 用户在使用应用前必须等待所有下载完成

#### 模式 2: NoPrerender (现代 PWA) - 默认

```javascript
self.mode = 'NoPrerender';
```

**工作原理：** 立即启动，并按需懒加载资源。

**行为：**
- 应用瞬间启动
- 资源在用户导航时按需加载
- 逐步缓存资源以供离线使用
- 如果网络丢失，只有已访问的页面可离线工作

**推荐用于：**
- 利用 PWA 进行安装而非离线的现代应用
- 需要推送通知的应用
- 管理面板和内部工具
- 将即时启动作为优先级的应用

**演示：** https://adminpanel.bitplatform.dev/

**优点：**
- ✅ 应用瞬间启动
- ✅ 初始带宽占用最小
- ✅ 最适合无论如何都需要网络的应用

**缺点：**
- ❌ 未访问页面的资源需要网络
- ❌ 如果导航过程中连接丢失，应用可能会中断

**何时使用：**
- 您在 appsettings.json 中设置了 `PrerenderEnabled: false`
- PWA 功能是为了可安装性/通知，而非离线
- 您可以接受懒加载资源页面对网络的依赖

#### 模式 3: InitialPrerender

```javascript
// self.mode = 'InitialPrerender';
```

**工作原理：** 仅在**首次访问**时获取预渲染的 HTML。

**行为：**
- 首次访问：从服务器获取并显示预渲染的 HTML
- 在后台加载 Blazor 运行时期间显示内容
- 后续访问：使用缓存的应用，不再进行服务器预渲染
- 减少首次访问后的服务器负载

**推荐用于：**
- 具有快速首屏体验的 SEO 友好型应用
- 注重第一印象的面向公众的应用
- 平衡 SEO 优势和服务器资源的应用

**演示：** https://todo.bitplatform.dev/

**优点：**
- ✅ 首次访问感知加载快
- ✅ SEO 优势（搜索引擎能看到内容）
- ✅ 减少后续访问的服务器负载
- ✅ 为新访客提供更好的用户体验

**何时使用：**
- 您在 appsettings.json 中启用了 `PrerenderEnabled: true`
- 您想要 SEO 优势，但不想在每次刷新时都增加服务器负载
- 首次访问体验对用户转化至关重要

#### 模式 4: AlwaysPrerender

```javascript
// self.mode = 'AlwaysPrerender';
```

**工作原理：** 在**每次**应用加载时获取预渲染的 HTML。

**行为：**
- 每次页面加载都从服务器获取预渲染的 HTML
- 每次访问都能立即显示内容

**演示：** https://sales.bitplatform.dev/

**Initial Prerender (初始预渲染)** vs **Always Prerender (始终预渲染)**：
如果启用了预渲染，`Always Prerender` 会在每次应用加载时获取站点文档。之所以在每次应用加载时都获取文档，是因为 Blazor WebAssembly 的运行时在低端 Android 设备上可能需要一些时间才能启动，因此如果用户刷新页面或访问新页面，它会在 Blazor WebAssembly 运行时加载期间显示预渲染的文档。缺点？由于频繁的预渲染会增加服务器负载，这可以通过响应缓存来减少（将在后续阶段介绍）。

### 服务工作者资源配置

服务工作者控制要缓存哪些资源：

```javascript
self.assetsInclude = [];
self.assetsExclude = [
    /bit\.blazorui\.fluent\.css$/,
    /bit\.blazorui\.fluent-dark\.css$/,
    /bit\.blazorui\.fluent-light\.css$/,

    // 如果 PWA 中需要 PDF 阅读器，请删除这些行：
    /pdfjs-4\.7\.76\.js$/,
    /pdfjs-4\.7\.76-worker\.js$/,

    // 国家旗帜 (大文件)
    /_content\/Bit\.BlazorUI\.Extras\/flags/
];
```

**资源排除策略：**
- `assetsExclude`: 防止缓存您不需要离线的大文件
- 主题：仅缓存当前激活的主题，不缓存所有变体
- PDF.js: 默认排除（如果您需要离线 PDF 阅读器则删除）
- 国家旗帜：因体积大而排除（数百个旗帜图片）

**自定义提示：** 审查您的应用离线需求并相应调整排除项。

```javascript
self.externalAssets = [
    { "url": "/" },
    { url: "_framework/bit.blazor.web.es2019.js" },
    { "url": "AI.Boilerplate.Server.Web.styles.css" },
    { "url": "AI.Boilerplate.Client.Web.bundle.scp.css" }
];
```

**外部资源**是必须单独缓存的资源，因为服务工作者无法自动发现它们。

```javascript
self.serverHandledUrls = [
    /\/api\//,
    /\/odata\//,
    /\/core\//,
    /\/hangfire/,
    ...
];
```

**服务器处理的 URL**会绕过服务工作者，让服务器处理这些端点：
- API 端点 (`/api/`, `/odata/`)
- 管理界面 (`/hangfire`, `/healthchecks-ui`)
- 健康检查端点 (`/healthz`, `/health`, `/alive`)
- 认证回调 (`/signin-*`)
- 静态服务器资源 (sitemap, well-known 文件)

### Web 推送通知

服务工作者处理推送通知，即使应用已关闭：

```javascript
self.addEventListener('push', function (event) {
    const eventData = event.data.json();
    
    self.registration.showNotification(eventData.title, {
        data: eventData.data,
        body: eventData.message,
        icon: '/images/icons/bit-icon-512.png'
    });
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    const pageUrl = event.notification.data.pageUrl;
    if (pageUrl != null) {
        // 点击通知时导航到特定页面
        event.waitUntil(
            clients.openWindow(pageUrl)
        );
    }
});
```

**工作原理：**
1. 服务器向浏览器发送推送通知
2. 服务工作者接收推送事件（即使应用已关闭）
3. 显示包含标题、消息和图标的通知
4. 当用户点击通知时，应用打开并跳转到指定的 `pageUrl`（如果适用）

---
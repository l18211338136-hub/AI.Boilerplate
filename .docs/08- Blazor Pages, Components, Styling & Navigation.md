# 第八阶段：Blazor 页面、组件、样式与导航

欢迎回到入门指南的第八阶段！在本阶段，我们将探讨本项目中 Blazor UI 架构的工作原理，包括组件结构、SCSS 样式、主题变量以及导航系统。

---

## 1. 组件结构：三文件模式

在本项目中，Blazor 页面和组件遵循**三文件结构**，以分离关注点从而提高可维护性：

### 示例：ProductsPage

让我们以项目中的 `ProductsPage` 为例进行详细查看：

#### 文件 1：`ProductsPage.razor` (标记)
**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor)

此文件包含使用 Razor 语法和 Bit.BlazorUI 组件的 **UI 标记**：

```xml
@attribute [Route(PageUrls.Products)]
@attribute [Authorize(Policy = AuthPolicies.PRIVILEGED_ACCESS)]
@inherits AppPageBase

<AppPageData Title="@Localizer[nameof(AppStrings.Products)]" />

<section>
    <BitStack>
        <BitStack FitHeight>
            <BitButton IconName="@BitIconName.Add" 
                       OnClick="WrapHandled(CreateProduct)">
                @Localizer[nameof(AppStrings.AddProduct)]
            </BitButton>
            @if (isLoading)
            {
                <BitSlickBarsLoading />
            }
        </BitStack>
        <BitSpacer />
        <BitSearchBox OnSearch="HandleOnSearch"
                      Placeholder="@Localizer[nameof(AppStrings.SearchProductsPlaceholder)]" />
    </BitStack>
    <!-- 用于显示产品的 BitDataGrid -->
</section>
```

**关键点：**
- 使用 `@attribute` 进行路由和授权配置
- 继承自 `AppPageBase`（稍后详述）
- 使用 `Bit.BlazorUI` 组件，如 `BitButton`, `BitStack`, `BitDataGrid`
- 引用代码隐藏文件中的变量，如 `isSmallScreen`, `isLoading`
- 使用 `WrapHandled()` 处理事件（异常处理）

#### 文件 2：`ProductsPage.razor.cs` (代码隐藏)
**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor.cs)

此文件包含**组件逻辑**——组件的所有 C# 代码：

```csharp
using AI.Boilerplate.Shared.Dtos.Products;
using AI.Boilerplate.Shared.Controllers.Products;

namespace AI.Boilerplate.Client.Core.Components.Pages.Products;

public partial class ProductsPage
{
    private bool isLoading;
    private string? searchQuery;
    private ProductDto? deletingProduct;

    private BitDataGrid<ProductDto>? dataGrid;
    private BitDataGridItemsProvider<ProductDto> productsProvider = default!;

    [AutoInject] IProductController productController = default!;

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        PrepareGridDataProvider();
    }

    private void PrepareGridDataProvider()
    {
        productsProvider = async req =>
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                var query = new ODataQuery { /* OData 选项 */ };

                var data = await productController.WithQuery(query.ToString())
                                                  .GetProducts(req.CancellationToken);

                return BitDataGridItemsProviderResult.From(data!.Items!, (int)data!.TotalCount);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        };
    }

    private async Task CreateProduct()
    {
        NavigationManager.NavigateTo(PageUrls.AddOrEditProduct);
    }

    private async Task DeleteProduct()
    {
        if (deletingProduct is null) return;

        await productController.Delete(deletingProduct.Id, 
            deletingProduct.Version, 
            CurrentCancellationToken);

        await RefreshData();
    }
}
```

**关键点：**
- 声明为 `partial class` 以便与 `.razor` 文件连接
- 使用 `[AutoInject]` 进行依赖注入（简化的 DI 模式）
- 重写 `OnInitAsync()` 而不是 `OnInitializedAsync()`（来自基类的更安全生命周期方法）
- 可以访问从 `AppPageBase` 继承的服务，如 `ExceptionHandler`, `NavigationManager`, `CurrentCancellationToken`

#### 文件 3：`ProductsPage.razor.scss` (作用域样式)
**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor.scss`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor.scss)

此文件包含**特定于组件的样式**，这些样式会自动作用于该组件：

```scss
@import '../../../Styles/abstracts/_media-queries.scss';
@import '../../../Styles/abstracts/_bit-css-variables.scss';

.grid-container {
    overflow: auto;
    height: calc(#{$bit-env-height-available} - 12.1rem);
}

::deep {
    .products-grid {
        width: 100%;
        background-color: $bit-color-background-secondary;

        .name-col {
            padding-inline-start: 16px;
        }

        thead {
            background-color: $bit-color-background-tertiary;
        }

        td {
            border-bottom: 1px solid $bit-color-border-tertiary;
        }
    }
}
```

**关键点：**
- 导入共享的 SCSS 文件以获取媒体查询和主题变量
- 样式自动作用于该组件
- 使用 `::deep` 选择器来样式化子组件（下文将详细解释）
- 使用主题颜色变量（如 `$bit-color-background-secondary`）以支持深色/浅色模式

### 导航集成

为了让页面在应用程序中可访问，需要将其添加到导航系统中：

#### 添加到 NavBar
**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/Layout/NavBar.razor`](/src/Client/AI.Boilerplate.Client.Core/Components/Layout/NavBar.razor)

```xml
<BitNavBar TItem="BitNavBarOption">
    <BitNavBarOption Text="@Localizer[nameof(AppStrings.Home)]" 
                     IconName="@BitIconName.Home" 
                     Url="@PageUrls.Home" />
    
    <BitNavBarOption Text="@Localizer[nameof(AppStrings.Terms)]" 
                     IconName="@BitIconName.EntityExtraction" 
                     Url="@PageUrls.Terms" />
</BitNavBar>
```

#### 添加到导航面板
**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/Layout/MainLayout.razor.items.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Layout/MainLayout.razor.items.cs)

```csharp
navPanelItems.Add(new()
{
    Text = localizer[nameof(AppStrings.About)],
    IconName = BitIconName.Info,
    Url = PageUrls.About,
});
```

这确保页面同时出现在导航面板和 NavBar（移动端）中。

### 跨平台页面：特定于平台的组件

某些页面需要访问原生平台功能（例如设备信息、原生 API）。与其将它们放在 `AI.Boilerplate.Client.Core` 中并使用依赖注入或发布 - 订阅模式，不如直接在平台项目中创建**特定于平台的页面**。

#### 示例：AboutPage

`AboutPage` 展示了这种模式，它存在于多个平台项目中：
- [`/src/Client/AI.Boilerplate.Client.Maui/Components/Pages/AboutPage.razor`](/src/Client/AI.Boilerplate.Client.Maui/Components/Pages/AboutPage.razor)
- [`/src/Client/AI.Boilerplate.Client.Windows/Components/Pages/AboutPage.razor`](/src/Client/AI.Boilerplate.Client.Windows/Components/Pages/AboutPage.razor)
- [`/src/Client/AI.Boilerplate.Client.Web/Components/Pages/AboutPage.razor`](/src/Client/AI.Boilerplate.Client.Web/Components/Pages/AboutPage.razor)

**这种方法的优势：**
- ✅ 无需 DI 或接口即可直接访问原生平台功能
- ✅ 无需条件编译即可实现特定于平台的实现
- ✅ 代码更简洁——无需抽象层

**Maui 项目中 `AboutPage.razor` 的示例：**

```xml
@attribute [Route(PageUrls.About)]
@inherits AppPageBase

<section>
    <BitStack AutoWidth>
        <BitText>应用名称：<b>@appName</b></BitText>
        <BitText>应用版本：<b>@appVersion</b></BitText>
        <BitText>操作系统：<b>@platform</b></BitText>
        <BitText>制造商：<b>@oem</b></BitText>
    </BitStack>
</section>
```

代码隐藏文件可以直接访问 MAUI API，而无需接口：
```csharp
// 直接访问 MAUI 功能
var appName = AppInfo.Current.Name;
var platform = DeviceInfo.Current.Platform;
```

#### 平台项目中的 SCSS 支持

所有平台项目（Maui, Windows, Web）都配置了 SCSS 编译支持。请检查 `.csproj` 文件：

**在 `AI.Boilerplate.Client.Maui.csproj`, `AI.Boilerplate.Client.Windows.csproj`, 和 `AI.Boilerplate.Client.Web.csproj` 中：**

```xml
<Target Name="BeforeBuildTasks" AfterTargets="CoreCompile">
    <CallTarget Targets="BuildCssFiles" />
</Target>

<Target Name="BuildCssFiles">
    <Exec Command="../AI.Boilerplate.Client.Core/node_modules/.bin/sass Components:Components" />
</Target>
```

这意味着您可以在这些项目的任何一个中创建 `AboutPage.razor.scss`，它将自动被编译并作用于该组件。

---

## 2. SCSS 样式架构

### 2.1 隔离的组件样式

每个组件的 `.razor.scss` 文件创建**隔离的样式**，仅应用于该特定组件。这防止了样式冲突，使组件更易于维护。

**工作原理：**
- 构建期间，SCSS 文件被编译为 CSS
- Blazor 应用唯一标识符以确保样式作用域化
- 样式不会意外影响其他组件

### 2.2 全局样式：`app.scss`

**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Styles/app.scss`](/src/Client/AI.Boilerplate.Client.Core/Styles/app.scss)

这是应用于整个应用程序的**主全局样式表**：

```scss
@import '../Styles/abstracts/_media-queries.scss';
@import '../Styles/abstracts/_bit-css-variables.scss';

* {
    box-sizing: border-box;
    font-family: "Segoe UI";
}

html, body, #app-container {
    margin: 0;
    padding: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
}

h1, h2, h3, h4, h5 {
    margin: 0;
}
```

**全局样式的用例：**
- CSS 重置和规范化
- 全局排版设置
- 整个应用中使用的工具类
- 基础 HTML 元素样式

### 2.3 主题颜色变量：`_bit-css-variables.scss`

**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Styles/abstracts/_bit-css-variables.scss`](/src/Client/AI.Boilerplate.Client.Core/Styles/abstracts/_bit-css-variables.scss)

此文件提供**SCSS 变量**，这些变量映射到 Bit.BlazorUI 主题系统中的 CSS 自定义属性。这些变量自动支持深色和浅色模式。

**变量示例：**

```scss
/*-------- 颜色 --------*/
// 主色
$bit-color-primary: var(--bit-clr-pri);
$bit-color-primary-hover: var(--bit-clr-pri-hover);
$bit-color-primary-active: var(--bit-clr-pri-active);

// 背景色
$bit-color-background-primary: var(--bit-clr-bg-pri);
$bit-color-background-secondary: var(--bit-clr-bg-sec);
$bit-color-background-tertiary: var(--bit-clr-bg-ter);

// 前景色 (文本颜色)
$bit-color-foreground-primary: var(--bit-clr-fg-pri);
$bit-color-foreground-secondary: var(--bit-clr-fg-sec);
$bit-color-foreground-tertiary: var(--bit-clr-fg-ter);

// 边框色
$bit-color-border-primary: var(--bit-clr-brd-pri);
$bit-color-border-secondary: var(--bit-clr-brd-sec);
$bit-color-border-tertiary: var(--bit-clr-brd-ter);

// 语义颜色
$bit-color-success: var(--bit-clr-suc);
$bit-color-warning: var(--bit-clr-wrn);
$bit-color-error: var(--bit-clr-err);
$bit-color-info: var(--bit-clr-inf);

// 环境变量
$bit-env-height-available: var(--bit-env-height-avl);
$bit-env-width-available: var(--bit-env-width-avl);
```

**⚠️ 关键：始终使用主题变量**

您**必须**在 C#、Razor 和 SCSS 文件中使用这些主题变量，而不是硬编码颜色。这确保了：
- ✅ 自动支持深色/浅色模式
- ✅ 整个应用的设计一致性
- ✅ 轻松的主题自定义

**来自 `CategoriesPage.razor.scss` 的示例：**

```scss
::deep {
    .categories-grid {
        background-color: $bit-color-background-secondary;  // ✅ 正确 - 适应主题

        thead {
            background-color: $bit-color-background-tertiary;  // ✅ 正确
        }

        td {
            border-bottom: 1px solid $bit-color-border-tertiary;  // ✅ 正确
        }
    }
}
```

**❌ 不要这样做：**

```scss
.my-component {
    background-color: #ffffff;  // ❌ 错误 - 硬编码，破坏深色模式
    color: black;               // ❌ 错误 - 无法适应主题
}
```

### 2.4 `::deep` 选择器

`::deep` 选择器（也称为深度选择器或 `>>>`）允许您从父组件的作用域样式表中**样式化子组件**。

**为什么需要它？**

默认情况下，组件样式是作用域化的，不会影响子组件。当您使用 Bit.BlazorUI 组件（如 `BitDataGrid`, `BitButton` 等）时，您需要 `::deep` 来样式化它们的内部元素。

**来自 `ProductsPage.razor.scss` 的示例：**

```scss
// 如果没有 ::deep，这将只样式化 ProductsPage 中的直接元素
::deep {
    .products-grid {
        // 这些样式现在可以深入到 BitDataGrid 的内部结构
        width: 100%;
        height: 100%;
        background-color: $bit-color-background-secondary;

        .name-col {
            padding-inline-start: 16px;
        }

        thead {
            height: 44px;
            background-color: $bit-color-background-tertiary;
        }

        td {
            height: 44px;
            white-space: nowrap;
            border-bottom: 1px solid $bit-color-border-tertiary;
        }
    }

    .bitdatagrid-paginator {
        padding: 8px;
        font-size: 14px;
        background-color: $bit-color-background-secondary;

        button {
            cursor: pointer;
            font-size: 12px;
        }
    }
}
```

**用例：**
- 样式化 Bit.BlazorUI 和其他第三方 UI 库组件（它们是子组件）

---

## 3. Bit.BlazorUI 组件与文档

### 使用 Bit.BlazorUI 组件

本项目使用 **`Bit.BlazorUI`** 作为主要的 UI 组件库。您**必须**使用这些组件而不是通用的 HTML 元素，以确保 UI 一致性并利用内置功能。

**项目中的示例：**

```xml
<!-- ✅ 使用 Bit.BlazorUI 组件 -->
<BitButton OnClick="WrapHandled(CreateProduct)">
    @Localizer[nameof(AppStrings.AddProduct)]
</BitButton>

<BitSearchBox OnSearch="HandleOnSearch" />

<BitDataGrid TGridItem="ProductDto" ItemsProvider="productsProvider" />

<!-- ❌ 当存在 Bit 组件时，避免使用通用 HTML -->
<button>Add Product</button>
<input type="text" />
```

### 组件特定的样式属性

**重要提示：** 每个 Bit.BlazorUI 组件都有其自己的 **CSS 变量** 和**样式参数**（`Styles` 和 `Classes` 属性），允许您样式化嵌套的子元素。

**来自 `SignOutConfirmDialog.razor` 的使用 `Styles` 参数的示例：**
```xml
<BitDialog @bind-IsOpen="isSignOutDialogOpen"
           Styles="@(new() { OkButton = "width:100%", CancelButton = "width:100%" })" />
```

**来自 `AppMenu.razor` 的使用 `Classes` 参数的示例：**
```xml
<BitCallout @bind-IsOpen="isMenuOpen"
            Classes="@(new() { Callout = "app-menu-callout" })">
    <!-- 内容 -->
</BitCallout>
```

**为什么采用这种方法？**
- ✅ 更明确且类型安全
- ✅ 对嵌套元素名称提供 IntelliSense 支持
- ✅ 使用 `Styles` 参数时无需 `::deep` 选择器
- ✅ 样式值可以绑定到 C# 变量

### 全面的文档

**`Bit.BlazorUI` 拥有广泛的文档，地址为：**
📚 **https://blazorui.bitplatform.dev**

文档包括：
- 每个组件的完整 API 参考
- 交互式示例和演示
- 属性描述
- 使用模式
- 样式指南

### 自动 DeepWiki 集成

**您无需手动搜索文档！**

当您在 **GitHub Copilot Chat** 中提问或发出与 UI 组件相关的命令时，系统会**自动查询 `bitfoundation/bitplatform` 的 DeepWiki 知识库**以查找相关信息。

**交互示例：**

- **您问：** "如何向 BitDataGrid 添加过滤器？"
  - **Copilot：** 自动搜索 DeepWiki 并提供带有代码示例的答案

- **您问：** "如何自定义 BitButton 的颜色？"
  - **Copilot：** 检索关于 `BitColor` 枚举和样式选项的信息

- **您命令：** "添加一个带验证的 BitDatePicker"
  - **Copilot：** 找到正确的实现模式并创建代码

**您可以自然地提问：**
- "如何让 BitModal 全屏显示？"
- "给我看 BitDataGrid 分页示例"
- "如何向 BitNavMenu 项添加图标？"
- "BitChart 有哪些属性？"
- "如果我熟悉 Bootstrap 网格系统，如何使用 BitGrid 和 BitStack 组件实现网格系统和布局？"

DeepWiki 系统会自动处理文档查找！

---

## 4. 使用 PageUrls 进行导航

### PageUrls 类

**位置**: [`/src/Shared/PageUrls.cs`](/src/Shared/PageUrls.cs)

本项目使用**集中的 `PageUrls` 类**将所有路由路径定义为常量。这防止了拼写错误，使路由更改更易于管理。

```csharp
namespace AI.Boilerplate.Shared;

public static partial class PageUrls
{
    public const string Home = "/";
    public const string NotFound = "/not-found";
    public const string Terms = "/terms";
    public const string Settings = "/settings";
    public const string About = "/about";
}
```

**其他部分文件：**
- `PageUrls.Identity.cs` - 身份相关路由（登录、注册等）
- `PageUrls.SettingsSections.cs` - 设置部分路由

### 使用 PageUrls

**在 Razor 文件中（路由）：**

```xml
@attribute [Route(PageUrls.Products)]
@attribute [Route("{culture?}" + PageUrls.Products)]
```

**在 C# 代码中（导航）：**

```csharp
private async Task CreateProduct()
{
    NavigationManager.NavigateTo(PageUrls.AddOrEditProduct);
}

// 带参数
NavigationManager.NavigateTo($"{PageUrls.AddOrEditProduct}/{product.Id}");
```

**在 Razor 标记中（链接）：**

```xml
<BitButton Href="@($"{PageUrls.AddOrEditProduct}/{product.Id}")" />

<BitNavLink Href="@PageUrls.Dashboard">
    Dashboard
</BitNavLink>
```

**优势：**
- ✅ 无魔术字符串 - 编译时安全
- ✅ IntelliSense 支持
- ✅ 易于重构路由
- ✅ 集中式路由

---

## 5. 组件基类

本项目提供了增强的基类，为您的组件和页面添加了强大的功能。

### 5.1 AppComponentBase

**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/AppComponentBase.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/AppComponentBase.cs)

这是**所有组件的基类**。大多数 `.razor.cs` 文件都继承自此类。

**提供的关键功能：**

```csharp
public partial class AppComponentBase
{
    [AutoInject] protected IJSRuntime JSRuntime = default!;
    [AutoInject] protected NavigationManager NavigationManager = default!;
    [AutoInject] protected IStringLocalizer<AppStrings> Localizer = default!;
    [AutoInject] protected IExceptionHandler ExceptionHandler = default!;

    protected CancellationToken CurrentCancellationToken { get; }

    protected bool InPrerenderSession { get; }

    protected virtual Task OnInitAsync() { }
    protected virtual Task OnParamsSetAsync() { }
    protected virtual Task OnAfterFirstRenderAsync() { }
}
```

**主要优势：**

1. **自动异常处理**：增强的生命周期方法捕获并处理异常：
   - `OnInitAsync()` 替代 `OnInitializedAsync()`
   - `OnParamsSetAsync()` 替代 `OnParametersSetAsync()`
   - `OnAfterFirstRenderAsync()` - 仅在第一次渲染时触发，而非每次渲染

2. **预注入服务**：所有组件自动访问常用服务，无需手动注入：
   ```csharp
   // ✅ 在任何继承自 AppComponentBase 的组件中可用
   protected override async Task OnInitAsync()
   {
       var userName = await StorageService.GetItem("username");
       var message = Localizer[nameof(AppStrings.Welcome)];
       NavigationManager.NavigateTo(PageUrls.Dashboard);
   }
   ```

3. **自动取消令牌**：对所有异步操作使用 `CurrentCancellationToken`。当用户导航离开时，它会自动取消。

### 5.2 AppPageBase

**位置**: [`/src/Client/AI.Boilerplate.Client.Core/Components/Pages/AppPageBase.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/AppPageBase.cs)

这是**页面的基类**（扩展了 `AppComponentBase` 并添加了页面特定功能）。

```csharp
public abstract partial class AppPageBase : AppComponentBase
{
    [Parameter] public string? culture { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (string.IsNullOrEmpty(culture) is false)
            {
                // 验证 culture 参数
            }
        }
    }
}
```

**额外功能：**
- 带有自动验证的文化/本地化支持
- 通过 `AppPageData` 组件进行页面级元数据配置
- 来自 `AppComponentBase` 的所有功能

**页面中的使用示例：**

```xml
@attribute [Route(PageUrls.Products)]
@inherits AppPageBase

<AppPageData Title="@Localizer[nameof(AppStrings.Products)]"
             PageTitle="@Localizer[nameof(AppStrings.ProductsPageTitle)]" />

<!-- 页面内容 -->
```

```csharp
public partial class ProductsPage
{
    [AutoInject] IProductController productController = default!;

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        PrepareGridDataProvider();
    }

    private async Task DeleteProduct()
    {
        await productController.Delete(deletingProduct.Id, 
            deletingProduct.Version, 
            CurrentCancellationToken);
    }
}
```

---

### AI Wiki: 已回答的问题
* [如果我熟悉 Bootstrap 网格系统，如何使用 `BitGrid` 和 `BitStack` 组件实现 `网格系统` 和布局？](https://deepwiki.com/search/how-can-i-implement-a-grid-sys_25d76f3c-d0a6-4c75-8b9c-7f86ae317fb6)
* [结合 `Skeleton UI` 或 `Shimmer` 使用 `StateHasChanged` 加载页面数据的最佳方式是什么？](https://deepwiki.com/search/what-is-the-optimal-way-to-loa_e9b729ca-d36b-4c61-a855-7d21ceb783ae)
* [SCSS 如何在 Visual Studio 和 Visual Studio Code 中实时编译为 CSS？](https://deepwiki.com/search/how-is-scss-compiled-to-css-in_d4ea9c05-f002-4300-99df-076c167993d5)

在此处提出您自己的问题：[https://wiki.bitplatform.dev](https://wiki.bitplatform.dev)

---
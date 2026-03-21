# 第三阶段：API 控制器与 OData

欢迎来到第三阶段！在本阶段，您将学习本项目如何实现 API 控制器、如何利用 OData 提供强大的查询功能，以及遵循哪些最佳实践来构建高效、安全且可扩展的 API。

本阶段将全面探索后端 API 架构，通过实际项目代码库中的真实示例进行演示，并提供可点击、可导航的文件引用。

---

## 目录

1. [控制器架构](#controller-architecture)
2. [使用 [AutoInject] 进行依赖注入](#dependency-injection-with-autoinject)
3. [使用 IQueryable 读取数据](#reading-data-with-iqueryable)
4. [支持 OData 查询选项](#odata-query-options-support)
5. [用于总数的 PagedResponse](#PagedResponse-for-total-count)
6. [数据安全与权限](#data-security-and-permissions)
7. [实时演示](#live-demos)
8. [性能优化](#performance-optimization)
9. [真实使用示例](#real-usage-examples)
10. [代理接口模式](#proxy-interface-pattern)
11. [架构理念](#architectural-philosophy)

---

## 控制器架构

本项目中的所有 API 控制器都继承自 `AppControllerBase`，其位置如下：

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Infrastructure/Controllers/AppControllerBase.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Controllers/AppControllerBase.cs)

```csharp
namespace AI.Boilerplate.Server.Api.Controllers;

public partial class AppControllerBase : ControllerBase
{
    [AutoInject] protected ServerApiSettings AppSettings = default!;

    [AutoInject] protected AppDbContext DbContext = default!;

    [AutoInject] protected IStringLocalizer<AppStrings> Localizer = default!;
}
```

---

## 使用 [AutoInject] 进行依赖注入

本项目最强大的功能之一是 `[AutoInject]` 属性，它简化了依赖注入过程。

### 传统构造函数 vs 主构造函数 vs [AutoInject]

**传统构造函数**:

```csharp
public class ProductController : AppControllerBase
{
    private readonly AppDbContext dbContext;
    private readonly IStringLocalizer<AppStrings> localizer;
    private readonly ServerApiSettings settings;
    private readonly HtmlSanitizer htmlSanitizer;
    
    public ProductController(
        AppDbContext dbContext,
        IStringLocalizer<AppStrings> localizer,
        ServerApiSettings settings,
        HtmlSanitizer htmlSanitizer): base(dbContext, localizer, settings)
    {
        this.dbContext = dbContext;
        this.localizer = localizer;
        this.settings = settings;
        this.htmlSanitizer = htmlSanitizer;
    }
}
```

**主构造函数 (Primary Constructor)**:

```csharp
public class ProductController(
        AppDbContext dbContext,
        IStringLocalizer<AppStrings> localizer,
        ServerApiSettings settings,
        HtmlSanitizer htmlSanitizer) : AppControllerBase(dbContext, localizer, settings)
{
}
```

**AutoInject**:

```csharp
public partial class ProductController : AppControllerBase
{
    [AutoInject] private HtmlSanitizer htmlSanitizer = default!;
}
```

### [AutoInject] 的主要优势

1. **无需重复的构造函数代码**：您不需要编写冗长的构造函数。
2. **自动继承**：基类中已注入的依赖项（如 `AppControllerBase` 中的 `DbContext`、`Localizer`、`AppSettings`）会自动在派生类中可用，无需重新声明。
3. **代码更简洁**：减少样板代码，更专注于业务逻辑。

### CategoryController 示例

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs)

```csharp
public partial class CategoryController : AppControllerBase, ICategoryController
{
    [AutoInject] private IHubContext<AppHub> appHubContext = default!;

    [HttpGet, EnableQuery]
    public IQueryable<CategoryDto> Get()
    {
        // DbContext 和 Localizer 继承自 AppControllerBase
        return DbContext.Categories.Project();
    }
}
```

注意控制器如何直接使用 `DbContext` 和 `Localizer` 而无需显式注入——它们是从 `AppControllerBase` 继承而来的。

---

## 使用 IQueryable 读取数据

本项目推荐的读取数据模式是从 API 端点**返回 `IQueryable<DTO>`**。

### 为什么使用 IQueryable？

当您返回 `IQueryable<T>` 并结合 `[EnableQuery]` 属性时，您可以启用：

- **过滤**：使用 `$filter`
- **排序**：使用 `$orderby`
- **分页**：使用 `$top` 和 `$skip`
- **选择特定属性/列**：使用 `$select`
- **展开（包含）相关实体**：使用 `$expand`

这意味着客户端可以灵活地查询 API，而无需为每种过滤/排序组合创建新的端点。

### 示例：简单的 Get 方法

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs)

```csharp
[HttpGet, EnableQuery]
public IQueryable<CategoryDto> Get()
{
    return DbContext.Categories
        .Project(); // 使用 Mapperly 进行高效投影
}
```

`[EnableQuery]` 属性会自动处理请求 URL 中的 OData 查询参数，并在执行前将其应用于 `IQueryable`。

---

## 支持 OData 查询选项

本项目支持大多数 OData 查询选项，允许客户端创建强大、灵活的查询。

### 支持的 OData 查询选项

✅ **支持**：
- `$top` - 限制结果数量
- `$skip` - 跳过一定数量的结果（分页）
- `$filter` - 根据条件过滤结果
- `$orderby` - 对结果排序
- `$select` - 选择特定字段
- `$expand` - 包含相关实体
- `$search` - 全文搜索

❌ **尚不支持**：
- `$count` - 作为单独查询获取总数

### OData 查询示例

以下是客户端如何查询您的 API 的实际示例：

#### 1. Top (限制结果)
```
GET /api/Category/Get?$top=5
```
仅返回前 5 个类别。

#### 2. Skip (分页)
```
GET /api/Category/Get?$skip=10&$top=10
```
返回 10 个类别，从第 11 个类别开始（适用于每页 10 项的第 2 页）。

#### 3. Filter (条件查询)
```
GET /api/Product/Get?$filter=Price gt 100
```
返回价格大于 100 的产品。

**复杂过滤示例**：
```
GET /api/Product/Get?$filter=contains(tolower(Name),'phone') and Price lt 500
```
返回名称中包含 "phone" 且价格小于 500 的产品。

#### 4. OrderBy (排序)
```
GET /api/Product/Get?$orderby=Name desc
```
返回按名称降序排列的产品。

**多字段排序**：
```
GET /api/Product/Get?$orderby=CategoryName asc, Price desc
```

#### 5. 组合查询
```
GET /api/Product/Get?$filter=Price gt 50&$orderby=Name&$top=20&$skip=0
```
返回前 20 个价格大于 50 的产品，并按名称排序。

### OData 查询构建

本项目包含一个 `ODataQuery` 辅助类，用于以编程方式构建查询。

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/ODataQuery.cs`](/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Services/ODataQuery.cs)

```csharp
public partial class ODataQuery
{
    public int? Top { get; set; }
    public int? Skip { get; set; }
    public string? Filter { get; set; }
    public string? OrderBy { get; set; }
    public string? Select { get; set; }
    public string? Expand { get; set; }
    public string? Search { get; set; }
    
    public override string? ToString()
    {
        // 构建查询字符串，如 "$top=10&$skip=0&$filter=..."
    }
}
```

### 真实的客户端使用示例

**文件**: [`src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Products/ProductsPage.razor.cs)

```csharp
private void PrepareGridDataProvider()
{
    productsProvider = async req =>
    {
        var query = new ODataQuery
        {
            Top = req.Count ?? 10,
            Skip = req.StartIndex,
            OrderBy = string.Join(", ", req.GetSortByProperties()
                .Select(p => $"{p.PropertyName} {(p.Direction == BitDataGridSortDirection.Ascending ? "asc" : "desc")}"))
        };

        if (string.IsNullOrEmpty(ProductNameFilter) is false)
        {
            query.Filter = $"contains(tolower({nameof(ProductDto.Name)}),'{ProductNameFilter.ToLower()}')";
        }

        if (string.IsNullOrEmpty(CategoryNameFilter) is false)
        {
            query.AndFilter = $"contains(tolower({nameof(ProductDto.CategoryName)}),'{CategoryNameFilter.ToLower()}')";
        }

        var queriedRequest = productController.WithQuery(query.ToString());
        var data = await queriedRequest.GetProducts(req.CancellationToken);

        return BitDataGridItemsProviderResult.From(data!.Items!, (int)data!.TotalCount);
    };
}
```

此代码：
1. 创建一个包含分页、排序和过滤的 `ODataQuery` 对象
2. 使用 `WithQuery()` 将其应用于 API 请求
3. 获取包含数据和总计数量的 `PagedResponse`

---

## 用于总数量的 PagedResponse

当客户端（如数据网格）需要同时知道**页面数据**和记录的**总数量**时，请使用 `PagedResponse<T>`。

### PagedResponse 类

**文件**: [`src/Shared/Infrastructure/Dtos/PagedResponse.cs`](/src/Shared/Infrastructure/Dtos/PagedResponse.cs)

```csharp
public partial class PagedResponse<T>
{
    public T[] Items { get; set; } = [];
    public long TotalCount { get; set; }

    [JsonConstructor]
    public PagedResponse(T[] items, long totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }
}
```

### 为什么使用 PagedResponse？

如果没有总数量，客户端将无法知道：
- 共有多少页
- 是否显示“下一页”按钮
- 进度指示器，如“显示 250 项中的第 10 项”

### 服务器端实现

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs)

```csharp
[HttpGet]
public async Task<PagedResponse<CategoryDto>> GetCategories(
    ODataQueryOptions<CategoryDto> odataQuery, 
    CancellationToken cancellationToken)
{
    // 应用过滤和排序，但暂时不应用 $top/$skip
    var query = (IQueryable<CategoryDto>)odataQuery.ApplyTo(
        Get(), 
        ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

    // 在分页前获取总数量
    var totalCount = await query.LongCountAsync(cancellationToken);

    // 现在应用分页
    query = query.SkipIf(odataQuery.Skip is not null, odataQuery.Skip?.Value)
                 .TakeIf(odataQuery.Top is not null, odataQuery.Top?.Value);

    // 返回数据和数量
    return new PagedResponse<CategoryDto>(
        await query.ToArrayAsync(cancellationToken), 
        totalCount);
}
```

### 关键步骤：
1. 应用过滤/排序，但忽略 `$top`/`$skip`
2. 统计过滤后的结果总数
3. 应用分页
4. 在 `PagedResponse` 中返回数据 + 数量

---

## 数据安全与权限

**重要**：客户端只能接收其有权访问的数据。

### 授权示例

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs)

```csharp
[ApiController, Route("api/[controller]/[action]"),
    Authorize(Policy = AuthPolicies.PRIVILEGED_ACCESS),
    Authorize(Policy = AppFeatures.AdminPanel.ManageProductCatalog)]
public partial class CategoryController : AppControllerBase, ICategoryController
{
    // 所有方法都需要 PRIVILEGED_ACCESS 和 ManageProductCatalog 权限
}
```

### 安全保障

即使恶意客户端尝试操纵 OData 查询参数，他们也**无法**：
- 访问无权查看的数据
- 绕过服务器端授权检查
- 执行任意数据库查询
- 访问软删除或隐藏的记录

### 工作原理

1. **授权发生在 OData 处理之前**：首先检查 `[Authorize]` 属性。
2. **查询范围限定在用户权限内**：您可以在控制器方法中添加特定于用户的过滤器。
3. **Entity Framework 防止 SQL 注入**：OData 参数被安全地转换为 SQL。

### 示例：特定于用户的数据

```csharp
[HttpGet, EnableQuery]
public IQueryable<TodoItemDto> GetMyTodos()
{
    var userId = User.GetUserId(); // 获取当前用户 ID
    
    return DbContext.TodoItems
        .Where(t => t.UserId == userId) // 仅返回当前用户的待办事项
        .Project();
}
```

无论客户端发送什么 OData 参数，他们只能看到自己的待办事项。

---

## 实时演示

### 演示 1：带 OData 的管理面板

访问实时管理面板查看 OData 的实际效果：

🔗 **[https://adminpanel.bitplatform.dev/categories](https://adminpanel.bitplatform.dev/categories)**

此演示展示：
- 按类别名称实时过滤
- 点击列标题进行排序
- 带有页面大小选择的分页
- 全部由 OData 查询驱动

### 演示 2：直接 API 查询

您也可以直接调用 API 查看 OData 查询字符串：

🔗 **[https://sales.bitplatform.dev/api/ProductView/Get?$top=10&$skip=10&$orderby=Name](https://sales.bitplatform.dev/api/ProductView/Get?$top=10&$skip=10&$orderby=Name)**

此 URL：
- 返回产品的第二页 (`$skip=10`)
- 每页显示 10 个产品 (`$top=10`)
- 按产品名称排序 (`$orderby=Name`)

---

## 性能优化

`IQueryable`、Entity Framework Core 和 OData 的组合提供了卓越的性能特征。

### 主要性能优势

#### 1. 直接 SQL 转换
当您使用带有 `[EnableQuery]` 的 `IQueryable` 时，OData 参数会直接转换为 SQL：

```csharp
// 控制器代码
[HttpGet, EnableQuery]
public IQueryable<ProductDto> Get()
{
    return DbContext.Products.Project();
}

// 客户端请求
GET /api/Product/Get?$top=10&$skip=10&$orderby=Name

// 生成的 SQL (简化版)
SELECT TOP 10 p.Id, p.Name, p.Price, c.Name as CategoryName
FROM Products p
INNER JOIN Categories c ON p.CategoryId = c.Id
ORDER BY p.Name
OFFSET 10 ROWS
```

#### 2. 最小内存消耗
- 仅加载请求的 10 个产品到内存中
- **不是**加载所有产品然后在 C# 中过滤/排序
- 投影在数据库级别使用 `Project()` 完成

#### 3. 无过度获取 (Over-Fetching)
- 仅获取 DTO 属性，而非整个实体
- 相关实体被高效连接

### 示例：高效的产品查询

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductController.cs)

```csharp
[HttpGet, EnableQuery]
public IQueryable<ProductDto> Get()
{
    return DbContext.Products
        .Project(); // Mapperly 投影
}
```

**当客户端请求时发生的情况：**
```
GET /api/Product/Get?$top=10&$skip=20&$orderby=Price desc&$filter=Price gt 50
```

1. 生成带有 `WHERE Price > 50` 的 SQL 查询
2. 结果在数据库中按 `Price DESC` 排序
3. 数据库跳过 20 行并返回 10 行
4. 仅选择 DTO 属性（而非完整实体）
5. 总内存使用量：约 10 个 DTO 对象

### 可扩展性

即使有**数百万条记录**，此模式也能高效扩展：
- 数据库处理过滤和排序
- 仅通过网络传输请求的页面
- 客户端仅接收所需数据

**示例**：如果您数据库中有 100 万个产品：
- 获取第 1,000 页（第 10,000-10,010 行）与获取第 1 页一样快
- 无论数据库大小如何，内存消耗都是恒定的
- 网络流量是恒定的（每页仅 10 个产品）

---

## 真实使用示例

让我们检查项目中的真实控制器，看看这些模式的实际应用。

### 示例 1：CategoryController

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs)

此控制器展示了：

#### 简单的 IQueryable GET
```csharp
[HttpGet, EnableQuery]
public IQueryable<CategoryDto> Get()
{
    return DbContext.Categories.Project();
}
```

#### 带有总数量的 PagedResponse
```csharp
[HttpGet]
public async Task<PagedResponse<CategoryDto>> GetCategories(
    ODataQueryOptions<CategoryDto> odataQuery, 
    CancellationToken cancellationToken)
{
    var query = (IQueryable<CategoryDto>)odataQuery.ApplyTo(
        Get(), 
        ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

    var totalCount = await query.LongCountAsync(cancellationToken);

    query = query.SkipIf(odataQuery.Skip is not null, odataQuery.Skip?.Value)
                 .TakeIf(odataQuery.Top is not null, odataQuery.Top?.Value);

    return new PagedResponse<CategoryDto>(
        await query.ToArrayAsync(cancellationToken), 
        totalCount);
}
```

#### 带有错误处理的单项 GET
```csharp
[HttpGet("{id}")]
public async Task<CategoryDto> Get(Guid id, CancellationToken cancellationToken)
{
    var dto = await Get().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    if (dto is null)
        throw new ResourceNotFoundException(
            Localizer[nameof(AppStrings.CategoryCouldNotBeFound)]);

    return dto;
}
```

#### 带有验证的创建
```csharp
[HttpPost]
public async Task<CategoryDto> Create(CategoryDto dto, CancellationToken cancellationToken)
{
    var entityToAdd = dto.Map(); // Mapperly

    await DbContext.Categories.AddAsync(entityToAdd, cancellationToken);

    await Validate(entityToAdd, cancellationToken);

    await DbContext.SaveChangesAsync(cancellationToken);

    await PublishDashboardDataChanged(cancellationToken);

    return entityToAdd.Map();
}
```

#### 带有并发检查的更新
```csharp
[HttpPut]
public async Task<CategoryDto> Update(CategoryDto dto, CancellationToken cancellationToken)
{
    var entityToUpdate = await DbContext.Categories.FindAsync([dto.Id], cancellationToken)
        ?? throw new ResourceNotFoundException(
            Localizer[nameof(AppStrings.CategoryCouldNotBeFound)]);

    dto.Patch(entityToUpdate); // Mapperly 部分更新

    await Validate(entityToUpdate, cancellationToken);

    await DbContext.SaveChangesAsync(cancellationToken);

    await PublishDashboardDataChanged(cancellationToken);

    return entityToUpdate.Map();
}
```

#### 带有业务逻辑验证的删除
```csharp
[HttpDelete("{id}/{version}")]
public async Task Delete(Guid id, string version, CancellationToken cancellationToken)
{
    // 业务规则：如果类别包含产品，则不能删除
    if (await DbContext.Products.AnyAsync(p => p.CategoryId == id, cancellationToken))
    {
        throw new BadRequestException(Localizer[nameof(AppStrings.CategoryNotEmpty)]);
    }

    DbContext.Categories.Remove(new() 
    { 
        Id = id, 
        Version = Convert.FromHexString(version) 
    });

    await DbContext.SaveChangesAsync(cancellationToken);

    await PublishDashboardDataChanged(cancellationToken);
}
```

#### 远程验证
```csharp
private async Task Validate(Category category, CancellationToken cancellationToken)
{
    var entry = DbContext.Entry(category);
    
    // 检查重复的类别名称
    if ((entry.State is EntityState.Added || entry.Property(c => c.Name).IsModified)
        && await DbContext.Categories.AnyAsync(p => p.Name == category.Name, cancellationToken))
        throw new ResourceValidationException(
            (nameof(CategoryDto.Name), 
             [Localizer[nameof(AppStrings.DuplicateCategoryName), category.Name!]]));
}
```

### 示例 2：ProductController

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductController.cs)

此控制器展示了更多高级功能：

#### 语义搜索集成
```csharp
[HttpGet("{searchQuery}")]
public async Task<PagedResponse<ProductDto>> SearchProducts(
    string searchQuery, 
    ODataQueryOptions<ProductDto> odataQuery, 
    CancellationToken cancellationToken)
{
    // 使用嵌入服务进行 AI 驱动的搜索
    var searchResults = await productEmbeddingService.SearchProducts(searchQuery, cancellationToken);
    
    var query = (IQueryable<ProductDto>)odataQuery.ApplyTo(
        searchResults.Project(),
        ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.OrderBy);
        
    var totalCount = await query.LongCountAsync(cancellationToken);

    query = query.SkipIf(odataQuery.Skip is not null, odataQuery.Skip?.Value)
                 .TakeIf(odataQuery.Top is not null, odataQuery.Top?.Value);

    return new PagedResponse<ProductDto>(
        await query.ToArrayAsync(cancellationToken), 
        totalCount);
}
```

#### HTML 清理
```csharp
[AutoInject] private HtmlSanitizer htmlSanitizer = default!;

[HttpPost]
public async Task<ProductDto> Create(ProductDto dto, CancellationToken cancellationToken)
{
    // 清理 HTML 以防止 XSS 攻击
    dto.DescriptionHTML = htmlSanitizer.Sanitize(dto.DescriptionHTML ?? string.Empty);

    var entityToAdd = dto.Map();
    // ... 其余创建逻辑
}
```

---

## 代理接口模式

本项目使用**强类型 HTTP 客户端包装器**模式来调用后端 API。这提供了类型安全、IntelliSense 支持，并消除了魔术字符串。

### 工作原理

该模式涉及：
1. 在 `Shared/Controllers` 中定义接口（客户端和服务器共享）
2. 在 `AI.Boilerplate.Server.Api/Controllers` 中实现接口（服务器端）
3. 客户端代码注入接口并像调用本地方法一样调用它们

### 步骤 1：定义接口

**文件**: [`src/Shared/Features/Categories/ICategoryController.cs`](/src/Shared/Features/Categories/ICategoryController.cs)

```csharp
[Route("api/[controller]/[action]/")]
[AuthorizedApi] // 需要认证
public interface ICategoryController : IAppController
{
    [HttpGet("{id}")]
    Task<CategoryDto> Get(Guid id, CancellationToken cancellationToken);

    [HttpGet]
    Task<PagedResponse<CategoryDto>> GetCategories(CancellationToken cancellationToken) => default!;

    [HttpGet]
    Task<List<CategoryDto>> Get(CancellationToken cancellationToken) => default!;

    [HttpPost]
    Task<CategoryDto> Create(CategoryDto dto, CancellationToken cancellationToken);

    [HttpPut]
    Task<CategoryDto> Update(CategoryDto dto, CancellationToken cancellationToken);

    [HttpDelete("{id}/{version}")]
    Task Delete(Guid id, string version, CancellationToken cancellationToken);
}
```

### 步骤 2：在服务器端实现

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs)

```csharp
[ApiController, Route("api/[controller]/[action]")]
public partial class CategoryController : AppControllerBase, ICategoryController
{
    [HttpGet("{id}")]
    public async Task<CategoryDto> Get(Guid id, CancellationToken cancellationToken)
    {
        // 实现
    }

    [HttpGet]
    public async Task<PagedResponse<CategoryDto>> GetCategories(
        ODataQueryOptions<CategoryDto> odataQuery, // 服务器特定参数
        CancellationToken cancellationToken)
    {
        // 实现
    }
    
    // ... 其他实现
}
```

### 步骤 3：在客户端代码中使用

```csharp
public partial class ProductsPage
{
    [AutoInject] IProductController productController = default!;

    private async Task LoadProducts()
    {
        // 具有 IntelliSense 的类型安全 API 调用
        var products = await productController.Get(CurrentCancellationToken);
        
        // 构建 OData 查询
        var query = new ODataQuery
        {
            Top = 10,
            Filter = "Price gt 50",
            OrderBy = "Name"
        };
        
        // 应用查询并调用 API
        var PagedResponse = await productController
            .WithQuery(query.ToString())
            .GetProducts(CurrentCancellationToken);
    }
}
```

### 约定优于配置

代理生成器遵循以下约定：

- **URL 约定**：`IUserController` 中的 `GetCurrentUser` 方法 → `api/User/GetCurrentUser`
- **HTTP 方法**：使用 `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` 属性
- **查询参数**：自动添加到 URL 查询字符串
- **正文参数**：复杂类型作为 JSON 发送到请求正文中

### 处理仅服务器端参数

某些服务器方法具有客户端不存在的参数（如 `ODataQueryOptions`）。

**解决方案**：在接口中使用 `=> default!`：

```csharp
public interface ICategoryController : IAppController
{
    // 服务器接受 ODataQueryOptions，但客户端不传递它
    [HttpGet]
    Task<PagedResponse<CategoryDto>> GetCategories(CancellationToken cancellationToken) => default!;
}
```

`=> default!` 告诉 C# 编译器此方法具有默认实现，从而防止构建错误。

### 此模式的优势

1. **类型安全**：API 调用的编译时检查
2. **IntelliSense**：API 方法和参数的自动补全
3. **重构**：重命名方法，所有用法自动更新
4. **无魔术字符串**：不再有 `HttpClient.GetAsync("/api/categories/get")`
5. **契约强制执行**：接口确保客户端和服务器保持同步

### 高级示例：外部 API 调用

您也可以使用此模式调用外部 API：

**文件**: [`src/Shared/Controllers/Statistics/IStatisticsController.cs`](/src/Shared/Controllers/Statistics/IStatisticsController.cs) (示例)

```csharp
public interface IStatisticsController : IAppController
{
    // 直接调用 GitHub API
    [HttpGet, Route("https://api.github.com/repos/bitfoundation/bitplatform"), ExternalApi]
    Task<GitHubStats> GetGitHubStats(CancellationToken cancellationToken) => default!;
}
```

### 更多信息

有关代理接口模式的详细信息，请参阅：

**文件**: [`src/Shared/Features/Readme.md`](/src/Shared/Features/Readme.md)

---

## 架构理念

在结束之前，了解此模板背后的架构决策非常重要。

### 后端架构：刻意简单

此模板中的后端架构（基于功能的控制器直接访问 `DbContext`）**刻意保持简单**，以帮助开发人员快速入门。

**关键点**：
- 这**不是**构建后端的“唯一正确方法”
- 不同的项目有不同的需求
- 经验丰富的开发人员可能有自己的架构偏好
- 您可以自由实施任何您喜欢的架构

### 架构选项

是否使用：
- **分层架构**（表示层 → 业务层 → 数据层）
- **CQRS**（命令查询职责分离）
- **洋葱架构**（领域为核心）
- **整洁架构**
- **垂直切片架构**
- **领域驱动设计 (DDD)**

...完全取决于您，并取决于您的项目需求和团队偏好。

### 没有万能解决方案

没有一种**“一刀切”的架构**适用于每个项目：
- 小型初创应用的需求与企业系统不同
- 读密集型系统需要的模式与写密集型系统不同
- 团队规模、经验和偏好很重要

### 真正的价值：前端架构

**此模板的真正架构价值在于前端**：为**跨平台 Blazor 应用程序**提供完整、生产就绪的架构。

这是模板的亮点所在，因为 .NET 生态系统中的 dotnet 前端架构模式尚未完全确立。

### 仍包含的后端功能

虽然架构简单，但后端仍包含许多高级功能：
- ✅ 功能齐全的 ASP.NET Core Identity 解决方案
- ✅ 基于 JWT 的身份验证及刷新令牌
- ✅ AI 集成（语义搜索、聊天机器人）
- ✅ 超优化的响应缓存系统
- ✅ 使用 Hangfire 进行后台作业处理
- ✅ SignalR 实时通信
- ✅ OData 查询支持
- ✅ 健康检查和诊断
- ✅ OpenTelemetry 集成
- ✅ 推送通知
- ✅ 本地化和多语言支持
- ✅ 以及更多...

### 总结

您可以随意以任何您认为合适的方式重构后端。该模板提供了坚实的基础和高级功能，但架构由您掌控。

---
# 搭建完整的实体 CRUD 功能

你是一位专家，擅长为项目搭建完整的实体实现方案。

## 指令

为一个实体生成完整的 CRUD（增删改查）实现，包括以下内容：
1.  **实体模型 (Entity Model)**
2.  **实体类型配置** (Entity Framework Core)
3.  **DbContext 注册** (DbSet)
4.  **EF Core 迁移 (Migration)**
5.  **DTO** (数据传输对象)
6.  **映射器 (Mapper)** (使用 Mapperly)
7.  **API 控制器**
8.  **IAppController 接口** (强类型 HTTP 客户端)
9.  **资源字符串** (AppStrings.resx)
10. **数据网格页面 (Data Grid Page)**
11. **添加/编辑页面**
12. **集成工作**：更新 `PageUrls.cs`、`NavBar.razor` 和 `MainLayout.razor.items.cs`

### 实体 (Model)
-   **位置**: `src/Server/Boilerplate.Server.Api/Features/{FeatureName}/`
-   **文件**: `{EntityName}.cs`
-   **要求**:
    -   包含 `Id` 和 `Version` 属性。
    -   添加适当的导航属性。
    -   使用可空引用类型 (Nullable Reference Types)。
    -   根据需要添加数据注解 (Data Annotations)。

### 实体配置、AppDbContext DbSet 和迁移
-   **位置**: `src/Server/Boilerplate.Server.Api/Features/{FeatureName}/`
-   **文件**:
    -   `{EntityName}Configuration.cs` - 实现 `IEntityTypeConfiguration<{EntityName}>` 接口。
    -   配置唯一索引和关系。
    -   通过 `modelBuilder.ApplyConfigurationsFromAssembly()` 在 `AppDbContext` 中自动注册。
-   **迁移**:
    -   在 `Boilerplate.Server.Api` 项目中运行命令：
        `dotnet ef migrations add {MigrationName} --output-dir Infrastructure/Data/Migrations --verbose`

### DTO
-   **位置**: `src/Shared/Boilerplate.Shared/Features/{FeatureName}/`
-   **文件**: `{EntityName}Dto.cs`
-   **要求**:
    -   使用 `[DtoResourceType(typeof(AppStrings))]` 特性。
    -   添加验证特性：`[Required]`, `[MaxLength]`, `[Display]`。
    -   错误消息和显示名称使用 `nameof(AppStrings.PropertyName)`。
    -   包含 `Id` 和 `Version` 属性。
    -   如有需要，添加计算属性（例如 `ProductsCount`）。
    -   在 `AppJsonContext.cs` 中添加 `[JsonSerializable(typeof({DtoName}))]`。

### 映射器 (Mapper)
-   **位置**: `src/Server/Boilerplate.Server.Api/Features/{FeatureName}/`
-   **文件**: `{EntityName}Mapper.cs` (如果涉及多个实体，则使用 `{FeatureName}Mapper.cs`)
-   **要求**:
    -   使用 Mapperly 的 `[Mapper]` 特性。
    -   创建 `static partial class {MapperName}Mapper`。
    -   添加投影方法：`public static partial IQueryable<{DtoName}> Project(this IQueryable<{EntityName}> query);`。
    -   添加用于 CRUD 操作的映射方法：`Map()`, `Patch()`。
    -   如果需要复杂映射，使用 `[MapProperty]`。

### API 控制器
-   **位置**: `src/Server/Boilerplate.Server.Api/Features/{FeatureName}/`
-   **文件**: `{EntityName}Controller.cs`
-   **要求**:
    -   继承自 `AppControllerBase`。
    -   实现对应的 `IAppController` 接口。
    -   添加适当的授权特性 (Authorization Attributes)。
    -   对支持 OData 的 GET 端点使用 `[EnableQuery]`。
    -   在私有方法中实现验证逻辑。
    -   查询和映射时使用 `Project()` 方法。
    -   使用 `ResourceNotFoundException` 处理资源未找到的情况。

### IAppController 接口
-   **位置**: `src/Shared/Boilerplate.Shared/Features/{FeatureName}/`
-   **文件**: `I{EntityName}Controller.cs`
-   **要求**:
    -   继承自 `IAppController`。
    -   添加 `[Route("api/[controller]/[action]/")]` 特性。
    -   如果需要认证，添加 `[AuthorizedApi]`。
    -   始终使用 `CancellationToken` 参数。
    -   返回类型应为 `Task<T>`，其中 T 是 JSON 可序列化类型（如 DTO, int, 或 `List<Dto>`）。
    -   如果后端 API 操作返回 `IQueryable<T>`，则接口返回类型使用 `Task<List<T>>` 并返回 `=> default!`。
    -   如果后端 API 操作返回 `IActionResult`，则使用 `Produces<T>` 特性指定响应类型，并返回 `=> default!`。
    -   如果后端 API 接受 `ODataQueryOptions`，在接口定义中直接忽略它。

### 页面

每个 Blazor 页面都遵循三文件结构：
-   `PageName.razor` - 使用 Razor 语法的 UI 标记。
-   `PageName.razor.cs` - 包含 C# 逻辑的代码后置文件。
-   `PageName.razor.scss` - 作用域样式。

**位置**: `src/Client/Boilerplate.Client.Core/Components/Pages/{FeatureName}/`

-   **网格/列表页面**: `{FeatureName}Page.razor` + `.razor.cs` + `.razor.scss`
-   **添加/编辑模态框或页面**: `AddOrEdit{EntityName}Page.razor` 或 `AddOrEdit{EntityName}Modal.razor`

使用 `_bit-css-variables.scss` 中的 SCSS 变量进行主题化：
```scss
@import '../../Styles/abstracts/_bit-css-variables.scss';
background: $bit-color-background-secondary;
color: $bit-color-primary;
```

始终使用 `WrapHandled` 处理事件处理器和生命周期方法。异常会被 `ExceptionHandler` 捕获和处理：
```razor
<BitButton OnClick="WrapHandled(SaveData)" />
<BitTextField OnEnter="WrapHandled(async (args) => await Submit())" />
```
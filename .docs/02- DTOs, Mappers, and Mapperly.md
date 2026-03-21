# 第二阶段：DTOs、映射器 (Mappers) 和 Mapperly

欢迎参加 AI.Boilerplate 项目教程的**第二阶段**！在本阶段，您将学习：

- **DTOs (数据传输对象)**：数据如何在客户端和服务器之间传输
- **AppJsonContext**：高效的 JSON 序列化配置
- **使用 Mapperly 进行映射**：用于读写数据的高性能对象映射
- **Project 与 Map**：理解何时使用每种方法
- **Patch 方法**：高效的客户端状态管理

---

## 1. 什么是 DTOs？

**DTOs (数据传输对象)** 是简单的类，代表在客户端和服务器之间发送的数据结构。它们具有多种用途：

- **解耦**：将数据库实体与 API 契约分离
- **安全性**：精确控制向客户端暴露哪些数据
- **验证**：添加验证属性以确保数据完整性
- **文档化**：自我文档化的 API 契约

### 示例：CategoryDto

让我们看看项目中的一个真实 DTO：[`CategoryDto`](/src/Shared/Features/Categories/CategoryDto.cs)

```csharp
namespace AI.Boilerplate.Shared.Dtos.Categories;

[DtoResourceType(typeof(AppStrings))]
public partial class CategoryDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Name))]
    public string? Name { get; set; }

    [Display(Name = nameof(AppStrings.Color))]
    public string? Color { get; set; }

    public int ProductsCount { get; set; }

    public long Version { get; set; }
}
```

### 本项目中 DTOs 的关键特性

1. **`[DtoResourceType(typeof(AppStrings))]`**：将 DTO 连接到本地化资源，用于多语言验证消息和显示名称。这将在后续阶段详细讨论。

2. **验证属性**：
   - `[Required]`：确保字段不为空
   - `[MaxLength]`：限制字符串属性的长度
   - `[EmailAddress]`, `[Phone]`：验证特定数据类型的格式

3. **计算属性**：`ProductsCount` 是一个计算属性，显示类别中产品的数量（不存储在数据库中）

4. **Version**：用于乐观并发控制，防止冲突更新

### 示例：UserDto

另一个例子是 [`UserDto`](/src/Shared/Features/Identity/Dtos/UserDto.cs)：

```csharp
[DtoResourceType(typeof(AppStrings))]
public partial class UserDto : IValidatableObject
{
    public Guid Id { get; set; }

    [Display(Name = nameof(AppStrings.UserName))]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = nameof(AppStrings.EmailAddressAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Email))]
    public string? Email { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(PhoneNumber))
            yield return new ValidationResult(
                errorMessage: nameof(AppStrings.EitherProvideEmailOrPhoneNumber),
                memberNames: [nameof(Email), nameof(PhoneNumber)]
            );
    }
}
```

**注意**：`UserDto` 实现了 `IValidatableObject`，用于处理无法用简单属性表达的复杂验证规则。

---

## 2. AppJsonContext - 高效的 JSON 序列化

**位置**：[`src/Shared/Infrastructure/Dtos/AppJsonContext.cs`](/src/Shared/Infrastructure/Dtos/AppJsonContext.cs)

本项目使用 **System.Text.Json 源生成器 (Source Generator)** 进行高性能 JSON 序列化。所有用于 API/SignalR 通信的 DTO 都必须在 `AppJsonContext` 中注册。

```csharp
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(CategoryDto))]
[JsonSerializable(typeof(List<CategoryDto>))]
[JsonSerializable(typeof(ProductDto))]
[JsonSerializable(typeof(List<ProductDto>))]
// ... 更多类型
public partial class AppJsonContext : JsonSerializerContext
{
}
```

### 为什么需要 AppJsonContext？

1. **性能**：源生成消除了运行时的反射开销
2. **AOT 兼容性**：对于预编译 (Ahead-of-Time) 场景是必需的
3. **类型安全**：如果不支持序列化，会在编译时报错

### 何时更新 AppJsonContext

**每当您执行以下操作时，必须添加 `[JsonSerializable]` 属性：**
- 创建新的 DTO
- 从 API 返回 DTO 的 `List<T>` 或 `PagedResponse<T>`
- 在 SignalR 通信中使用 DTO

---

## 3. 使用 Mapperly 进行映射

**Mapperly** 是一个高性能的对象映射库，它在编译时生成映射代码（无反射！）。

**更多信息**：[Server/Features/Mappers.md](/src/Server/AI.Boilerplate.Server.Api/Features/Mappers.md)

### 三个核心方法

1. **`Project()`**：将 `IQueryable<Entity>` 转换为 `IQueryable<DTO>`（用于读取数据）
2. **`Map()`**：在 `Entity` ↔ `DTO` 之间转换（用于单个对象）
3. **`Patch()`**：用另一个对象的值更新现有对象（用于更新操作）

### 示例：CategoriesMapper

**位置**：[`src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoriesMapper.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoriesMapper.cs)

```csharp
[Mapper]
public static partial class CategoriesMapper
{
    public static partial IQueryable<CategoryDto> Project(this IQueryable<Category> query);

    [MapProperty(nameof(@Category.Products.Count), nameof(@CategoryDto.ProductsCount))]
    public static partial CategoryDto Map(this Category source);
    
    public static partial Category Map(this CategoryDto source);
    
    public static partial void Patch(this CategoryDto source, Category destination);
}
```

### Category 实体

供参考，这是实体模型：[`Category.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/Category.cs)

```csharp
public partial class Category
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public long Version { get; set; }

    public IList<Product> Products { get; set; } = [];
}
```

### 理解 [MapProperty]

在上面的映射器中，请注意：

```csharp
[MapProperty(nameof(@Category.Products.Count), nameof(@CategoryDto.ProductsCount))]
```

**作用**：将 `Category.Products.Count` 映射到 `CategoryDto.ProductsCount`

**重要提示**：在此特定情况下，`[MapProperty]` 实际上**并非必需**，因为 Mapperly 会按照约定自动将 `Products.Count` 映射到 `ProductsCount`。此处包含它是为了演示如何在需要时显式映射属性。

---

## 4. Project() 与 Map() - 何时使用哪一个

### Project() - 用于高效查询

**当以下情况时使用 `Project()`：**
- 从数据库读取数据
- 您拥有 `IQueryable<Entity>` 并想要 `IQueryable<DTO>`
- 您希望生成高效的 SQL

**来自 [`CategoryController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryController.cs) 的示例：**

```csharp
[HttpGet, EnableQuery]
public IQueryable<CategoryDto> Get()
{
    return DbContext.Categories
        .Project(); // 来自 CategoriesMapper 的扩展方法
}
```

**幕后发生了什么：**

1. EF Core 将查询转换为 SQL
2. 仅选择 `CategoryDto` 所需的列
3. 数据库直接返回 DTO（内存中不创建实体）
4. 对于大型数据集极其高效

**生成的 SQL 示例：**
```sql
SELECT 
    c.Id, 
    c.Name, 
    c.Color, 
    (SELECT COUNT(*) FROM Products WHERE CategoryId = c.Id) AS ProductsCount,
    c.Version
FROM Categories c
```

### Map() - 用于单个对象

**当以下情况时使用 `Map()`：**
- 将单个实体转换为 DTO
- 创建或更新记录
- 您已经在内存中拥有实体对象

**来自同一控制器的示例：**

```csharp
[HttpPost]
public async Task<CategoryDto> Create(CategoryDto dto, CancellationToken cancellationToken)
{
    var entityToAdd = dto.Map(); // 将 DTO 转换为实体

    await DbContext.Categories.AddAsync(entityToAdd, cancellationToken);
    await DbContext.SaveChangesAsync(cancellationToken);

    return entityToAdd.Map(); // 将实体转换回 DTO
}
```

### 手动投影替代方案

**重要**：使用 Mapperly 的 `Project()` **不是强制性的**。您可以使用 LINQ 的 `Select()` 手动执行投影：

```csharp
// 使用 Mapperly 的 Project()
return DbContext.Categories.Project();

// 手动替代方案（产生相同的 SQL）
return DbContext.Categories.Select(c => new CategoryDto
{
    Id = c.Id,
    Name = c.Name,
    Color = c.Color,
    ProductsCount = c.Products.Count,
    Version = c.Version
});
```

**两种方法产生完全相同的 SQL 查询**。然而，Mapperly 的 `Project()` 提供了以下优势：

1. **减少重复代码**：只需在映射器中编写一次映射逻辑
2. **自动更新**：当您添加/删除实体属性时，映射器会自动更新
3. **一致性**：确保整个应用程序使用相同的映射逻辑
4. **重构安全**：重命名属性会更新所有映射

---

## 5. Patch() 方法 - 高效更新

### 服务器端 Patch 用法

在 Update 控制器方法中，`Patch()` 用于使用 DTO 的值更新实体：

```csharp
[HttpPut]
public async Task<CategoryDto> Update(CategoryDto dto, CancellationToken cancellationToken)
{
    var entityToUpdate = await DbContext.Categories.FindAsync([dto.Id], cancellationToken)
        ?? throw new ResourceNotFoundException();

    dto.Patch(entityToUpdate); // 用 DTO 值更新实体

    await DbContext.SaveChangesAsync(cancellationToken);

    return entityToUpdate.Map();
}
```

**为什么使用 Patch() 而不是 Map()？**

- **保留未更改的属性**：仅更新客户端发送的属性
- **尊重实体状态**：保持 EF Core 的变更跟踪
- **安全性**：防止过度发布 (overposting) 攻击

### 客户端 Patch 用法

**位置**：[`src/Shared/Features/[Feature]/[Feature]Mapper.cs`](/src/Shared/Features/)

在客户端，项目在特定功能的映射器文件中定义了额外的 `Patch()` 方法，用于更新绑定到 UI 的 DTO：

```csharp
[Mapper(UseDeepCloning = true)]
public static partial class Mapper
{
    public static partial void Patch(this TodoItemDto source, TodoItemDto destination);
    public static partial void Patch(this ProductDto source, ProductDto destination);
    public static partial void Patch(this CategoryDto source, CategoryDto destination);
    // ... 更多 patch 方法
}
```

### 现实世界示例：AddOrEditCategoryModal

**位置**：[`AddOrEditCategoryModal.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Categories/AddOrEditCategoryModal.razor.cs)

```csharp
public async Task ShowModal(CategoryDto categoryToShow)
{
    isOpen = true;
    categoryToShow.Patch(category); // 用数据更新本地 category 对象
    StateHasChanged();
}
```

**为什么采用这种模式？**

当模态框打开以编辑某个类别时，我们不是替换整个 `category` 对象（这会破坏 UI 绑定），而是将值**修补 (patch)** 到现有对象中。这确保了：

1. **UI 绑定保持完整**：表单输入保持连接到同一个对象引用
2. **变更跟踪正常工作**：`EditContext.IsModified()` 能正确检测变更
3. **无重新渲染问题**：Blazor 的变更检测平滑运行

### 另一个示例：ProfileSection

**位置**：[`ProfileSection.razor.cs`](/src/Client/AI.Boilerplate.Client.Core/Components/Pages/Settings/ProfileSection.razor.cs)

```csharp
private async Task SaveProfile()
{
    editUserDto.Patch(CurrentUser);
    
    (await userController.Update(editUserDto, CurrentCancellationToken)).Patch(CurrentUser);
}
```

---
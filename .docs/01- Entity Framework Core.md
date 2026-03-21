# 第一阶段：Entity Framework Core

欢迎使用交互式入门指南！本文档将带您深入了解本 AI.Boilerplate 项目中 Entity Framework Core 的架构。

---

## 概述

Entity Framework Core (EF Core) 是本项目中使用的主要数据访问技术。它提供了一个对象关系映射 (ORM) 层，让您能够使用 .NET 对象操作数据库，从而省去了大量通常需要编写的数据访问代码。

在本阶段，您将学习到：
- **AppDbContext**：用于服务器端数据访问的核心数据库上下文
- **实体模型 (Entity Models)**：如何定义领域实体
- **可空引用类型 (Nullable Reference Types)**：理解实体属性中的可空性
- **实体类型配置 (Entity Type Configurations)**：配置实体映射的最佳实践
- **迁移 (Migrations)**：如何管理数据库架构变更

---

## 1. AppDbContext - 数据访问的核心

### 位置
主数据库上下文位于：
[`/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Data/AppDbContext.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Data/AppDbContext.cs)

### 什么是 AppDbContext？

`AppDbContext` 是协调数据模型 Entity Framework 功能的核心类。它继承自 `IdentityDbContext`，后者为 ASP.NET Core Identity（用户、角色、认证）提供了内置支持。

### 主要特性

以下是实际项目中 `AppDbContext` 的结构：

```csharp
public partial class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    // DbSets 代表数据库中的表
    public virtual DbSet<Category> Categories { get; set; } = default!;
    public virtual DbSet<Product> Products { get; set; } = default!;
    public virtual DbSet<TodoItem> TodoItems { get; set; } = default!;
    // ... 以及其他 DbSets

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```
---

## 2. 实体模型 - 定义您的领域

### 位置
实体模型按领域组织在：
[`/src/Server/AI.Boilerplate.Server.Api/Features/`](/src/Server/AI.Boilerplate.Server.Api/Features/)

文件夹结构如下：
```
Features/
├── Categories/
│   ├── Category.cs
│   └── CategoryConfiguration.cs
├── Products/
│   ├── Product.cs
│   └── ProductConfiguration.cs
├── Todo/
│   ├── TodoItem.cs
│   └── TodoConfiguration.cs
├── Identity/
│   ├── Models/
│   │   ├── User.cs, Role.cs, 等
│   └── Configurations/
│       ├── UserConfiguration.cs, RoleConfiguration.cs, 等
└── ... 其他领域
```

### 示例：Category 实体

让我们查看项目中的 `Category` 实体：

**文件：** [`/src/Server/AI.Boilerplate.Server.Api/Features/Categories/Category.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/Category.cs)

```csharp
using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api.Features.Categories;

public partial class Category
{
    public Guid Id { get; set; }

    [Required, MaxLength(64)]
    public string? Name { get; set; }

    public string? Color { get; set; }

    public long Version { get; set; }

    public IList<Product> Products { get; set; } = [];
}
```

### **Version** 并发戳记
```csharp
public long Version { get; set; }
```
- **对于乐观并发控制至关重要**
- 在 SQL Server 中配置为 **行版本 (row version)**
- 当多个用户编辑同一条记录时，自动防止更新丢失
- 您**必须**在所有将被更新的实体中包含此属性

---

### 理解可空引用类型

本项目使用 C# 可空引用类型来提高代码安全性。理解实体属性中的可空模式至关重要：

#### 带有 [Required] 的字符串属性

```csharp
[Required, MaxLength(64)]
public string? Name { get; set; }
```

**为什么尽管有 `[Required]` 属性，它还是 `string?` (可空)？**

- EF Core 在**验证发生之前**会将属性设置为 `null`
- `[Required]` 属性是一条**验证规则**，而非可空性约束
- 在实体实例化期间，属性初始值为 `null`
- 只有在验证之后，EF Core 才会强制执行该要求
- 使用 `string?` 可以在保持验证的同时避免编译器警告

#### 导航属性模式

本项目对关系遵循特定的可空性模式：

##### “一”端 (单个实体引用)

```csharp
[ForeignKey(nameof(CategoryId))]
public Category? Category { get; set; }

public Guid CategoryId { get; set; }
```

**模式：带 `?` 的可空类型**

- **为什么可空？** 相关实体可能尚未从数据库加载
- EF Core **不会**自动加载相关实体
- 示例：当您查询 `Products` 时，除非您显式包含它，否则 `Category` 属性为 `null`：
  ```csharp
  // Category 将为 null
  var product = await dbContext.Products.FirstAsync();
  
  // Category 将被加载
  var product = await dbContext.Products.Include(p => p.Category).FirstAsync();
  ```

##### “多”端 (集合导航属性)

```csharp
public IList<Product> Products { get; set; } = [];
```

**模式：非可空并使用 `= []` 初始化**

- **为什么非可空？** 集合永远不应为 `null`，以防止 `NullReferenceException`
- **为什么 `= []`？** 确保即使没有产品，集合也已初始化
- 您可以安全地调用 `.Add()`、`.Count` 等，无需进行空值检查
- 示例：
  ```csharp
  var category = new Category();
  category.Products.Add(new Product()); // ✅ 安全 - 无需空值检查
  ```

#### 可空引用类型总结表

| 属性类型 | 可空？ | 示例 | 原因 |
|--------------|-----------|---------|--------|
| **带 [Required] 的字符串** | ✅ 是 (`string?`) | `public string? Name { get; set; }` | EF Core 在验证前初始化为 `null` |
| **“一”端导航** | ✅ 是 (`Category?`) | `public Category? Category { get; set; }` | 相关实体可能未从数据库加载 |
| **“多”端集合** | ❌ 否 (使用 `= []` 初始化) | `public IList<Product> Products { get; set; } = []` | 防止空引用异常；安全遍历 |
| **值类型 (非可空)** | ❌ 否 | `public Guid Id { get; set; }` | 值类型有默认值 (例如 `Guid.Empty`) |
| **值类型 (可空)** | ✅ 是 (`DateTimeOffset?`) | `public DateTimeOffset? BirthDate { get; set; }` | 显式允许可选值为 null |

---

## 3. 实体类型配置 - 专业的方法

### 位置
实体配置与其实体放在一起，位于：
[`/src/Server/AI.Boilerplate.Server.Api/Features/`](/src/Server/AI.Boilerplate.Server.Api/Features/)

对于 Identity 领域，配置组织在专用的 Configurations 文件夹中：
```
Features/
├── Categories/
│   ├── Category.cs
│   └── CategoryConfiguration.cs
├── Products/
│   ├── Product.cs
│   └── ProductConfiguration.cs
├── Identity/
│   ├── Models/
│   │   ├── User.cs, Role.cs, 等
│   └── Configurations/
│       ├── UserConfiguration.cs, RoleConfiguration.cs, 等
└── ... 其他功能
```

### 示例：CategoryConfiguration

**文件：** [`/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryConfiguration.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Categories/CategoryConfiguration.cs)

```csharp
using AI.Boilerplate.Server.Api.Features.Categories;

namespace AI.Boilerplate.Server.Api.Features.Categories;

public partial class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // 配置 Name 的唯一索引
        builder.HasIndex(p => p.Name).IsUnique();

        // 种子初始数据
        var defaultVersion = 1;
        builder.HasData(
            new () { 
                Id = Guid.Parse("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), 
                Name = "Ford", 
                Color = "#FFCD56", 
                Version = defaultVersion 
            },
            new () { 
                Id = Guid.Parse("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), 
                Name = "Nissan", 
                Color = "#FF6384", 
                Version = defaultVersion 
            }
        );
    }
}
```

### 理解配置

#### 1. **唯一索引**
```csharp
builder.HasIndex(p => p.Name).IsUnique();
```
- 在 `Name` 列上创建唯一索引
- 确保没有两个类别具有相同的名称
- 数据库将在 SQL 级别强制此约束

#### 2. **使用 HasData() 播种数据**
```csharp
builder.HasData(
    new Category { Id = Guid.Parse("..."), Name = "Ford", ... }
);
```
用初始数据预填充数据库，数据在创建数据库时插入

### 如何应用配置

在 `AppDbContext.OnModelCreating()` 中：
```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
```

这短短一行代码：
- **扫描**整个程序集以查找实现 `IEntityTypeConfiguration<T>` 的类
- **自动应用**所有配置
- 您无需手动注册每个配置类

---

## 4. 迁移 (对于服务器端是可选的)

### 关于迁移的重要说明

**EF Core 迁移在本项目中不是必须的**，特别是对于：
- 测试项目
- 快速原型设计场景
- 可以轻松重新创建数据库的开发环境

### 默认方法：EnsureCreatedAsync()

默认情况下，项目使用 `Database.EnsureCreatedAsync()`，它会根据您的实体**自动创建**数据库架构，而无需迁移：

**文件：** [`/src/Server/AI.Boilerplate.Server.Api/Program.cs`](/src/Server/AI.Boilerplate.Server.Api/Program.cs)
```csharp
if (builder.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync(); // 自动创建架构
}
```

### 何时使用迁移？

当出现以下情况时，您应该**切换到迁移**：
- 部署到**生产环境**
- 需要在架构变更期间**保留现有数据**
- 想要对数据库架构进行**版本控制**
- 在**团队环境**中工作，需要跟踪架构变更

### 如何切换到迁移

如果您决定使用迁移，请按照以下步骤操作：

#### 步骤 1：用 MigrateAsync() 替换 EnsureCreatedAsync()

在以下 3 个文件中将 `EnsureCreatedAsync()` 替换为 `MigrateAsync()`：
1. [`/src/Server/AI.Boilerplate.Server.Api/Program.cs`](/src/Server/AI.Boilerplate.Server.Api/Program.cs)
2. [`/src/Server/AI.Boilerplate.Server.Web/Program.cs`](/src/Server/AI.Boilerplate.Server.Web/Program.cs)
3. [`/src/Tests/Infrastructure/TestsAssemblyInitializer.cs`](/src/Tests/Infrastructure/TestsAssemblyInitializer.cs)

**之前：**
```csharp
await dbContext.Database.EnsureCreatedAsync();
```

**之后：**
```csharp
await dbContext.Database.MigrateAsync();
```

#### 步骤 2：删除现有数据库 (如果适用)

**重要：** 如果您已经使用 `EnsureCreatedAsync()` 运行了项目，在切换到迁移之前，您**必须删除现有数据库**。

- `EnsureCreatedAsync()` 和 `MigrateAsync()` 不能混用
- 您的数据库将通过初始迁移重新创建

#### 步骤 3：创建您的第一个迁移

在 `AI.Boilerplate.Server.Api` 项目目录中打开终端并运行：

```bash
dotnet tool restore && dotnet ef migrations add Initial --output-dir Infrastructure/Data/Migrations --verbose
```

这将在 `/Infrastructure/Data/Migrations/` 文件夹中创建迁移文件。

#### 步骤 4：应用迁移

应用程序启动时，迁移将**自动应用** (得益于 `MigrateAsync()`)。

**注意：** 您**不需要**手动运行 `dotnet ef database update` 或 `Update-Database`。应用程序启动代码中的 `MigrateAsync()` 调用会自动处理此事。

### 添加未来的迁移

当您修改实体或配置时，创建一个新的迁移：

```bash
dotnet tool restore && dotnet ef migrations add <MigrationName> --output-dir Infrastructure/Data/Migrations --verbose
```

---
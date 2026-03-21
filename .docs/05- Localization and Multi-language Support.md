# 第五阶段：本地化与多语言支持

## 概述

AI.Boilerplate 项目包含一个全面的本地化和多语言支持系统，使您的应用程序能够为使用多种语言的用户提供服务。该系统基于 .NET 标准的 `.resx` 资源文件基础架构构建，并与 Blazor 组件、验证特性和服务器端代码无缝集成。

该系统提供类型安全、编译时验证的本地化功能，可在所有平台（Web、MAUI、Windows）上运行，并支持在运行时动态切换语言。

---

## 1. 资源文件 (`.resx`) 结构

资源文件是本地化系统的基础。它们以 XML 格式将可翻译的字符串存储为键值对。

### 位置

资源文件分布在项目中的两个位置：

#### 共享资源 (`src/Shared/Resources/`)
- **`AppStrings.resx`**: 用于通用应用程序字符串的默认语言资源文件（默认英文）
- **`AppStrings.fa.resx`**: 波斯语/法尔西语翻译
- **`AppStrings.sv.resx`**: 瑞典语翻译
- **`IdentityStrings.resx`**: 默认的身份认证相关字符串（身份验证、授权、用户管理）
- **`IdentityStrings.fa.resx`**: 波斯语身份认证字符串
- **`IdentityStrings.sv.resx`**: 瑞典语身份认证字符串

#### 服务器 API 资源 (`src/Server/AI.Boilerplate.Server.Api/Features/Identity/Resources/`)
- **`EmailStrings.resx`**: 默认电子邮件模板字符串
- **`EmailStrings.fa.resx`**: 波斯语电子邮件模板
- **`EmailStrings.sv.resx`**: 瑞典语电子邮件模板

这种分离使得服务器特定资源（如电子邮件模板）能够独立于面向客户端的 UI 字符串进行管理。

### 文件结构

每个 `.resx` 文件都是一个包含键值对的 XML 文档：

```xml
<data name="Name" xml:space="preserve">
  <value>Name</value>
</data>
<data name="Language" xml:space="preserve">
  <value>Language</value>
</data>
<data name="RequiredAttribute_ValidationError" xml:space="preserve">
  <value>The {0} field is required.</value>
</data>
```

**要点：**
- 每个 `<data>` 元素都有一个唯一的 `name` 属性（资源键）
- `<value>` 元素包含翻译后的文本
- `xml:space="preserve"` 确保保留空格
- `{0}`、`{1}` 等占位符用于参数化字符串

### 命名约定

- **默认语言**: `[FileName].resx`（例如，`AppStrings.resx`）
- **翻译文件**: `[FileName].[culture].resx`（例如，`AppStrings.fa.resx`, `AppStrings.sv.resx`）

区域性代码遵循 ISO 639-1 标准：
- `fa` 波斯语
- `sv` 瑞典语
- `fr` 法语
- `es` 西班牙语
- `de` 德语
- `ar` 阿拉伯语
- `zh` 中文
- 等等

### 示例：比较默认文件与翻译文件

**AppStrings.resx（英语 - 默认）:**
```xml
<data name="Settings" xml:space="preserve">
  <value>Settings</value>
</data>
<data name="Language" xml:space="preserve">
  <value>Language</value>
</data>
```

**AppStrings.fa.resx（波斯语）:**
```xml
<data name="Settings" xml:space="preserve">
  <value>تنظیمات</value>
</data>
<data name="Language" xml:space="preserve">
  <value>زبان</value>
</data>
```

请注意，所有语言文件中的 **资源键保持一致** - 只有值发生变化。这对于本地化系统的正常运行至关重要。

---

## 2. DTOs 与 `[DtoResourceType]` 特性

DTO（数据传输对象）使用 `[DtoResourceType]` 特性将验证消息和显示名称连接到资源文件。这确保了验证错误和表单标签能够根据用户选择的语言自动本地化。

### 它解决的问题

如果没有 `[DtoResourceType]`，您需要在 DTO 的 **每一个验证特性** 上指定资源类型：

```csharp
// ❌ 重复的做法（不使用 DtoResourceType）
public partial class CategoryDto
{
    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError), 
              ErrorMessageResourceType = typeof(AppStrings))]  // 重复！
    [Display(Name = nameof(AppStrings.Name), 
             ResourceType = typeof(AppStrings))]  // 重复！
    public string? Name { get; set; }
}
```

`[DtoResourceType]` 特性通过在类级别 **一次性** 指定资源类型消除了这种重复。

### 示例：CategoryDto

这是项目中实际的 `CategoryDto`（位于 `src/Shared/Dtos/Categories/CategoryDto.cs`）：

```csharp
namespace AI.Boilerplate.Shared.Dtos.Categories;

[DtoResourceType(typeof(AppStrings))]  // ✅ 一次性指定资源类型！
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

### 要点：

1. **`[DtoResourceType(typeof(AppStrings))]`**: 指定哪个资源类包含此 DTO 的本地化字符串
   - 在类级别应用一次
   - 自动被该类中的所有验证特性使用
   
2. **`ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError)`**: 
   - 使用 `nameof()` 引用资源键以实现编译时安全
   - 实际的错误消息根据用户的语言来自 `.resx` 文件
   
3. **`Display(Name = nameof(AppStrings.Name))`**: 
   - 指定表单标签的显示名称
   - 同样从资源文件中提取并自动本地化

4. **计算属性**: 
   - 像 `ProductsCount` 这样经过计算或只读的属性不需要验证特性
   - 它们仍然是出于数据传输目的的 DTO 的一部分

### 为什么使用 `nameof()`？

使用 `nameof()` 提供 **编译时安全**：

**✅ 使用 `nameof()`:**
```csharp
ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError)
```
- 如果您重命名或删除了资源键，您将收到 **编译错误**
- 重构工具可以安全地重命名资源键
- 智能提示（IntelliSense）提供自动补全

**❌ 不使用 `nameof()`（字符串字面量）:**
```csharp
ErrorMessage = "RequiredAttribute_ValidationError"  // ❌ 危险！
```
- 如果您重命名或删除了资源键，您将收到 **运行时错误**
- 拼写错误直到运行时才会被发现
- 没有智能提示支持

### 资源键示例

以下是 `AppStrings.resx` 中对应的条目：

```xml
<!-- Validation Error Messages -->
<data name="RequiredAttribute_ValidationError" xml:space="preserve">
  <value>The {0} field is required.</value>
</data>

<data name="MaxLengthAttribute_InvalidMaxLength" xml:space="preserve">
  <value>MaxLengthAttribute must have a Length value that is greater than zero...</value>
</data>

<!-- Field Names -->
<data name="Name" xml:space="preserve">
  <value>Name</value>
</data>

<data name="Color" xml:space="preserve">
  <value>Color</value>
</data>
```

以及它们在 `AppStrings.fa.resx` 中的波斯语翻译：

```xml
<!-- Validation Error Messages -->
<data name="RequiredAttribute_ValidationError" xml:space="preserve">
  <value>مقدار {0} الزامی است.</value>
</data>

<!-- Field Names -->
<data name="Name" xml:space="preserve">
  <value>نام</value>
</data>

<data name="Color" xml:space="preserve">
  <value>رنگ</value>
</data>
```

请注意 **占位符 `{0}`** 是如何在两种语言中保留的 - 它将在运行时被替换为字段名。

---

## 3. AppDataAnnotationsValidator

要使 `[DtoResourceType]` 在 Blazor EditForms 中工作，您 **必须** 使用自定义的 `AppDataAnnotationsValidator` 组件，而不是标准的 `DataAnnotationsValidator`。

### 为什么需要它？

Blazor 中标准的 `DataAnnotationsValidator` **无法识别** `[DtoResourceType]` 特性。它是为在每个验证特性上指定 `ErrorMessageResourceType` 的传统方法而设计的。

`AppDataAnnotationsValidator`（位于 `src/Client/AI.Boilerplate.Client.Core/Components/AppDataAnnotationsValidator.cs`）将验证系统扩展为：

1. **识别 `[DtoResourceType]`**: 从 DTO 类中读取特性
2. **解析本地化消息**: 自动从正确的资源文件中提取错误消息
3. **显示服务器端验证错误**: 通过用于 `ResourceValidationException` 的 `DisplayErrors()` 方法

**为什么显示服务器端验证错误很重要：**

虽然 Blazor 的 `EditForm` 会根据 DataAnnotations 特性（如 `[Required]`、`[MaxLength]`）显示客户端验证错误，但在某些情况下，您需要显示只能由服务器确定的 **服务器端验证错误**。例如：

- **名称重复**: 检查数据库中是否已存在某个类别名称（例如，“名称为 'Electronics' 的类别已存在”）

在这些情况下，服务器会抛出一个包含特定字段错误消息的 `ResourceValidationException`。`AppDataAnnotationsValidator.DisplayErrors()` 方法将这些服务器端错误映射到相应的表单字段，将它们与相关输入控件内联显示——就像客户端验证错误一样。

### 使用示例

这是项目中实际的示例（位于 `src/Client/AI.Boilerplate.Client.Core/Components/Pages/Categories/AddOrEditCategoryModal.razor`）：

```xml
<EditForm @ref="editForm" Model="category" OnValidSubmit="WrapHandled(Save)" novalidate>
    <AppDataAnnotationsValidator @ref="validatorRef" />

    <BitStack Gap="0.25rem">
        <BitTextField @bind-Value="category.Name"
                      Label="@Localizer[nameof(AppStrings.Name)]" />
        <ValidationMessage For="() => category.Name" />
        
        <BitLabel For="catColorInput">@Localizer[nameof(AppStrings.Color)]</BitLabel>
        <ValidationMessage For="() => category.Color" />
        
        <BitButton IsLoading=isSaving ButtonType="BitButtonType.Submit">
            @Localizer[nameof(AppStrings.Save)]
        </BitButton>
    </BitStack>
</EditForm>
```

### 后台代码集成

在后台代码文件（`AddOrEditCategoryModal.razor.cs`）中，您可以引用验证器来显示服务器端验证错误：

```csharp
private AppDataAnnotationsValidator validatorRef = default!;

private async Task Save()
{
    if (isSaving) return;
    isSaving = true;

    try
    {
        if (category.Id == default)
        {
            await categoryController.Create(category, CurrentCancellationToken);
        }
        else
        {
            await categoryController.Update(category, CurrentCancellationToken);
        }
    }
    catch (ResourceValidationException e)
    {
        // Display server-side validation errors in the form
        validatorRef.DisplayErrors(e);
    }
    finally
    {
        isSaving = false;
    }
}
```

---

## 4. 在代码中使用 `IStringLocalizer<T>`

`IStringLocalizer<T>` 接口是在 C# 代码中以编程方式访问本地化字符串的主要途径。它允许您根据资源键检索翻译后的字符串，并支持参数化消息。

### 在组件和页面中

所有继承自 `AppComponentBase` 的组件或继承自 `AppPageBase` 的页面都会自动访问 `Localizer` 属性。

**基类位置**: 
- 组件: `src/Client/AI.Boilerplate.Client.Core/Components/AppComponentBase.cs`
- 页面: `src/Client/AI.Boilerplate.Client.Core/Components/Pages/AppPageBase.cs`

```csharp
public partial class AppComponentBase
{
    [AutoInject] protected IStringLocalizer<AppStrings> Localizer = default!;
    // ... other injected services
}
```

这意味着 **项目中的每个组件** 都会自动获得可用的 `Localizer` 属性，而无需手动注入。

#### 在 Razor 文件中的使用

**`AddOrEditCategoryModal.razor` 示例:**

```xml
<BitText Typography="BitTypography.H5">
    @if (category.Id == default)
    {
        @Localizer[nameof(AppStrings.AddCategory)]
    }
    else
    {
        @Localizer[nameof(AppStrings.EditCategory)]
    }
</BitText>

<BitTextField @bind-Value="category.Name"
              Label="@Localizer[nameof(AppStrings.Name)]" />

<BitButton ButtonType="BitButtonType.Submit">
    @Localizer[nameof(AppStrings.Save)]
</BitButton>
```

### 在控制器中（服务器端）

所有继承自 `AppControllerBase` 的 API 控制器都会自动访问 `Localizer` 属性。

**基类位置**: `src/Server/AI.Boilerplate.Server.Api/Infrastructure/Controllers/AppControllerBase.cs`

```csharp
public partial class AppControllerBase : ControllerBase
{
    [AutoInject] protected AppDbContext DbContext = default!;
    [AutoInject] protected IStringLocalizer<AppStrings> Localizer = default!;
    [AutoInject] protected ServerApiSettings AppSettings = default!;
}
```

#### 在控制器中的使用

**`CategoryController.cs` 示例:**

```csharp
[HttpGet("{id}")]
public async Task<CategoryDto> Get(Guid id, CancellationToken cancellationToken)
{
    var dto = await Get().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    if (dto is null)
        throw new ResourceNotFoundException(Localizer[nameof(AppStrings.CategoryCouldNotBeFound)]);

    return dto;
}

[HttpDelete("{id}/{version}")]
public async Task Delete(Guid id, string version, CancellationToken cancellationToken)
{
    if (await DbContext.Products.AnyAsync(p => p.CategoryId == id, cancellationToken))
    {
        throw new BadRequestException(Localizer[nameof(AppStrings.CategoryNotEmpty)]);
    }
    
    // Delete logic...
}

private async Task Validate(Category category, CancellationToken cancellationToken)
{
    var entry = DbContext.Entry(category);
    
    if ((entry.State is EntityState.Added || entry.Property(c => c.Name).IsModified)
        && await DbContext.Categories.AnyAsync(p => p.Name == category.Name, cancellationToken))
    {
        // Validation error with parameter
        throw new ResourceValidationException(
            (nameof(CategoryDto.Name), 
             [Localizer[nameof(AppStrings.DuplicateCategoryName), category.Name!]])
        );
    }
}
```

**要点:**
- 控制器自动从 HTTP 请求中获取用户的区域性（culture）
- 从控制器抛出的错误消息会自动为客户端进行本地化
- 客户端会以其选择的语言接收错误消息

### 在服务中

对于未继承包含 `Localizer` 基类的服务，您可以使用 `[AutoInject]` 特性直接注入 `IStringLocalizer<T>`。

**异常处理程序和服务示例:**

```csharp
public partial class ExceptionDelegatingHandler
{
    [AutoInject] private IStringLocalizer<AppStrings> localizer = default!;
    
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // ... request handling
        }
        catch (HttpRequestException exp) when (exp.StatusCode is HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedException(localizer[nameof(AppStrings.YouNeedToSignIn)]);
        }
        catch (HttpRequestException exp) when (exp.StatusCode is HttpStatusCode.Forbidden)
        {
            throw new ForbiddenException(localizer[nameof(AppStrings.ForbiddenException)]);
        }
    }
}
```

### 带参数的本地化

资源字符串可以包含像 `{0}`、`{1}` 等占位符，这些占位符将在运行时被替换。

**资源文件示例 (`AppStrings.resx`):**
```xml
<data name="DuplicateCategoryName" xml:space="preserve">
  <value>Category with name '{0}' already exists.</value>
</data>

<data name="PrivilegedDeviceLimitMessage" xml:space="preserve">
  <value>From {0} devices allowed for full features, you've used {1}.
After reaching {0}, extra sign-ins will have reduced functions.</value>
</data>

<data name="UserLockedOut" xml:space="preserve">
  <value>Your account has been locked. Try again in {0}.</value>
</data>
```

**带参数的使用:**

```csharp
// 单个参数
var message = Localizer[nameof(AppStrings.DuplicateCategoryName), categoryName];
// 结果: "Category with name 'Electronics' already exists."

// 多个参数
var message = Localizer[nameof(AppStrings.PrivilegedDeviceLimitMessage), maxDevices, usedDevices];
// 结果: "From 5 devices allowed for full features, you've used 3..."

// 复杂参数（格式化时间）
throw new BadRequestException(
    Localizer[nameof(AppStrings.UserLockedOut), 
              tryAgainIn.Humanize(culture: CultureInfo.CurrentUICulture)]
);
// 结果: "Your account has been locked. Try again in 5 minutes."
```

### IStringLocalizer 的最佳实践

1. **始终使用 `nameof()`** 来引用资源键：
   ```csharp
   ✅ Localizer[nameof(AppStrings.Save)]
   ❌ Localizer["Save"]  // 没有编译时安全！
   ```

2. **不要拼接本地化字符串** - 而是使用参数化消息：
   ```csharp
   ❌ var msg = Localizer[nameof(AppStrings.Hello)] + " " + userName;
   ✅ var msg = Localizer[nameof(AppStrings.HelloUser), userName];
   ```
   
3. **保留占位符顺序** - 某些语言可能需要不同的词序：
   ```xml
   <!-- 英语 -->
   <value>Delete {0} from {1}?</value>
   
   <!-- 某些语言可能需要 -->
   <value>{1} থেকে {0} মুছবেন?</value>  <!-- 孟加拉语：不同的词序 -->
   ```

4. **使用包含上下文的描述性资源键**：
   ```csharp
   ✅ Localizer[nameof(AppStrings.CategoryDeleteConfirmation)]
   ❌ Localizer[nameof(AppStrings.Confirm)]  // 过于宽泛
   ```

---

## 5. `bit-resx` 工具 - 自动翻译

`bit-resx` 工具（也称为 `Bit.ResxTranslator`）是一个 .NET 全局工具，它使用大型语言模型（LLM）（如 OpenAI 或 Azure OpenAI）自动翻译 `.resx` 文件。这极大地减少了在多种语言之间维护翻译所需的人力。

### 它的作用是什么？

该工具自动执行以下任务：

1. **比较资源文件**: 将默认语言的 `.resx` 文件与翻译版本进行分析比较
2. **识别缺失的翻译**: 找出默认文件中存在但在目标语言文件中缺失的资源键
3. **AI 驱动翻译**: 使用 LLM（OpenAI/Azure OpenAI）自动翻译缺失的条目
4. **保留现有翻译**: 从不覆盖手动翻译 - 仅添加缺失的翻译
5. **维持占位符格式**: 正确保留翻译文本中的 `{0}`、`{1}` 等占位符
6. **更新文件**: 自动将新翻译插入到相应的 `.resx` 文件中，同时保持 XML 结构
7. **创建新语言文件**: 如果目标语言的 `.resx` 文件不存在，可以生成新的文件

### 安装

将 `bit-resx` 安装为 .NET 全局工具：

```bash
dotnet tool install --global Bit.ResxTranslator
```

#### `Bit.ResxTranslator.json` 配置选项说明

**`DefaultLanguage`**: 您的主要语言的区域性代码（通常为 `"en"` 表示英语）

**`SupportedLanguages`**: 要翻译成的 ISO 639-1 语言代码数组：
- `"fa"` - 波斯语/法尔西语
- `"sv"` - 瑞典语
- `"fr"` - 法语
- `"de"` - 德语
- `"es"` - 西班牙语
- `"ar"` - 阿拉伯语
- `"zh"` - 中文
- `"hi"` - 印地语
- 等等

**`ResxPaths`**: 用于定位 `.resx` 文件的 Glob 模式：
```json
"ResxPaths": [
    "/src/**/*.resx",                    // src 目录下的所有 .resx 文件
]
```

**`ChatOptions`**: LLM 配置:
- `"Temperature": "0"` - 确定性翻译（推荐以保持一致性）

**`OpenAI` / `AzureOpenAI`**: LLM 提供商配置:
- `Model` - 要使用的模型（例如，`gpt-4.1-mini`）
- `Endpoint` - API 端点 URL
- `ApiKey` - 您的 API 密钥（请参阅下方的安全提示）

#### 安全提示：API 密钥

**❌ 永远不要将 API 密钥提交到源代码控制中！**

而是使用环境变量：

```json
{
    "OpenAI": {
        "ApiKey": null  // 在配置文件中保持为空
    }
}
```

然后设置环境变量：

**Windows (PowerShell):**
```powershell
$env:OpenAI__ApiKey = "your-api-key-here"
```

**Windows (CMD):**
```cmd
set OpenAI__ApiKey=your-api-key-here
```

**Linux/macOS:**
```bash
export OpenAI__ApiKey="your-api-key-here"
```

**对于 Azure OpenAI:**
```powershell
$env:AzureOpenAI__ApiKey = "your-azure-key"
```

该工具自动使用 `{Section}__{Property}` 的模式读取环境变量。

### 使用方法

在项目根目录（`Bit.ResxTranslator.json` 所在位置）运行翻译命令：

```bash
bit-resx-translate
```

## bit-resx 翻译器在 CD 流水线中的理念

虽然 `bit-resx` 翻译器是一个强大的自动翻译工具，但其核心理念是简化 CI/CD 流水线中的本地化过程。您 **不需要** 为所有支持的语言手动翻译或提交每一个语言文件。相反，您可以：

- 仅添加或手动翻译对您的项目重要的键和语言。
- 保持不太重要的语言（或不太关键的键）未翻译，甚至在源代码中省略它们。
- 在 CD（持续部署）过程中，`bit-resx` 将在发布之前自动为所有支持的语言填充任何缺失的翻译。

例如，在这个项目中，瑞典语文件比英语文件小得多，并且只包含经过手动审查或改进的键。如果某些自动翻译不令人满意，您只需手动添加或覆盖这些特定的键。在下一次 CD 运行中，只有缺失的键会被自动翻译，而您的手动翻译将被保留。

这种方法保持您的源代码整洁和专注，同时确保在部署时所有语言都已完全翻译。

这就是为什么 `bit-resx` 工具被添加到项目 CD 流水线中的原因。以下是它在这个项目的 GitHub Actions 中的使用方式：

**`.github/workflows/cd.yml` 示例:**

```yaml
- name: Install Bit.ResxTranslator
  run: dotnet tool install --global Bit.ResxTranslator --prerelease

- name: Translate Resources
  env:
    OpenAI__ApiKey: ${{ secrets.OPENAI_API_KEY }}
  run: bit-resx-translate
```

### 何时运行该工具

**在开发过程中（可选）:**
- 在将新的资源键添加到默认 `.resx` 文件后
- 在提交包含新的可翻译字符串的更改之前
- 当添加对新语言的支持时

**在 CD 流水线中:**
- 作为 CD 期间发布/部署的一部分

---

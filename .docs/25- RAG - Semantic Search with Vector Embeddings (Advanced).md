# 第二十五阶段：RAG - 基于向量嵌入的语义搜索（高级）

欢迎回到第二十五阶段！在这个高级阶段，您将学习利用向量嵌入（Vector Embeddings）进行数据库查询的强大语义搜索功能。此功能实现了超越简单关键词匹配的、基于含义的搜索。

---

## 1. 不同的搜索方法

本项目支持多种文本搜索实现方法，每种方法具有不同的能力和复杂度：

### 1.1 简单字符串匹配
- **描述**：使用基本的 `Contains()` 方法在数据库字段中搜索文本
- **示例**：`products.Where(p => p.Name.Contains("laptop"))`
- **优点**：实现简单，无需额外设置
- **缺点**：仅限于精确或部分关键词匹配，无法理解含义或同义词

### 1.2 全文搜索 (Full-Text Search)
- **描述**：数据库原生的全文搜索功能
- **示例**：PostgreSQL 的全文搜索，SQL Server 的 Full-Text Search
- **优点**：性能优于简单字符串匹配，支持词干提取和排名
- **缺点**：仍然基于关键词，无法理解语义含义或进行跨语言查询

### 1.3 基于向量的语义搜索
- **描述**：使用文本嵌入模型将文本转换为捕捉语义含义的数值向量
- **实现**：项目使用 Microsoft.Extensions.AI 中的 `IEmbeddingGenerator<string, Embedding<float>>`
- **优点**：
  - 理解含义，而不仅仅是关键词
  - 即使措辞不同也能找到语义相似的结果
  - 支持跨语言搜索
- **缺点**：需要集成 AI 模型，设置更复杂，计算成本更高

### 1.4 混合方法
- **描述**：结合全文搜索和基于向量的搜索以获得最佳结果
- **实现**：先尝试全文搜索，如果结果不足则回退到向量搜索
- **优点**：兼具两者的优势——速度和语义理解
- **缺点**：实现最复杂

---

## 2. 理解向量嵌入 (Vector Embeddings)

### 2.1 什么是嵌入？

**嵌入 (Embeddings)** 是文本的数值表示（向量），它们捕捉了语义含义。可以将它们想象成多维空间中的坐标，语义相似的概念在该空间中彼此靠近。

**示例**：
```
"laptop computer" (笔记本电脑) → [0.23, -0.45, 0.78, ..., 0.12]  (384 维)
"notebook PC" (笔记型电脑)     → [0.25, -0.43, 0.80, ..., 0.14]  (384 维)
"apple fruit" (苹果水果)       → [-0.67, 0.34, -0.21, ..., 0.89] (384 维)
```

注意，"laptop computer" 和 "notebook PC" 具有相似的向量（在语义空间中距离很近），而 "apple fruit" 则相距甚远。

### 2.2 语义搜索能力

使用向量嵌入，您可以执行理解含义而不仅仅是关键词的搜索：

**传统关键词搜索**：
- 查询："laptop computer"
- 结果：仅包含确切单词 "laptop" 或 "computer" 的产品

**语义向量搜索**：
- 查询："laptop computer"
- 结果：
  - 包含 "laptop", "computer", "notebook", "PC", "portable workstation" 的产品
  - 甚至是用不同但相关术语描述的产品
  - 甚至可以找到具有相似含义的其他语言的结果！

### 2.3 跨语言搜索

语义搜索最强大的功能之一是**跨语言能力**：

**示例场景**：
- 用户用英语搜索："laptop computer"
- 系统可以找到用以下语言描述的产品：
  - 英语："Portable notebook PC"
  - 法语："Ordinateur portable"
  - 西班牙语："Computadora portátil"
  - 德语："Tragbarer Computer"

之所以有效，是因为嵌入捕捉的是跨语言的**语义含义**，而不仅仅是字面单词。

---

## 3. 嵌入模型

本项目支持多种嵌入模型提供商。您可以在 `AI.Boilerplate.Server.Api` 项目的 `appsettings.json` 文件的 `AI` 部分下进行配置。

### 3.1 LocalTextEmbeddingGenerationService (默认)

**位置**: [`src/Server/AI.Boilerplate.Server.Api/Program.Services.cs`](/src/Server/AI.Boilerplate.Server.Api/Program.Services.cs) (第 367-372 行)

```csharp
services.AddEmbeddingGenerator(sp => new LocalTextEmbeddingGenerationService()
    .AsEmbeddingGenerator())
    .UseLogging()
    .UseOpenTelemetry();
```

**特点**：
- 本地运行，无需外部 API 调用
- 生成 384 维向量
- 使用在服务器上运行的小型模型
- **不推荐用于生产环境**，因为准确性有限
- 适用于开发和测试

### 3.2 生产环境推荐模型

对于生产环境，您应该使用更准确的模型：

#### **OpenAI Embeddings**
**配置** (`appsettings.json`):
```json
"OpenAI": {
    "EmbeddingModel": "text-embedding-3-small",
    "EmbeddingApiKey": "your-api-key",
    "EmbeddingEndpoint": "https://models.inference.ai.azure.com"
}
```

- **模型**: `text-embedding-3-small`, `text-embedding-3-large`
- **优点**: 高准确性，广泛支持
- **缺点**: 需要 API 调用，按 Token 收费

#### **Azure OpenAI**
**配置** (`appsettings.json`):
```json
"AzureOpenAI": {
    "EmbeddingModel": "text-embedding-3-small",
    "EmbeddingApiKey": "your-key",
    "EmbeddingEndpoint": "https://yourResourceName.openai.azure.com/openai/deployments/yourDeployment"
}
```

- **优点**: 企业级，合规性，数据驻留控制
- **缺点**: 需要 Azure 订阅，有费用

#### **Hugging Face 模型**
**配置** (`appsettings.json`):
```json
"HuggingFace": {
    "EmbeddingApiKey": "your-key",
    "EmbeddingEndpoint": "https://api-inference.huggingface.co/models/..."
}
```

---

## 4. 在项目中启用嵌入

默认情况下，项目中**禁用**了嵌入功能。要启用它：

### 步骤 1：打开 AppDbContext.cs

**文件位置**: [`src/Server/AI.Boilerplate.Server.Api/Infrastructure/Data/AppDbContext.cs`](/src/Server/AI.Boilerplate.Server.Api/Infrastructure/Data/AppDbContext.cs)

**当前状态**:
```csharp
// 这需要 SQL Server 2025+
public static readonly bool IsEmbeddingEnabled = false;
```

### 步骤 2：修改以启用嵌入

**修改为**:
```csharp
// 这需要 SQL Server 2025+
public static readonly bool IsEmbeddingEnabled = true;
```

### 步骤 3：重要前提条件

⚠️ **关键要求**：向量嵌入需要 **SQL Server 2025+**，该版本包含原生向量搜索支持。

如果您使用的是较旧版本的 SQL Server：
- **选项 1**: 升级到 SQL Server 2025+
- **选项 2**: 切换到带有 `pgvector` 扩展的 PostgreSQL
- **选项 3**: 使用其他支持向量的数据库（例如 Azure Cosmos DB, Qdrant 等）

---

## 5. 嵌入在项目中如何工作

### 5.1 Product 实体

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Products/Product.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Products/Product.cs)

`Product` 模型包含一个 `Embedding` 属性：

```csharp
public SqlVector<float>? Embedding { get; set; }
```

此属性存储产品数据的 384 维向量表示。

### 5.2 Product 配置

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductConfiguration.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductConfiguration.cs)

```csharp
if (AppDbContext.IsEmbeddingEnabled)
{
    builder.Property(p => p.Embedding).HasColumnType("vector(384)");
}
else
{
    builder.Ignore(p => p.Embedding);
}
```

- **启用时**: 在数据库中创建 `vector(384)` 列
- **禁用时**: 忽略该属性（不创建数据库列）
- **维度**: 384 与 `LocalTextEmbeddingGenerationService` 的输出匹配（其他模型需调整）

### 5.3 ProductEmbeddingService

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductEmbeddingService.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductEmbeddingService.cs)

此服务处理嵌入生成和语义搜索：

#### **生成嵌入**

`Embed()` 方法从产品数据创建嵌入：

```csharp
public async Task Embed(Product product, CancellationToken cancellationToken)
{
    // 组合多个字段并赋予不同权重
    List<(string text, float weight)> inputs =
    [
        ($"Id: {product.ShortId}", 0.9f),
        ($"Name: {product.Name}", 0.9f),
        (product.Category!.Name!, 0.9f)
    ];
    
    if (string.IsNullOrEmpty(product.DescriptionText) is false)
    {
        inputs.Add((product.DescriptionText, 0.7f));
    }
    
    // ...
    
    // 为所有输入生成嵌入，按权重组合，并进行归一化
    
    // ...
    
    product.Embedding = new(embedding);
}
```

**关键点**：
- **加权组合**: 不同的产品字段具有不同的重要性（权重）
- **产品名称和 ID**: 最高权重 (0.9) - 对搜索最重要
- **描述**: 中等权重 (0.7)
- **替代文本 (Alt Text)**: 较低权重 (0.5)
- **类别**: 高权重 (0.9)
- **归一化**: L2 归一化确保余弦距离计算的稳定性

#### **使用嵌入进行搜索**

`SearchProducts()` 方法执行语义搜索：

```csharp
public async Task<IQueryable<Product>> SearchProducts(string searchQuery, CancellationToken cancellationToken)
{
    if (AppDbContext.IsEmbeddingEnabled is false)
        throw new InvalidOperationException("嵌入未启用。");
    
    // 为搜索查询生成嵌入
    var embeddedSearchQuery = await embeddingGenerator.GenerateAsync(searchQuery, cancellationToken);
    
    // 使用余弦距离搜索
    var value = new Microsoft.Data.SqlTypes.SqlVector<float>(embeddedSearchQuery.Vector);
    return dbContext.Products
        .Where(p => p.Embedding.HasValue && 
                    EF.Functions.VectorDistance("cosine", p.Embedding.Value, value) < DISTANCE_THRESHOLD)
        .OrderBy(p => EF.Functions.VectorDistance("cosine", p.Embedding!.Value, value!));
}
```

**关键点**：
- **距离阈值**: `0.65f` - 仅返回余弦距离小于此值的产品
- **余弦距离**: 测量向量之间的角度（越小越相似）
- **排序**: 结果按相似度排序（最匹配的排在前面）

### 5.4 在 ProductController 中的使用

**文件**: [`src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductController.cs`](/src/Server/AI.Boilerplate.Server.Api/Features/Products/ProductController.cs)

控制器在创建或更新产品时调用 `productEmbeddingService.Embed()` 方法。

#### **语义搜索**

```csharp
[HttpGet]
public async Task<IQueryable<ProductDto>> Search(string? searchQuery, ODataQueryOptions<ProductDto> odataQuery, CancellationToken cancellationToken)
{
    var query = (IQueryable<ProductDto>)odataQuery.ApplyTo(
        (await productEmbeddingService.SearchProducts(searchQuery, cancellationToken)).Project(),
        new ODataQuerySettings { HandleNullPropagation = HandleNullPropagationOption.False }
    );
    
    return query;
}
```

---

## 6. 重要注意事项

### 6.1 测试数据的局限性

⚠️ **重要**: 已植入数据库的测试产品**没有**嵌入数据。

**原因**: 嵌入是在通过 API 创建或更新产品时生成的。植入的数据是直接添加到数据库的，未经过嵌入处理过程。

**这意味着**:
- 使用语义搜索查找植入的测试产品将**不会**返回任何结果
- 只有通过 API 创建/更新的产品才会有嵌入并可被搜索

**解决方案**:
- 通过 API 创建新产品以测试语义搜索
- 或者通过 API 更新植入的产品以手动为其生成嵌入

### 6.2 性能考虑

**基于向量的搜索**：
- 比关键词搜索计算成本更高
- 需要 AI 模型 API 调用或本地模型执行
- 数据库向量操作需要现代数据库版本

**最佳实践 - 混合方法**：
混合方法可以在速度和准确性之间取得平衡。您可以先执行快速的全文搜索，如果结果不足，再回退到更全面的语义搜索。

### 6.3 何时使用语义搜索

**语义搜索对于以下情况是大材小用**：
- 名称简单的简单产品目录
- 精确匹配搜索
- 对简单数据的高性能要求

**语义搜索非常适合以下情况**：
- 大型内容库（文章、文档、知识库）
- 多语言内容
- 包含同义词和相关概念的复杂查询
- 推荐系统
- 支持工单路由
- FAQ 和聊天机器人系统

---

## 7. 性能优化：Azure DiskANN 索引与重排序 (Reranking)

当在 **SQL Server** 或 **PostgreSQL** 中使用大型数据集（例如数百万个向量）转向生产环境时，强烈建议使用 **DiskANN** 索引。DiskANN 提供高性能、基于磁盘的近似最近邻 (ANN) 搜索。

然而，为了高效利用 DiskANN 并保持高准确率（召回率），您应该修改查询策略。不要使用简单的 "Order By & Take"，而是使用**两步重排序 (Reranking)** 方法。

### 启用 DiskANN 的步骤

#### 步骤 A：在 `DbContext` 中启用扩展

在您的 `AppDbContext.cs` (Server.Api 项目) 中，确保注册了扩展：
以下是针对 PostgreSQL 的示例说明：

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{    
    // 为向量操作启用 pgvector 扩展
    modelBuilder.HasPostgresExtension("vector");
    
    // 启用 Azure 的高性能 DiskANN 扩展
    modelBuilder.HasPostgresExtension("pg_diskann"); 
}
```

#### 步骤 B：配置索引 (`EntityTypeConfiguration`)

在您的实体配置文件中（例如 `ProductConfiguration.cs`），定义 DiskANN 索引。这会告诉 EF Core 创建专用索引：

```csharp
public void Configure(EntityTypeBuilder<Product> builder)
{
    // 定义向量列
    builder.Property(p => p.Embedding)
           .HasColumnType("vector(384)"); // 根据您的嵌入模型调整大小

    // 定义 DiskANN 索引
    builder.HasIndex(p => p.Embedding)
           .HasMethod("diskann") // 显式使用 Azure 的 DiskANN
           .HasOperators("vector_cosine_ops") // 使用余弦相似度
           .HasStorageParameter("product_quantized", true); // 启用 `Product Quantization` 以利用高维支持
}
```

### 查询策略

1. **获取候选项**: 使用近似索引请求比您需要更多的结果（例如 50 个）。
2. **重排序**: 按精确距离重新排序这些候选项，并返回前几个结果（例如 10 个）。

参考文档：https://learn.microsoft.com/en-us/azure/postgresql/extensions/how-to-use-pgdiskann#improve-accuracy-when-using-pq-with-vector-reranking

不要使用标准查询，而是重构您的 LINQ 查询以使用子查询结构。这鼓励数据库引擎在进行细粒度排序之前先使用索引进行粗略过滤。

**标准查询（对 DiskANN 优化较少）**:

```csharp
// 简单扫描 - 在使用 heavily compressed indexes 时可能较慢或准确性较低
var value = new Pgvector.Vector(embeddedSearchQuery.Vector);
return dbContext.Products
    .Where(p => p.Embedding!.CosineDistance(value!) < DISTANCE_THRESHOLD)
    .OrderBy(p => p.Embedding!.CosineDistance(value!))
    .Take(10);
```

**推荐的重排序查询**:

```csharp
var value = new Pgvector.Vector(embeddedSearchQuery.Vector);
return dbContext.Products
    // 这里需要应用其余的过滤器，而不是由调用者在方法返回的 IQueryable 上应用。例如 Price > X 等。
    .Where(p => p.Embedding!.CosineDistance(value!) < DISTANCE_THRESHOLD)
    .OrderBy(p => p.Embedding!.CosineDistance(value!))
    .Take(CANDIDATE_COUNT) // 步骤 1：近似搜索（获取更多候选项以提高准确性）。这对于使用 DiskANN 索引尤为重要，因为它们可能会为了速度而牺牲一些准确性
    .OrderBy(p => p.Embedding!.CosineDistance(value!))
    .Take(FINAL_RESULT_COUNT); // 步骤 2：重排序（按精确距离细化为顶部结果）
```

**重要**: 数据库执行计划应反映重排序策略，确保有效利用 DiskANN 索引。

**为什么这很重要**:

* **DiskANN** 使用压缩（量化）来提高速度。
* 通过首先获取更大的“候选列表”（50 个），您可以补偿潜在的近似误差。
* 按精确距离对小列表（50 个项目）进行排序可确保您的最终前 10 名具有极高的准确性。

---

祝您编码愉快！🚀
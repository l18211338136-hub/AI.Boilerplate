namespace AI.Boilerplate.Server.Api.Features.Products;

public partial class ProductEmbeddingJobRunner
{
    [AutoInject] private AppDbContext dbContext = default!;
    [AutoInject] private ProductEmbeddingService productEmbeddingService = default!;
    [AutoInject] private ILogger<ProductEmbeddingJobRunner> logger = default!;

    /// <summary>
    /// 初始化产品向量化任务
    /// </summary>
    public void Initialize()
    {
        // 程序启动时立即执行一次
        RecurringJob.AddOrUpdate("product-embedding-job", () => ProcessProductsEmbedding(), Cron.MinuteInterval(5));
        logger.LogInformation("Product embedding job initialized");
    }

    /// <summary>
    /// 处理产品向量化
    /// </summary>
    public async Task ProcessProductsEmbedding()
    {
        try
        {
            logger.LogInformation("Starting product embedding process");

            // 查询需要向量化的产品
            var productsToEmbed = await dbContext.Products
                .Where(p => !string.IsNullOrEmpty(p.DescriptionText) && p.Embedding == null)
                .ToListAsync();

            logger.LogInformation($"Found {productsToEmbed.Count} products that need embedding");

            foreach (var product in productsToEmbed)
            {
                try
                {
                    logger.LogInformation($"Processing embedding for product: {product.Name}");
                    await productEmbeddingService.Embed(product, CancellationToken.None);
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation($"Successfully embedded product: {product.Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error embedding product {product.Name}");
                }
            }

            logger.LogInformation("Product embedding process completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in product embedding job");
        }
    }
}

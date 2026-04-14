namespace AI.Boilerplate.Server.Api.Features.Products;

public partial class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(t => t.HasComment("产品表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.ShortId).HasComment("产品短编号");
        builder.Property(p => p.Name).HasComment("产品名称");
        builder.Property(p => p.Price).HasComment("产品价格");
        builder.Property(p => p.DescriptionHTML).HasComment("HTML 描述");
        builder.Property(p => p.DescriptionText).HasComment("纯文本描述");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CategoryId).HasComment("类别 ID");
        builder.Property(p => p.Version).HasComment("并发版本");
        builder.Property(p => p.HasPrimaryImage).HasComment("是否有主图");
        builder.Property(p => p.PrimaryImageAltText).HasComment("主图替代文本");

        builder.HasIndex(p => p.Name).IsUnique(); // 产品名称索引
        builder.HasIndex(p => p.ShortId).IsUnique(); // 短编号索引

        builder.Property(p => p.ShortId).UseSequence("产品短编号");
        if (AppDbContext.IsEmbeddingEnabled)
        {
            builder.Property(p => p.Embedding).HasComment("语义向量");
            builder.Property(p => p.Embedding).HasColumnType("vector(768)"); // 维度取决于所使用的模型，这里假设为 768，因为使用的是 LocalTextEmbeddingGenerationService。
        }
        else
        {
            builder.Ignore(p => p.Embedding); // 忽略嵌入向量属性
        }
    }
}

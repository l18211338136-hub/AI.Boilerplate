namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagKnowledgeBaseConfiguration : IEntityTypeConfiguration<RagKnowledgeBase>
{
    public void Configure(EntityTypeBuilder<RagKnowledgeBase> builder)
    {
        builder.ToTable(t => t.HasComment("RAG 知识库表"));
        builder.Property(k => k.Id).HasComment("主键ID");
        builder.Property(k => k.Code).HasComment("知识库编码");
        builder.Property(k => k.Name).HasComment("知识库名称");
        builder.Property(k => k.EmbeddingModel).HasComment("嵌入模型");
        builder.Property(k => k.EmbeddingDimension).HasComment("嵌入维度");
        builder.Property(k => k.IsEnabled).HasComment("是否启用");
        builder.Property(k => k.CreatedOn).HasComment("创建时间");
        builder.Property(k => k.CreatedBy).HasComment("创建人ID");
        builder.Property(k => k.ModifiedOn).HasComment("最后修改时间");
        builder.Property(k => k.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(k => k.IsDeleted).HasComment("是否删除");
        builder.Property(k => k.DeletedOn).HasComment("删除时间");
        builder.Property(k => k.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(k => k.Code).IsUnique();

        builder.HasMany(k => k.Documents)
            .WithOne(d => d.KnowledgeBase)
            .HasForeignKey(d => d.KnowledgeBaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
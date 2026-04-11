namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagDocumentConfiguration : IEntityTypeConfiguration<RagDocument>
{
    public void Configure(EntityTypeBuilder<RagDocument> builder)
    {
        builder.ToTable(t => t.HasComment("RAG 文档表"));
        builder.Property(d => d.Id).HasComment("主键ID");
        builder.Property(d => d.KnowledgeBaseId).HasComment("知识库ID");
        builder.Property(d => d.SourceType).HasComment("来源类型");
        builder.Property(d => d.SourceId).HasComment("来源ID");
        builder.Property(d => d.Title).HasComment("文档标题");
        builder.Property(d => d.LastIndexedAt).HasComment("最后索引时间");
        builder.Property(d => d.CreatedOn).HasComment("创建时间");
        builder.Property(d => d.UpdatedAt).HasComment("更新时间");
        builder.Property(d => d.IsDeleted).HasComment("是否删除");
        builder.Property(d => d.DeletedOn).HasComment("删除时间");

        builder.HasIndex(d => new { d.KnowledgeBaseId, d.SourceType, d.SourceId }).IsUnique();

        builder.HasMany(d => d.Chunks)
            .WithOne(c => c.Document)
            .HasForeignKey(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunkConfiguration : IEntityTypeConfiguration<RagChunk>
{
    public void Configure(EntityTypeBuilder<RagChunk> builder)
    {
        builder.ToTable(t => t.HasComment("RAG 文本分片表"));
        builder.Property(c => c.Id).HasComment("主键ID");
        builder.Property(c => c.DocumentId).HasComment("文档ID");
        builder.Property(c => c.ChunkIndex).HasComment("分片索引");
        builder.Property(c => c.Content).HasComment("分片内容");
        builder.Property(c => c.TokenCount).HasComment("令牌数量");
        builder.Property(c => c.Embedding).HasComment("向量嵌入").HasColumnType("vector(768)");
        builder.Property(c => c.CreatedOn).HasComment("创建时间");
        builder.Property(c => c.CreatedBy).HasComment("创建人ID");
        builder.Property(c => c.ModifiedOn).HasComment("最后修改时间");
        builder.Property(c => c.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(c => c.IsDeleted).HasComment("是否删除");
        builder.Property(c => c.DeletedOn).HasComment("删除时间");
        builder.Property(c => c.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(c => new { c.DocumentId, c.ChunkIndex }).IsUnique();
    }
}
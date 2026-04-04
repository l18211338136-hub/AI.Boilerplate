namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunkConfiguration : IEntityTypeConfiguration<RagChunk>
{
    public void Configure(EntityTypeBuilder<RagChunk> builder)
    {
        builder.HasIndex(c => new { c.DocumentId, c.ChunkIndex }).IsUnique();
        builder.Property(c => c.Embedding).HasColumnType("vector(768)");
    }
}

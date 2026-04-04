namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagDocumentConfiguration : IEntityTypeConfiguration<RagDocument>
{
    public void Configure(EntityTypeBuilder<RagDocument> builder)
    {
        builder.HasIndex(d => new { d.KnowledgeBaseId, d.SourceType, d.SourceId }).IsUnique();

        builder.HasMany(d => d.Chunks)
            .WithOne(c => c.Document)
            .HasForeignKey(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

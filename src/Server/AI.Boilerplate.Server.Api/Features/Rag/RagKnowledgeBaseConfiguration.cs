namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagKnowledgeBaseConfiguration : IEntityTypeConfiguration<RagKnowledgeBase>
{
    public void Configure(EntityTypeBuilder<RagKnowledgeBase> builder)
    {
        builder.HasIndex(k => k.Code).IsUnique();

        builder.HasMany(k => k.Documents)
            .WithOne(d => d.KnowledgeBase)
            .HasForeignKey(d => d.KnowledgeBaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

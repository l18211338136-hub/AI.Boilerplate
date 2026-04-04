namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagKnowledgeBase
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string EmbeddingModel { get; set; } = string.Empty;

    public int EmbeddingDimension { get; set; } = 768;

    public bool IsEnabled { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public List<RagDocument> Documents { get; set; } = [];
}

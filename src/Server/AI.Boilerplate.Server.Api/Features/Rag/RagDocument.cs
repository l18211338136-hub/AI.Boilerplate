namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagDocument
{
    public Guid Id { get; set; }

    public Guid KnowledgeBaseId { get; set; }

    [Required]
    [MaxLength(32)]
    public string SourceType { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string SourceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = string.Empty;

    public DateTimeOffset LastIndexedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public RagKnowledgeBase? KnowledgeBase { get; set; }

    public List<RagChunk> Chunks { get; set; } = [];
}

using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagDocument : AuditEntity
{
    public Guid Id { get; set; }

    public Guid? KnowledgeBaseId { get; set; }

    [MaxLength(32)]
    public string? SourceType { get; set; }

    [MaxLength(512)]
    public string? SourceId { get; set; }

    [MaxLength(256)]
    public string? Title { get; set; }

    public DateTimeOffset? LastIndexedAt { get; set; }

    public RagKnowledgeBase? KnowledgeBase { get; set; }

    public List<RagChunk> Chunks { get; set; } = [];
}

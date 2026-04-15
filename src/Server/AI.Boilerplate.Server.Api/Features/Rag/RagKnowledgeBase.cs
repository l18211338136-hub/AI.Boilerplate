using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagKnowledgeBase : AuditEntity
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string? Code { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(128)]
    public string? EmbeddingModel { get; set; }

    public int? EmbeddingDimension { get; set; }

    public bool? IsEnabled { get; set; }

    public List<RagDocument> Documents { get; set; } = [];
}

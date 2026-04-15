using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagKnowledgeBase : AuditEntity
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

    public List<RagDocument> Documents { get; set; } = [];
}

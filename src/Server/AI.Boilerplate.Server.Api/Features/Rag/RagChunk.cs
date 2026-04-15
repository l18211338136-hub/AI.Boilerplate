using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using Pgvector;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunk : AuditEntity
{
    public Guid Id { get; set; }

    public Guid? DocumentId { get; set; }

    public int? ChunkIndex { get; set; }

    public string? Content { get; set; }

    public int? TokenCount { get; set; }

    public Vector? Embedding { get; set; }

    public RagDocument? Document { get; set; }
}

using Pgvector;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunk
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public int ChunkIndex { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public int TokenCount { get; set; }

    public Vector? Embedding { get; set; }

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public RagDocument? Document { get; set; }
}

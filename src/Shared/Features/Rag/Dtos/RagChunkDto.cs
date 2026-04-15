namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagChunkDto
{
    public Guid Id { get; set; }

    public Guid? DocumentId { get; set; }

    public int? ChunkIndex { get; set; }

    [Required]
    public string? Content { get; set; }

    public int? TokenCount { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }
}

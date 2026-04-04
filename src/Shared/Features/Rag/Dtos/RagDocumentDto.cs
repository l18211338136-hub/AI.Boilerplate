namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagDocumentDto
{
    public Guid Id { get; set; }

    public Guid KnowledgeBaseId { get; set; }

    [Required]
    [MaxLength(32)]
    public string? SourceType { get; set; }

    [Required]
    [MaxLength(512)]
    public string? SourceId { get; set; }

    [Required]
    [MaxLength(256)]
    public string? Title { get; set; }

    public int ChunkCount { get; set; }

    public string? ContentPreview { get; set; }

    public string? Content { get; set; }

    public DateTimeOffset LastIndexedAt { get; set; }
}

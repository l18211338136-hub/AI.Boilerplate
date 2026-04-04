namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagDocumentUpsertDto
{
    [Required]
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

    [MaxLength(20000)]
    public string? Content { get; set; }
}

namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagRetrievalDebugRequestDto
{
    [Required]
    public Guid KnowledgeBaseId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string? Question { get; set; }

    [Range(1, 20)]
    public int TopK { get; set; } = 8;

    [Range(0, 1)]
    public double? VectorWeight { get; set; }

    [Range(0, 1)]
    public double? KeywordWeight { get; set; }
}

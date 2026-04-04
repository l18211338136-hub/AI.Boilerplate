namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagKnowledgeBaseUpsertDto
{
    [Required]
    [MaxLength(64)]
    public string? Code { get; set; }

    [Required]
    [MaxLength(128)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(128)]
    public string? EmbeddingModel { get; set; }

    [Range(128, 4096)]
    public int EmbeddingDimension { get; set; } = 768;
}

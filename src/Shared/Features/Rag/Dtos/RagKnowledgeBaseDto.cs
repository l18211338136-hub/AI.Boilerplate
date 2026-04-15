namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagKnowledgeBaseDto
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(64)]
    public string? Code { get; set; }

    [Required]
    [MaxLength(128)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(128)]
    public string? EmbeddingModel { get; set; }

    public int? EmbeddingDimension { get; set; }

    public bool? IsEnabled { get; set; }

    public int? DocumentCount { get; set; }

    public int? ChunkCount { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
}

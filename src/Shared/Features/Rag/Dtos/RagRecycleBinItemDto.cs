namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagRecycleBinItemDto
{
    public string ItemType { get; set; } = string.Empty;

    public Guid Id { get; set; }

    public Guid? KnowledgeBaseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset DeletedOn { get; set; }
}

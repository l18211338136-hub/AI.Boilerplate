namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagChunkingOptionsDto
{
    [Range(100, 4000)]
    public int MaxChunkLength { get; set; } = 450;

    public bool PreferParagraphFirst { get; set; } = true;

    [Range(1, 20)]
    public int MinChunkCount { get; set; } = 1;
}

namespace AI.Boilerplate.Shared.Features.Rag.Dtos;

public class RagRetrievalDebugResultDto
{
    public List<RagRetrievalHitDto> Hits { get; set; } = [];

    public string ContextPreview { get; set; } = string.Empty;

    public double VectorWeight { get; set; }

    public double KeywordWeight { get; set; }

    public int CandidateCount { get; set; }
}

public class RagRetrievalHitDto
{
    public Guid ChunkId { get; set; }

    public Guid DocumentId { get; set; }

    public int ChunkIndex { get; set; }

    public double Score { get; set; }

    public double VectorScore { get; set; }

    public double KeywordScore { get; set; }

    public string ContentPreview { get; set; } = string.Empty;
}

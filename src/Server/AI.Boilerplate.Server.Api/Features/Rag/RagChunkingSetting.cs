using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunkingSetting : AuditEntity
{
    public Guid Id { get; set; }

    public int MaxChunkLength { get; set; } = 450;

    public bool PreferParagraphFirst { get; set; } = true;

    public int MinChunkCount { get; set; } = 1;

    public long Version { get; set; }
}

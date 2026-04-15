using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunkingSetting : AuditEntity
{
    public Guid Id { get; set; }

    public int? MaxChunkLength { get; set; }

    public bool? PreferParagraphFirst { get; set; }

    public int? MinChunkCount { get; set; }

    public long Version { get; set; }
}

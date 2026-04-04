using AI.Boilerplate.Shared.Features.Rag.Dtos;

namespace AI.Boilerplate.Shared.Features.Rag;

[AuthorizedApi]
[Route("api/v1/[controller]/[action]/")]
public interface IRagManagementController : IAppController
{
    [HttpGet]
    Task<List<RagKnowledgeBaseDto>> GetKnowledgeBases(CancellationToken cancellationToken) => default!;

    [HttpPost]
    Task<RagKnowledgeBaseDto> CreateKnowledgeBase(RagKnowledgeBaseUpsertDto dto, CancellationToken cancellationToken);

    [HttpPut("{knowledgeBaseId}")]
    Task<RagKnowledgeBaseDto> UpdateKnowledgeBase(Guid knowledgeBaseId, RagKnowledgeBaseUpsertDto dto, CancellationToken cancellationToken);

    [HttpDelete("{knowledgeBaseId}")]
    Task DeleteKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken);

    [HttpPost("{knowledgeBaseId}")]
    Task RestoreKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken);

    [HttpDelete("{knowledgeBaseId}")]
    Task PermanentlyDeleteKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken);

    [HttpGet("{knowledgeBaseId}")]
    Task<List<RagDocumentDto>> GetDocuments(Guid knowledgeBaseId, CancellationToken cancellationToken) => default!;

    [HttpPost]
    Task<RagDocumentDto> AddDocument(RagDocumentUpsertDto dto, CancellationToken cancellationToken);

    [HttpGet("{documentId}")]
    Task<RagDocumentDto> GetDocument(Guid documentId, CancellationToken cancellationToken) => default!;

    [HttpPut("{documentId}")]
    Task<RagDocumentDto> UpdateDocument(Guid documentId, RagDocumentUpsertDto dto, CancellationToken cancellationToken);

    [HttpDelete("{documentId}")]
    Task DeleteDocument(Guid documentId, CancellationToken cancellationToken);

    [HttpPost("{documentId}")]
    Task RestoreDocument(Guid documentId, CancellationToken cancellationToken);

    [HttpDelete("{documentId}")]
    Task PermanentlyDeleteDocument(Guid documentId, CancellationToken cancellationToken);

    [HttpGet("{documentId}")]
    Task<List<RagChunkDto>> GetChunks(Guid documentId, CancellationToken cancellationToken) => default!;

    [HttpPost]
    Task<RagRetrievalDebugResultDto> DebugRetrieve(RagRetrievalDebugRequestDto dto, CancellationToken cancellationToken);

    [HttpGet]
    Task<RagChunkingOptionsDto> GetChunkingOptions(CancellationToken cancellationToken) => default!;

    [HttpPost]
    Task<RagChunkingOptionsDto> UpdateChunkingOptions(RagChunkingOptionsDto dto, CancellationToken cancellationToken);

    [HttpGet]
    Task<List<RagRecycleBinItemDto>> GetRecycleBinItems(CancellationToken cancellationToken) => default!;
}

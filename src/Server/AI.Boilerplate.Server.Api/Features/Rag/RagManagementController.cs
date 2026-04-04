using AI.Boilerplate.Shared.Features.Rag;
using AI.Boilerplate.Shared.Features.Rag.Dtos;

namespace AI.Boilerplate.Server.Api.Features.Rag;

[ApiVersion(1)]
[ApiController, Route("api/v{v:apiVersion}/[controller]/[action]")]
[Authorize(Policy = AppFeatures.Management.ManageAiPrompt)]
public partial class RagManagementController : AppControllerBase, IRagManagementController
{
    [AutoInject] private RagManagementStore ragManagementStore = default!;

    [HttpGet]
    public Task<List<RagKnowledgeBaseDto>> GetKnowledgeBases(CancellationToken cancellationToken)
    {
        return ragManagementStore.GetKnowledgeBases(cancellationToken);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task<RagKnowledgeBaseDto> CreateKnowledgeBase(RagKnowledgeBaseUpsertDto dto, CancellationToken cancellationToken)
    {
        return ragManagementStore.CreateKnowledgeBase(dto, cancellationToken);
    }

    [HttpPut("{knowledgeBaseId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task<RagKnowledgeBaseDto> UpdateKnowledgeBase(Guid knowledgeBaseId, RagKnowledgeBaseUpsertDto dto, CancellationToken cancellationToken)
    {
        return ragManagementStore.UpdateKnowledgeBase(knowledgeBaseId, dto, cancellationToken);
    }

    [HttpDelete("{knowledgeBaseId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task DeleteKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        return ragManagementStore.DeleteKnowledgeBase(knowledgeBaseId, cancellationToken);
    }

    [HttpPost("{knowledgeBaseId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task RestoreKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        return ragManagementStore.RestoreKnowledgeBase(knowledgeBaseId, cancellationToken);
    }

    [HttpDelete("{knowledgeBaseId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task PermanentlyDeleteKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        return ragManagementStore.PermanentlyDeleteKnowledgeBase(knowledgeBaseId, cancellationToken);
    }

    [HttpGet("{knowledgeBaseId}")]
    public Task<List<RagDocumentDto>> GetDocuments(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        return ragManagementStore.GetDocuments(knowledgeBaseId, cancellationToken);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task<RagDocumentDto> AddDocument(RagDocumentUpsertDto dto, CancellationToken cancellationToken)
    {
        return ragManagementStore.AddDocument(dto, cancellationToken);
    }

    [HttpGet("{documentId}")]
    public Task<RagDocumentDto> GetDocument(Guid documentId, CancellationToken cancellationToken)
    {
        return ragManagementStore.GetDocument(documentId, cancellationToken);
    }

    [HttpPut("{documentId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task<RagDocumentDto> UpdateDocument(Guid documentId, RagDocumentUpsertDto dto, CancellationToken cancellationToken)
    {
        return ragManagementStore.UpdateDocument(documentId, dto, cancellationToken);
    }

    [HttpDelete("{documentId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task DeleteDocument(Guid documentId, CancellationToken cancellationToken)
    {
        return ragManagementStore.DeleteDocument(documentId, cancellationToken);
    }

    [HttpPost("{documentId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task RestoreDocument(Guid documentId, CancellationToken cancellationToken)
    {
        return ragManagementStore.RestoreDocument(documentId, cancellationToken);
    }

    [HttpDelete("{documentId}")]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task PermanentlyDeleteDocument(Guid documentId, CancellationToken cancellationToken)
    {
        return ragManagementStore.PermanentlyDeleteDocument(documentId, cancellationToken);
    }

    [HttpGet("{documentId}")]
    public Task<List<RagChunkDto>> GetChunks(Guid documentId, CancellationToken cancellationToken)
    {
        return ragManagementStore.GetChunks(documentId, cancellationToken);
    }

    [HttpPost]
    public Task<RagRetrievalDebugResultDto> DebugRetrieve(RagRetrievalDebugRequestDto dto, CancellationToken cancellationToken)
    {
        return ragManagementStore.DebugRetrieve(dto, cancellationToken);
    }

    [HttpGet]
    public Task<RagChunkingOptionsDto> GetChunkingOptions(CancellationToken cancellationToken)
    {
        return ragManagementStore.GetChunkingOptions(cancellationToken);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ELEVATED_ACCESS)]
    public Task<RagChunkingOptionsDto> UpdateChunkingOptions(RagChunkingOptionsDto dto, CancellationToken cancellationToken)
    {
        return ragManagementStore.UpdateChunkingOptions(dto, cancellationToken);
    }

    [HttpGet]
    public Task<List<RagRecycleBinItemDto>> GetRecycleBinItems(CancellationToken cancellationToken)
    {
        return ragManagementStore.GetRecycleBinItems(cancellationToken);
    }
}

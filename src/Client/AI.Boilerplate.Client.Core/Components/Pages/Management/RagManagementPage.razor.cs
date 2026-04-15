using AI.Boilerplate.Shared.Features.Rag;
using AI.Boilerplate.Shared.Features.Rag.Dtos;

namespace AI.Boilerplate.Client.Core.Components.Pages.Management;

public partial class RagManagementPage
{
    private const string DocumentsTabKey = "documents";
    private const string ChunksTabKey = "chunks";
    private const string DebuggerTabKey = "debugger";

    [AutoInject] private IRagManagementController ragManagementController = default!;

    private bool isDebugging;
    private bool isLoadingChunkingOptions;
    private bool isLoadingChunks;
    private bool isLoadingDocuments;
    private bool isLoadingKnowledgeBases;
    private bool isLoadingRecycleBin;
    private bool isKnowledgeBaseEditorExpanded = true;
    private bool isDeleteDialogOpen;
    private bool isSavingChunkingOptions;
    private bool isChunkingPanelOpen;
    private bool isRecyclePanelOpen;
    private string? debugQuestion;
    private double debugVectorWeight = 0.85;
    private double debugKeywordWeight
    {
        get => Math.Round(1.0 - debugVectorWeight, 2);
        set => debugVectorWeight = Math.Round(1.0 - value, 2);
    }
    private string newDocumentSourceType = "db";
    private string? newDocumentContent;
    private string? newDocumentSourceId;
    private string? newDocumentTitle;
    private string newEmbeddingDimension = "768";
    private string newEmbeddingModel = "shaw/dmeta-embedding-zh";
    private string? newKnowledgeBaseCode;
    private string? newKnowledgeBaseName;
    private Guid? editingDocumentId;
    private string? editKnowledgeBaseCode;
    private string? editKnowledgeBaseName;
    private string editKnowledgeBaseModel = "shaw/dmeta-embedding-zh";
    private string editKnowledgeBaseDimension = "768";
    private Guid? selectedDocumentId;
    private Guid? selectedKnowledgeBaseId;
    private List<RagChunkDto> chunks = [];
    private List<RagDocumentDto> documents = [];
    private List<RagKnowledgeBaseDto> knowledgeBases = [];
    private List<BitNavItem> knowledgeBaseNavItems = [];
    private RagRetrievalDebugResultDto? retrievalDebugResult;
    private bool isTutorialOpen;
    private bool isAddKnowledgeBaseModalOpen;
    private bool isEditKnowledgeBaseModalOpen;
    private bool isDocumentModalOpen;
    private string selectedPivotKey = DocumentsTabKey;
    private string deleteDialogTitle = string.Empty;
    private string deleteDialogMessage = string.Empty;
    private string? pendingDeleteType;
    private Guid? pendingDeleteId;
    private bool pendingDeleteIsPermanent;
    private int chunkMaxChunkLength = 450;
    private int chunkMinChunkCount = 1;
    private bool chunkPreferParagraphFirst = true;
    private List<RagRecycleBinItemDto> recycleBinItems = [];

    private readonly List<BitDropdownItem<string>> documentSourceTypes =
    [
        new() { Text = "手动", Value = "manual" },
        new() { Text = "数据库", Value = "db" },
        new() { Text = "文件", Value = "file" },
        new() { Text = "链接", Value = "url" }
    ];

    private string GetSourceTypeName(string? type)
    {
        return documentSourceTypes.FirstOrDefault(x => x.Value == type)?.Text ?? type ?? "未知";
    }

    private string HighlightKeywords(string text, string? query)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(query))
            return text;

        var tokens = query.Split(new[] { ' ', '、', '，', '。', ',', '.', '?', '？', '！', '!' }, StringSplitOptions.RemoveEmptyEntries);
        var result = text;
        foreach (var token in tokens.Where(t => t.Length >= 2))
        {
            // Simple case-insensitive replace with <mark>
            result = System.Text.RegularExpressions.Regex.Replace(
                result,
                System.Text.RegularExpressions.Regex.Escape(token),
                $"<mark class=\"rag-highlight\">$&</mark>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
        return result;
    }
    private string GetSourceTypeColorClass(string? type)
    {
        return type switch
        {
            "manual" => "tag-blue",
            "db" => "tag-purple",
            "file" => "tag-green",
            "url" => "tag-orange",
            _ => "tag-gray"
        };
    }

    private string GetScoreBadgeClass(double score)
    {
        if (score >= 0.8) return "high";
        if (score >= 0.5) return "medium";
        return "low";
    }

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        await RefreshKnowledgeBases();
    }

    private int kbCurrentPage = 1;
    private int kbPageSize = 6;

    private int TotalKbPages => (int)Math.Ceiling(knowledgeBases.Count / (double)kbPageSize);

    private IList<BitNavItem> PaginatedKnowledgeBaseNavItems => knowledgeBaseNavItems
        .Skip((kbCurrentPage - 1) * kbPageSize)
        .Take(kbPageSize)
        .ToList();

    private void GoToPreviousKbPage()
    {
        if (kbCurrentPage > 1) kbCurrentPage--;
    }

    private void GoToNextKbPage()
    {
        if (kbCurrentPage < TotalKbPages) kbCurrentPage++;
    }

    private async Task RefreshKnowledgeBases()
    {
        if (isLoadingKnowledgeBases) return;

        try
        {
            isLoadingKnowledgeBases = true;
            knowledgeBases = await ragManagementController.GetKnowledgeBases(CurrentCancellationToken);
            if (kbCurrentPage > TotalKbPages && TotalKbPages > 0)
            {
                kbCurrentPage = TotalKbPages;
            }
            else if (TotalKbPages == 0)
            {
                kbCurrentPage = 1;
            }

            knowledgeBaseNavItems = [.. knowledgeBases.Select(kb => new BitNavItem
            {
                Key = kb.Id.ToString(),
                Text = string.IsNullOrWhiteSpace(kb.Name) ? kb.Code : kb.Name,
                Data = kb,
                IconName = BitIconName.Database
            })];

            if (selectedKnowledgeBaseId is null && knowledgeBases.Count > 0)
            {
                await HandleOnSelectKnowledgeBase(knowledgeBaseNavItems[0]);
            }
            else if (selectedKnowledgeBaseId is not null)
            {
                var selected = knowledgeBaseNavItems.FirstOrDefault(i => i.Key == selectedKnowledgeBaseId.ToString());
                if (selected is not null)
                {
                    await HandleOnSelectKnowledgeBase(selected);
                }
            }
        }
        finally
        {
            isLoadingKnowledgeBases = false;
        }
    }

    private async Task HandleOnSelectKnowledgeBase(BitNavItem? item)
    {
        if (item?.Key is null) return;

        selectedKnowledgeBaseId = Guid.Parse(item.Key);
        selectedDocumentId = null;
        chunks = [];
        retrievalDebugResult = null;
        selectedPivotKey = DocumentsTabKey;
        editingDocumentId = null;

        var selectedKnowledgeBase = knowledgeBases.FirstOrDefault(k => k.Id == selectedKnowledgeBaseId);
        if (selectedKnowledgeBase is not null)
        {
            editKnowledgeBaseCode = selectedKnowledgeBase.Code;
            editKnowledgeBaseName = selectedKnowledgeBase.Name;
            editKnowledgeBaseModel = selectedKnowledgeBase.EmbeddingModel ?? "shaw/dmeta-embedding-zh";
            editKnowledgeBaseDimension = selectedKnowledgeBase.EmbeddingDimension.ToString();
        }

        await LoadDocuments();
    }

    private async Task AddKnowledgeBase()
    {
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        var parsedDimension = int.TryParse(newEmbeddingDimension, out var value) ? value : 768;
        await ragManagementController.CreateKnowledgeBase(new RagKnowledgeBaseUpsertDto
        {
            Code = newKnowledgeBaseCode,
            Name = newKnowledgeBaseName,
            EmbeddingModel = newEmbeddingModel,
            EmbeddingDimension = parsedDimension
        }, CurrentCancellationToken);

        newKnowledgeBaseCode = null;
        newKnowledgeBaseName = null;

        CloseAddKnowledgeBaseModal();
        await RefreshKnowledgeBases();
    }

    private void ToggleKnowledgeBaseEditor()
    {
        isKnowledgeBaseEditorExpanded = !isKnowledgeBaseEditorExpanded;
    }

    private void OpenAddKnowledgeBaseModal()
    {
        newKnowledgeBaseCode = null;
        newKnowledgeBaseName = null;
        newEmbeddingModel = "shaw/dmeta-embedding-zh";
        newEmbeddingDimension = "768";
        isAddKnowledgeBaseModalOpen = true;
    }

    private void CloseAddKnowledgeBaseModal() => isAddKnowledgeBaseModalOpen = false;

    private void OpenEditKnowledgeBaseModal() => isEditKnowledgeBaseModalOpen = true;
    private void CloseEditKnowledgeBaseModal() => isEditKnowledgeBaseModalOpen = false;

    private void OpenAddDocumentModal()
    {
        editingDocumentId = null;
        newDocumentTitle = null;
        newDocumentContent = null;
        newDocumentSourceId = null;
        newDocumentSourceType = "db";
        isDocumentModalOpen = true;
    }

    private void CloseDocumentModal() => isDocumentModalOpen = false;

    private int documentCurrentPage = 1;
    private int documentPageSize = 10;

    private int TotalDocumentPages => (int)Math.Ceiling(documents.Count / (double)documentPageSize);

    private IEnumerable<RagDocumentDto> PaginatedDocuments => documents
        .Skip((documentCurrentPage - 1) * documentPageSize)
        .Take(documentPageSize);

    private void GoToPreviousPage()
    {
        if (documentCurrentPage > 1) documentCurrentPage--;
    }

    private void GoToNextPage()
    {
        if (documentCurrentPage < TotalDocumentPages) documentCurrentPage++;
    }

    private int chunkCurrentPage = 1;
    private int chunkPageSize = 9;

    private int TotalChunkPages => (int)Math.Ceiling(chunks.Count / (double)chunkPageSize);

    private IEnumerable<RagChunkDto> PaginatedChunks => chunks
        .Skip((chunkCurrentPage - 1) * chunkPageSize)
        .Take(chunkPageSize);

    private void GoToPreviousChunkPage()
    {
        if (chunkCurrentPage > 1) chunkCurrentPage--;
    }

    private void GoToNextChunkPage()
    {
        if (chunkCurrentPage < TotalChunkPages) chunkCurrentPage++;
    }

    private async Task RefreshDocuments()
    {
        await LoadDocuments();
    }

    private async Task LoadDocuments()
    {
        if (selectedKnowledgeBaseId is null || isLoadingDocuments) return;

        try
        {
            isLoadingDocuments = true;
            documentCurrentPage = 1;
            documents = await ragManagementController.GetDocuments(selectedKnowledgeBaseId.Value, CurrentCancellationToken);
        }
        finally
        {
            isLoadingDocuments = false;
        }
    }

    private async Task AddDocument()
    {
        if (selectedKnowledgeBaseId is null) return;
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;
        if (string.IsNullOrWhiteSpace(newDocumentTitle)) return;

        var resolvedSourceType = string.IsNullOrWhiteSpace(newDocumentSourceType) ? "manual" : newDocumentSourceType;
        var resolvedSourceId = string.IsNullOrWhiteSpace(newDocumentSourceId)
            ? $"manual:{newDocumentTitle.Trim().Replace(' ', '-')}"
            : newDocumentSourceId;

        var created = await ragManagementController.AddDocument(new RagDocumentUpsertDto
        {
            KnowledgeBaseId = selectedKnowledgeBaseId.Value,
            SourceType = resolvedSourceType,
            SourceId = resolvedSourceId,
            Title = newDocumentTitle,
            Content = newDocumentContent
        }, CurrentCancellationToken);

        newDocumentSourceId = null;
        newDocumentTitle = null;
        newDocumentContent = null;
        editingDocumentId = null;

        CloseDocumentModal();
        await RefreshKnowledgeBases();
        await SelectDocument(created.Id);
    }

    private async Task SelectDocument(Guid documentId)
    {
        selectedDocumentId = documentId;
        selectedPivotKey = ChunksTabKey;
        await LoadChunks();
    }

    private async Task BeginEditDocument(RagDocumentDto item)
    {
        var document = await ragManagementController.GetDocument(item.Id, CurrentCancellationToken);

        editingDocumentId = document.Id;
        newDocumentTitle = document.Title;
        newDocumentSourceType = document.SourceType ?? "manual";
        newDocumentSourceId = document.SourceId;
        newDocumentContent = document.Content;
        isDocumentModalOpen = true;
    }

    private async Task SaveEditedDocument()
    {
        if (editingDocumentId is null) return;
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        var resolvedSourceType = string.IsNullOrWhiteSpace(newDocumentSourceType) ? "manual" : newDocumentSourceType;
        var resolvedSourceId = string.IsNullOrWhiteSpace(newDocumentSourceId)
            ? $"manual:{newDocumentTitle?.Trim().Replace(' ', '-')}"
            : newDocumentSourceId;

        var updated = await ragManagementController.UpdateDocument(editingDocumentId.Value, new RagDocumentUpsertDto
        {
            KnowledgeBaseId = selectedKnowledgeBaseId ?? Guid.Empty,
            SourceType = resolvedSourceType,
            SourceId = resolvedSourceId,
            Title = newDocumentTitle,
            Content = newDocumentContent
        }, CurrentCancellationToken);

        editingDocumentId = null;
        newDocumentSourceId = null;
        newDocumentTitle = null;
        newDocumentContent = null;

        CloseDocumentModal();
        await RefreshKnowledgeBases();
        await SelectDocument(updated.Id);
    }

    private async Task DeleteDocument(Guid documentId)
    {
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        await ragManagementController.DeleteDocument(documentId, CurrentCancellationToken);

        if (selectedDocumentId == documentId)
        {
            selectedDocumentId = null;
            chunks = [];
        }
        if (editingDocumentId == documentId)
        {
            editingDocumentId = null;
            newDocumentSourceId = null;
            newDocumentTitle = null;
            newDocumentContent = null;
        }

        await RefreshKnowledgeBases();
        await LoadDocuments();
        await LoadRecycleBinItems();
    }

    private void CancelEditDocument()
    {
        editingDocumentId = null;
        newDocumentSourceId = null;
        newDocumentTitle = null;
        newDocumentContent = null;
    }

    private async Task UpdateCurrentKnowledgeBase()
    {
        if (selectedKnowledgeBaseId is null) return;
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        var parsedDimension = int.TryParse(editKnowledgeBaseDimension, out var value) ? value : 768;
        await ragManagementController.UpdateKnowledgeBase(selectedKnowledgeBaseId.Value, new RagKnowledgeBaseUpsertDto
        {
            Code = editKnowledgeBaseCode,
            Name = editKnowledgeBaseName,
            EmbeddingModel = editKnowledgeBaseModel,
            EmbeddingDimension = parsedDimension
        }, CurrentCancellationToken);

        CloseEditKnowledgeBaseModal();
        await RefreshKnowledgeBases();
    }

    private async Task DeleteCurrentKnowledgeBase(Guid? targetKnowledgeBaseId = null)
    {
        var id = targetKnowledgeBaseId ?? selectedKnowledgeBaseId;
        if (id is null) return;
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        await ragManagementController.DeleteKnowledgeBase(id.Value, CurrentCancellationToken);

        if (selectedKnowledgeBaseId == id)
            selectedKnowledgeBaseId = null;
        selectedDocumentId = null;
        documents = [];
        chunks = [];
        knowledgeBaseNavItems = [];

        await RefreshKnowledgeBases();
        await LoadRecycleBinItems();
    }

    private void RequestDeleteDocument(Guid documentId)
    {
        pendingDeleteType = "document";
        pendingDeleteId = documentId;
        pendingDeleteIsPermanent = false;
        deleteDialogTitle = "删除文档";
        deleteDialogMessage = "确认删除该文档吗？删除后可在回收站恢复。";
        isDeleteDialogOpen = true;
    }

    private void RequestDeleteKnowledgeBase()
    {
        if (selectedKnowledgeBaseId is null) return;
        pendingDeleteType = "knowledge-base";
        pendingDeleteId = selectedKnowledgeBaseId;
        pendingDeleteIsPermanent = false;
        deleteDialogTitle = "删除知识库";
        deleteDialogMessage = "确认删除当前知识库吗？其下文档会一并进入回收站。";
        isDeleteDialogOpen = true;
    }

    private void RequestPermanentlyDelete(string itemType, Guid itemId, string title)
    {
        pendingDeleteType = itemType;
        pendingDeleteId = itemId;
        pendingDeleteIsPermanent = true;
        deleteDialogTitle = "永久删除";
        deleteDialogMessage = $"确认永久删除“{title}”吗？该操作不可恢复。";
        isDeleteDialogOpen = true;
    }

    private void CancelDeleteDialog()
    {
        isDeleteDialogOpen = false;
        pendingDeleteType = null;
        pendingDeleteId = null;
        pendingDeleteIsPermanent = false;
        deleteDialogTitle = string.Empty;
        deleteDialogMessage = string.Empty;
    }

    private async Task ConfirmDelete()
    {
        if (pendingDeleteId is null || string.IsNullOrWhiteSpace(pendingDeleteType))
        {
            CancelDeleteDialog();
            return;
        }

        if (pendingDeleteIsPermanent)
        {
            if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

            if (pendingDeleteType == "document")
            {
                await ragManagementController.PermanentlyDeleteDocument(pendingDeleteId.Value, CurrentCancellationToken);
            }
            else if (pendingDeleteType == "knowledge-base")
            {
                await ragManagementController.PermanentlyDeleteKnowledgeBase(pendingDeleteId.Value, CurrentCancellationToken);
            }
        }
        else
        {
            if (pendingDeleteType == "document")
            {
                await DeleteDocument(pendingDeleteId.Value);
            }
            else if (pendingDeleteType == "knowledge-base")
            {
                await DeleteCurrentKnowledgeBase(pendingDeleteId.Value);
            }
        }

        CancelDeleteDialog();
        await LoadRecycleBinItems();
        await RefreshKnowledgeBases();
    }

    private async Task OpenRecyclePanel()
    {
        isRecyclePanelOpen = true;
        await LoadRecycleBinItems();
    }

    private void CloseRecyclePanel()
    {
        isRecyclePanelOpen = false;
    }

    private async Task LoadRecycleBinItems()
    {
        if (isLoadingRecycleBin) return;
        try
        {
            isLoadingRecycleBin = true;
            recycleBinItems = await ragManagementController.GetRecycleBinItems(CurrentCancellationToken);
        }
        finally
        {
            isLoadingRecycleBin = false;
        }
    }

    private async Task RestoreRecycleItem(RagRecycleBinItemDto item)
    {
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        if (item.ItemType == "document")
        {
            await ragManagementController.RestoreDocument(item.Id, CurrentCancellationToken);
        }
        else if (item.ItemType == "knowledge-base")
        {
            await ragManagementController.RestoreKnowledgeBase(item.Id, CurrentCancellationToken);
        }

        await RefreshKnowledgeBases();
        await LoadRecycleBinItems();
    }

    private async Task LoadChunks()
    {
        if (selectedDocumentId is null || isLoadingChunks) return;

        try
        {
            isLoadingChunks = true;
            chunkCurrentPage = 1;
            chunks = await ragManagementController.GetChunks(selectedDocumentId.Value, CurrentCancellationToken);
        }
        finally
        {
            isLoadingChunks = false;
        }
    }

    private async Task RunDebugRetrieve()
    {
        if (selectedKnowledgeBaseId is null || string.IsNullOrWhiteSpace(debugQuestion)) return;

        try
        {
            isDebugging = true;
            retrievalDebugResult = await ragManagementController.DebugRetrieve(new RagRetrievalDebugRequestDto
            {
                KnowledgeBaseId = selectedKnowledgeBaseId.Value,
                Question = debugQuestion,
                VectorWeight = debugVectorWeight,
                KeywordWeight = debugKeywordWeight,
                TopK = 8
            }, CurrentCancellationToken);
        }
        finally
        {
            isDebugging = false;
        }
    }

    private void OpenTutorial()
    {
        isTutorialOpen = true;
    }

    private void CloseTutorial()
    {
        isTutorialOpen = false;
    }

    private void JumpToDocuments()
    {
        selectedPivotKey = DocumentsTabKey;
    }

    private void JumpToChunks()
    {
        selectedPivotKey = ChunksTabKey;
    }

    private void JumpToDebugger()
    {
        selectedPivotKey = DebuggerTabKey;
    }

    private void FillTutorialExample()
    {
        newKnowledgeBaseCode = "sales-rag";
        newKnowledgeBaseName = "销售知识库";
        newEmbeddingModel = "shaw/dmeta-embedding-zh";
        newEmbeddingDimension = "768";
        newDocumentTitle = "Orders Table Schema";
        newDocumentSourceId = "public.orders";
        newDocumentContent = """
                             Table orders:
                             - id uuid primary key
                             - created_on timestamptz
                             - status text
                             - total_amount decimal
                             Query tips:
                             - filter by created_on in last 30 days
                             - group by status
                             """;
        newDocumentSourceType = "db";
        debugQuestion = "统计最近30天订单总额，并按状态分组";
        selectedPivotKey = DocumentsTabKey;
    }

    private async Task OpenChunkingPanel()
    {
        isChunkingPanelOpen = true;
        await LoadChunkingOptions();
    }

    private void CloseChunkingPanel()
    {
        isChunkingPanelOpen = false;
    }

    private async Task LoadChunkingOptions()
    {
        if (isLoadingChunkingOptions) return;

        try
        {
            isLoadingChunkingOptions = true;
            var options = await ragManagementController.GetChunkingOptions(CurrentCancellationToken);
            chunkMaxChunkLength = options.MaxChunkLength ?? 0;
            chunkPreferParagraphFirst = options.PreferParagraphFirst ?? true;
            chunkMinChunkCount = options.MinChunkCount ?? 0;
        }
        finally
        {
            isLoadingChunkingOptions = false;
        }
    }

    private async Task SaveChunkingOptions()
    {
        if (isSavingChunkingOptions) return;
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        try
        {
            isSavingChunkingOptions = true;
            var updated = await ragManagementController.UpdateChunkingOptions(new RagChunkingOptionsDto
            {
                MaxChunkLength = chunkMaxChunkLength,
                PreferParagraphFirst = chunkPreferParagraphFirst,
                MinChunkCount = chunkMinChunkCount
            }, CurrentCancellationToken);

            chunkMaxChunkLength = updated.MaxChunkLength ?? 0;
            chunkPreferParagraphFirst = updated.PreferParagraphFirst ?? true;
            chunkMinChunkCount = updated.MinChunkCount ?? 0;
            isChunkingPanelOpen = false;
        }
        finally
        {
            isSavingChunkingOptions = false;
        }
    }
}

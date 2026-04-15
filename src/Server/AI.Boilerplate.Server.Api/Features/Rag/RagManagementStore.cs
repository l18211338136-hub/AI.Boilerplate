using System.Data.Common;
using System.Text;
using AI.Boilerplate.Shared.Features.Rag.Dtos;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagManagementStore
{
    private readonly AppDbContext dbContext;
    private readonly IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator;
    private readonly ServerApiSettings serverApiSettings;
    private readonly IServiceProvider serviceProvider;

    public RagManagementStore(AppDbContext dbContext, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, ServerApiSettings serverApiSettings, IServiceProvider serviceProvider)
    {
        this.dbContext = dbContext;
        this.embeddingGenerator = embeddingGenerator;
        this.serverApiSettings = serverApiSettings;
        this.serviceProvider = serviceProvider;
    }

    public async Task<List<RagKnowledgeBaseDto>> GetKnowledgeBases(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await EnsureSeedDataAsync(cancellationToken);

        var list = await dbContext.RagKnowledgeBases
            .AsNoTracking()
            .Where(k => k.IsDeleted == false)
            .OrderByDescending(k => k.ModifiedOn)
            .Select(k => new RagKnowledgeBaseDto
            {
                Id = k.Id,
                Code = k.Code,
                Name = k.Name,
                EmbeddingModel = k.EmbeddingModel,
                EmbeddingDimension = k.EmbeddingDimension,
                IsEnabled = k.IsEnabled,
                DocumentCount = dbContext.RagDocuments.Count(d => d.KnowledgeBaseId == k.Id && d.IsDeleted == false),
                ChunkCount = dbContext.RagChunks.Count(c => c.Document!.KnowledgeBaseId == k.Id && c.Document.IsDeleted == false),
                ModifiedOn = k.ModifiedOn
            })
            .ToListAsync(cancellationToken);

        return list;
    }

    public async Task<RagKnowledgeBaseDto> CreateKnowledgeBase(RagKnowledgeBaseUpsertDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(dto.Code) || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.EmbeddingModel))
            throw new BadRequestException();

        var code = dto.Code.Trim();
        var codeLower = code.ToLowerInvariant();
        var exists = await dbContext.RagKnowledgeBases.AnyAsync(k => k.Code.ToLower() == codeLower && k.IsDeleted == false, cancellationToken);
        if (exists)
            throw new ConflictException();

        var entity = new RagKnowledgeBase
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = dto.Name.Trim(),
            EmbeddingModel = dto.EmbeddingModel.Trim(),
            EmbeddingDimension = dto.EmbeddingDimension,
            IsEnabled = true
        };

        await dbContext.RagKnowledgeBases.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RagKnowledgeBaseDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            EmbeddingModel = entity.EmbeddingModel,
            EmbeddingDimension = entity.EmbeddingDimension,
            IsEnabled = entity.IsEnabled,
            DocumentCount = 0,
            ChunkCount = 0,
            ModifiedOn = entity.ModifiedOn
        };
    }

    public async Task<RagKnowledgeBaseDto> UpdateKnowledgeBase(Guid knowledgeBaseId, RagKnowledgeBaseUpsertDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(dto.Code) || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.EmbeddingModel))
            throw new BadRequestException();

        var entity = await dbContext.RagKnowledgeBases.FirstOrDefaultAsync(k => k.Id == knowledgeBaseId && k.IsDeleted == false, cancellationToken)
            ?? throw new ResourceNotFoundException();

        var code = dto.Code.Trim();
        var codeLower = code.ToLowerInvariant();
        var hasConflictCode = await dbContext.RagKnowledgeBases.AnyAsync(k => k.Id != knowledgeBaseId && k.Code.ToLower() == codeLower && k.IsDeleted == false, cancellationToken);
        if (hasConflictCode)
            throw new ConflictException();

        entity.Code = code;
        entity.Name = dto.Name.Trim();
        entity.EmbeddingModel = dto.EmbeddingModel.Trim();
        entity.EmbeddingDimension = dto.EmbeddingDimension;
        entity.ModifiedOn = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RagKnowledgeBaseDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            EmbeddingModel = entity.EmbeddingModel,
            EmbeddingDimension = entity.EmbeddingDimension,
            IsEnabled = entity.IsEnabled,
            DocumentCount = await dbContext.RagDocuments.CountAsync(d => d.KnowledgeBaseId == entity.Id && d.IsDeleted == false, cancellationToken),
            ChunkCount = await dbContext.RagChunks.CountAsync(c => c.Document!.KnowledgeBaseId == entity.Id && c.Document.IsDeleted == false, cancellationToken),
            ModifiedOn = entity.ModifiedOn
        };
    }

    public async Task DeleteKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await dbContext.RagKnowledgeBases.FirstOrDefaultAsync(k => k.Id == knowledgeBaseId && k.IsDeleted == false, cancellationToken)
            ?? throw new ResourceNotFoundException();

        entity.IsDeleted = true;
        entity.DeletedOn = DateTimeOffset.UtcNow;
        entity.ModifiedOn = DateTimeOffset.UtcNow;

        var documents = await dbContext.RagDocuments.Where(d => d.KnowledgeBaseId == knowledgeBaseId && d.IsDeleted == false).ToListAsync(cancellationToken);
        foreach (var doc in documents)
        {
            doc.IsDeleted = true;
            doc.DeletedOn = entity.DeletedOn;
            doc.ModifiedOn = entity.DeletedOn.Value;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await dbContext.RagKnowledgeBases.FirstOrDefaultAsync(k => k.Id == knowledgeBaseId && k.IsDeleted, cancellationToken)
            ?? throw new ResourceNotFoundException();

        entity.IsDeleted = false;
        entity.DeletedOn = null;
        entity.ModifiedOn = DateTimeOffset.UtcNow;

        var documents = await dbContext.RagDocuments.Where(d => d.KnowledgeBaseId == knowledgeBaseId && d.IsDeleted).ToListAsync(cancellationToken);
        foreach (var doc in documents)
        {
            doc.IsDeleted = false;
            doc.DeletedOn = null;
            doc.ModifiedOn = entity.ModifiedOn;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task PermanentlyDeleteKnowledgeBase(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await dbContext.RagKnowledgeBases.FirstOrDefaultAsync(k => k.Id == knowledgeBaseId && k.IsDeleted, cancellationToken)
            ?? throw new ResourceNotFoundException();

        dbContext.RagKnowledgeBases.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RagDocumentDto>> GetDocuments(Guid knowledgeBaseId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var exists = await dbContext.RagKnowledgeBases.AnyAsync(k => k.Id == knowledgeBaseId && k.IsDeleted == false, cancellationToken);
        if (exists is false)
            throw new ResourceNotFoundException();

        var list = await dbContext.RagDocuments
            .AsNoTracking()
            .Where(d => d.KnowledgeBaseId == knowledgeBaseId && d.IsDeleted == false)
            .OrderByDescending(d => d.LastIndexedAt)
            .Select(d => new RagDocumentDto
            {
                Id = d.Id,
                KnowledgeBaseId = d.KnowledgeBaseId,
                SourceType = d.SourceType,
                SourceId = d.SourceId,
                Title = d.Title,
                ChunkCount = dbContext.RagChunks.Count(c => c.DocumentId == d.Id),
                ContentPreview = dbContext.RagChunks.Where(c => c.DocumentId == d.Id)
                                                    .OrderBy(c => c.ChunkIndex)
                                                    .Select(c => c.Content)
                                                    .FirstOrDefault(),
                Content = null,
                LastIndexedAt = d.LastIndexedAt
            })
            .ToListAsync(cancellationToken);

        return list;
    }

    public async Task<RagDocumentDto> AddDocument(RagDocumentUpsertDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(dto.SourceType) || string.IsNullOrWhiteSpace(dto.SourceId) || string.IsNullOrWhiteSpace(dto.Title))
            throw new BadRequestException();

        var knowledgeBase = await dbContext.RagKnowledgeBases.FirstOrDefaultAsync(k => k.Id == dto.KnowledgeBaseId && k.IsDeleted == false, cancellationToken)
            ?? throw new ResourceNotFoundException();

        var now = DateTimeOffset.UtcNow;
        var sourceId = dto.SourceId.Trim();

        var document = new RagDocument
        {
            Id = Guid.NewGuid(),
            KnowledgeBaseId = knowledgeBase.Id,
            SourceType = dto.SourceType.Trim(),
            SourceId = sourceId,
            Title = dto.Title.Trim(),
            LastIndexedAt = now
        };
        await dbContext.RagDocuments.AddAsync(document, cancellationToken);

        var chunkingOptions = await GetChunkingOptions(cancellationToken);
        var chunkTexts = BuildChunkTexts(sourceId, dto.Content, chunkingOptions);
        var embeddings = await embeddingGenerator.GenerateAsync(chunkTexts, cancellationToken: cancellationToken);
        var vectors = embeddings.Select(e => Normalize(e.Vector.ToArray())).ToArray();

        var chunkEntities = chunkTexts.Select((text, index) => new RagChunk
        {
            Id = Guid.NewGuid(),
            DocumentId = document.Id,
            ChunkIndex = index,
            Content = text,
            TokenCount = CountTokens(text),
            Embedding = new Vector(vectors[index]),
            CreatedOn = now,
            ModifiedOn = now
        }).ToList();

        await dbContext.RagChunks.AddRangeAsync(chunkEntities, cancellationToken);
        knowledgeBase.ModifiedOn = now;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RagDocumentDto
        {
            Id = document.Id,
            KnowledgeBaseId = document.KnowledgeBaseId,
            SourceType = document.SourceType,
            SourceId = document.SourceId,
            Title = document.Title,
            ChunkCount = chunkEntities.Count,
            ContentPreview = chunkEntities.FirstOrDefault()?.Content,
            Content = string.Join("\n\n", chunkEntities.OrderBy(c => c.ChunkIndex).Select(c => c.Content)),
            LastIndexedAt = document.LastIndexedAt
        };
    }

    public async Task<RagDocumentDto> GetDocument(Guid documentId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var document = await dbContext.RagDocuments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == documentId && d.IsDeleted == false, cancellationToken)
            ?? throw new ResourceNotFoundException();

        var chunkTexts = await dbContext.RagChunks.AsNoTracking()
            .Where(c => c.DocumentId == documentId)
            .OrderBy(c => c.ChunkIndex)
            .Select(c => c.Content)
            .ToListAsync(cancellationToken);

        return new RagDocumentDto
        {
            Id = document.Id,
            KnowledgeBaseId = document.KnowledgeBaseId,
            SourceType = document.SourceType,
            SourceId = document.SourceId,
            Title = document.Title,
            ChunkCount = chunkTexts.Count,
            ContentPreview = chunkTexts.FirstOrDefault(),
            Content = string.Join("\n\n", chunkTexts),
            LastIndexedAt = document.LastIndexedAt
        };
    }

    public async Task<RagDocumentDto> UpdateDocument(Guid documentId, RagDocumentUpsertDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(dto.SourceType) || string.IsNullOrWhiteSpace(dto.SourceId) || string.IsNullOrWhiteSpace(dto.Title))
            throw new BadRequestException();

        var document = await dbContext.RagDocuments.Include(d => d.KnowledgeBase).FirstOrDefaultAsync(d => d.Id == documentId && d.IsDeleted == false, cancellationToken)
            ?? throw new ResourceNotFoundException();

        var now = DateTimeOffset.UtcNow;
        document.SourceType = dto.SourceType.Trim();
        document.SourceId = dto.SourceId.Trim();
        document.Title = dto.Title.Trim();

        if (string.IsNullOrWhiteSpace(dto.Content) is false)
        {
            var oldChunks = await dbContext.RagChunks.Where(c => c.DocumentId == document.Id).ToListAsync(cancellationToken);
            dbContext.RagChunks.RemoveRange(oldChunks);

            var chunkingOptions = await GetChunkingOptions(cancellationToken);
            var chunkTexts = BuildChunkTexts(document.SourceId, dto.Content, chunkingOptions);
            var embeddings = await embeddingGenerator.GenerateAsync(chunkTexts, cancellationToken: cancellationToken);
            var vectors = embeddings.Select(e => Normalize(e.Vector.ToArray())).ToArray();

            var chunkEntities = chunkTexts.Select((text, index) => new RagChunk
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                ChunkIndex = index,
                Content = text,
                TokenCount = CountTokens(text),
                Embedding = new Vector(vectors[index]),
                CreatedOn = now,
                ModifiedOn = now
            }).ToList();

            await dbContext.RagChunks.AddRangeAsync(chunkEntities, cancellationToken);
        }

        document.LastIndexedAt = now;
        if (document.KnowledgeBase is not null)
            document.KnowledgeBase.ModifiedOn = now;

        await dbContext.SaveChangesAsync(cancellationToken);

        return await GetDocument(document.Id, cancellationToken);
    }

    public async Task DeleteDocument(Guid documentId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var document = await dbContext.RagDocuments.FirstOrDefaultAsync(d => d.Id == documentId && d.IsDeleted == false, cancellationToken)
            ?? throw new ResourceNotFoundException();

        document.IsDeleted = true;
        document.DeletedOn = DateTimeOffset.UtcNow;
        document.ModifiedOn = document.DeletedOn.Value;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreDocument(Guid documentId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var document = await dbContext.RagDocuments.Include(d => d.KnowledgeBase).FirstOrDefaultAsync(d => d.Id == documentId && d.IsDeleted, cancellationToken)
            ?? throw new ResourceNotFoundException();
        if (document.KnowledgeBase?.IsDeleted is true)
            throw new BadRequestException();

        document.IsDeleted = false;
        document.DeletedOn = null;
        document.ModifiedOn = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task PermanentlyDeleteDocument(Guid documentId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var document = await dbContext.RagDocuments.FirstOrDefaultAsync(d => d.Id == documentId && d.IsDeleted, cancellationToken)
            ?? throw new ResourceNotFoundException();

        dbContext.RagDocuments.Remove(document);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RagRecycleBinItemDto>> GetRecycleBinItems(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var deletedKnowledgeBases = await dbContext.RagKnowledgeBases
            .AsNoTracking()
            .Where(k => k.IsDeleted)
            .Select(k => new RagRecycleBinItemDto
            {
                ItemType = "knowledge-base",
                Id = k.Id,
                KnowledgeBaseId = k.Id,
                Title = k.Name,
                Description = k.Code,
                DeletedOn = k.DeletedOn ?? k.ModifiedOn
            })
            .ToListAsync(cancellationToken);

        var deletedDocuments = await dbContext.RagDocuments
            .AsNoTracking()
            .Where(d => d.IsDeleted)
            .Select(d => new RagRecycleBinItemDto
            {
                ItemType = "document",
                Id = d.Id,
                KnowledgeBaseId = d.KnowledgeBaseId,
                Title = d.Title,
                Description = $"{d.SourceType}/{d.SourceId}",
                DeletedOn = d.DeletedOn ?? d.ModifiedOn
            })
            .ToListAsync(cancellationToken);

        return [.. deletedKnowledgeBases.Concat(deletedDocuments).OrderByDescending(x => x.DeletedOn)];
    }

    public async Task<List<RagChunkDto>> GetChunks(Guid documentId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var exists = await dbContext.RagDocuments.AnyAsync(d => d.Id == documentId && d.IsDeleted == false, cancellationToken);
        if (exists is false)
            throw new ResourceNotFoundException();

        return await dbContext.RagChunks
            .AsNoTracking()
            .Where(c => c.DocumentId == documentId)
            .OrderBy(c => c.ChunkIndex)
            .Select(c => new RagChunkDto
            {
                Id = c.Id,
                DocumentId = c.DocumentId,
                ChunkIndex = c.ChunkIndex,
                Content = c.Content,
                TokenCount = c.TokenCount,
                CreatedOn = c.CreatedOn
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<RagRetrievalDebugResultDto> DebugRetrieve(RagRetrievalDebugRequestDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(dto.Question))
            throw new BadRequestException();

        var exists = await dbContext.RagKnowledgeBases.AnyAsync(k => k.Id == dto.KnowledgeBaseId && k.IsDeleted == false, cancellationToken);
        if (exists is false)
            throw new ResourceNotFoundException();

        var questionEmbedding = await embeddingGenerator.GenerateAsync(dto.Question, cancellationToken: cancellationToken);
        var queryVector = new Vector(Normalize(questionEmbedding.Vector.ToArray()));
        var queryKeywords = Tokenize(dto.Question);
        var ragOptions = serverApiSettings.RagRetrieval;
        var topK = Math.Clamp(dto.TopK, 1, ragOptions.MaxTopK);
        var candidateCount = Math.Clamp(topK * ragOptions.CandidateMultiplier, topK, ragOptions.CandidateCap);
        
        var vWeight = dto.VectorWeight ?? ragOptions.VectorWeight;
        var kWeight = dto.KeywordWeight ?? ragOptions.KeywordWeight;
        var weightSum = vWeight + kWeight;
        var vectorWeight = weightSum > 0 ? vWeight / weightSum : 0.85;
        var keywordWeight = weightSum > 0 ? kWeight / weightSum : 0.15;

        var candidates = await dbContext.RagChunks
            .AsNoTracking()
            .Where(c => c.Document!.KnowledgeBaseId == dto.KnowledgeBaseId && c.Document.IsDeleted == false && c.Embedding != null)
            .OrderBy(c => c.Embedding!.CosineDistance(queryVector))
            .ThenBy(c => c.ChunkIndex)
            .Take(candidateCount)
            .Select(c => new
            {
                c.Id,
                c.DocumentId,
                c.ChunkIndex,
                c.Content,
                Distance = c.Embedding!.CosineDistance(queryVector)
            })
            .ToListAsync(cancellationToken);

        var hits = candidates
            .Select(c =>
            {
                var vectorScore = Math.Clamp(1 - c.Distance, 0, 1);
                var keywordScore = ComputeKeywordScore(c.Content, queryKeywords);
                var finalScore = Math.Round((vectorScore * vectorWeight) + (keywordScore * keywordWeight), 4);
                return new RagRetrievalHitDto
                {
                    ChunkId = c.Id,
                    DocumentId = c.DocumentId,
                    ChunkIndex = c.ChunkIndex,
                    Score = finalScore,
                    VectorScore = Math.Round(vectorScore, 4),
                    KeywordScore = Math.Round(keywordScore, 4),
                    ContentPreview = c.Content
                };
            })
            .OrderByDescending(h => h.Score)
            .ThenBy(h => h.ChunkIndex)
            .Take(topK)
            .ToList();

        return new RagRetrievalDebugResultDto
        {
            Hits = hits,
            ContextPreview = string.Join("\n\n", hits.Select((h, index) => $"[{index + 1}] score={h.Score:F4}\n{h.ContentPreview}")),
            VectorWeight = Math.Round(vectorWeight, 4),
            KeywordWeight = Math.Round(keywordWeight, 4),
            CandidateCount = candidates.Count
        };
    }

    public Task<RagChunkingOptionsDto> GetChunkingOptions(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return GetOrCreateChunkingOptions(cancellationToken);
    }

    public async Task<RagChunkingOptionsDto> UpdateChunkingOptions(RagChunkingOptionsDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var setting = await dbContext.RagChunkingSettings.FirstOrDefaultAsync(cancellationToken);
        if (setting is null)
        {
            var defaults = serverApiSettings.RagChunking;
            setting = new RagChunkingSetting
            {
                Id = Guid.NewGuid(),
                MaxChunkLength = defaults.MaxChunkLength,
                PreferParagraphFirst = defaults.PreferParagraphFirst,
                MinChunkCount = defaults.MinChunkCount
            };
            await dbContext.RagChunkingSettings.AddAsync(setting, cancellationToken);
        }

        setting.MaxChunkLength = Math.Clamp(dto.MaxChunkLength, 100, 4000);
        setting.PreferParagraphFirst = dto.PreferParagraphFirst;
        setting.MinChunkCount = Math.Clamp(dto.MinChunkCount, 1, 20);
        setting.ModifiedOn = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RagChunkingOptionsDto
        {
            MaxChunkLength = setting.MaxChunkLength,
            PreferParagraphFirst = setting.PreferParagraphFirst,
            MinChunkCount = setting.MinChunkCount
        };
    }

    /// <summary>
    /// 生成数据库 Schema 知识库切片（按表切片，包含 Schema 和外键关系）
    /// </summary>
    public async Task GenerateSchemaSlicesAsync(CancellationToken cancellationToken = default)
    {
        const string KB_NAME = "数据库 Schema 知识库";

        // 1. 幂等性检查
        if (await dbContext.RagKnowledgeBases.AsNoTracking().AnyAsync(x => x.Name == KB_NAME, cancellationToken))
        {
            Console.WriteLine($"[Schema Generator] {KB_NAME} 已存在，跳过初始化。");
            return;
        }

        var now = DateTimeOffset.UtcNow;

        try
        {
            // 2. 创建知识库和文档
            var knowledgeBase = new RagKnowledgeBase
            {
                Id = Guid.CreateVersion7(),
                Code = "SYSTEM_SCHEMA",
                Name = KB_NAME,
                EmbeddingModel = "shaw/dmeta-embedding-zh",
                EmbeddingDimension = 768,
                IsEnabled = true,
                CreatedOn = now,
                ModifiedOn = now
            };
            await dbContext.RagKnowledgeBases.AddAsync(knowledgeBase);
            await dbContext.SaveChangesAsync(cancellationToken);

            var document = new RagDocument
            {
                Id = Guid.CreateVersion7(),
                KnowledgeBaseId = knowledgeBase.Id,
                SourceType = "db",
                SourceId = "db:schema",
                Title = "数据库 Schema 结构",
                CreatedOn = now,
                ModifiedOn = now
            };

            await dbContext.RagDocuments.AddAsync(document);
            await dbContext.SaveChangesAsync(cancellationToken);

            // 3. 执行 SQL 查询 Schema 信息
            var flatData = new List<SchemaInfoTempDto>();

            await using var scope = serviceProvider.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            await using var command = connection.CreateCommand();

            command.CommandText = @"
WITH foreign_keys AS (
    SELECT 
        con.conname AS constraint_name,
        con.conrelid AS referencing_table_id, 
        con.confrelid AS referenced_table_id, 
        unnest(con.conkey) AS referencing_attnum,    
        unnest(con.confkey) AS referenced_attnum     
    FROM pg_catalog.pg_constraint con
    WHERE con.contype = 'f'
)
SELECT
    n.nspname AS table_schema,
    c.relname AS table_name,
    obj_description(c.oid) AS table_comment,
    a.attname AS column_name,
    pg_catalog.format_type(a.atttypid, a.atttypmod) AS data_type,
    a.attnotnull AS is_required,
    col_description(a.attrelid, a.attnum) AS column_comment,
    CASE 
        WHEN fk.referencing_table_id IS NOT NULL 
        THEN ref_n.nspname || '.' || ref_c.relname
        ELSE NULL 
    END AS referenced_table_fullname
FROM pg_catalog.pg_class c
JOIN pg_catalog.pg_namespace n ON c.relnamespace = n.oid
JOIN pg_catalog.pg_attribute a ON a.attrelid = c.oid
LEFT JOIN foreign_keys fk ON fk.referencing_table_id = c.oid AND fk.referencing_attnum = a.attnum
LEFT JOIN pg_catalog.pg_class ref_c ON ref_c.oid = fk.referenced_table_id
LEFT JOIN pg_catalog.pg_namespace ref_n ON ref_n.oid = ref_c.relnamespace
WHERE
    n.nspname NOT IN ('pg_catalog', 'information_schema','jobs')
    AND c.relname NOT IN ('__EFMigrationsHistory','RagChunkingSettings','RagChunks','RagDocuments','RagKnowledgeBases','SystemPrompts','WebAuthnCredential')
    AND a.attnum > 0
    AND NOT a.attisdropped
    AND c.relkind = 'r' 
ORDER BY
    n.nspname, c.relname, a.attnum";

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                flatData.Add(new SchemaInfoTempDto
                {
                    TableSchema = reader.GetString(reader.GetOrdinal("table_schema")),
                    TableName = reader.GetString(reader.GetOrdinal("table_name")),
                    TableComment = reader.IsDBNull(reader.GetOrdinal("table_comment")) ? null : reader.GetString(reader.GetOrdinal("table_comment")),
                    ColumnName = reader.GetString(reader.GetOrdinal("column_name")),
                    DataType = reader.GetString(reader.GetOrdinal("data_type")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required")),
                    ColumnComment = reader.IsDBNull(reader.GetOrdinal("column_comment")) ? null : reader.GetString(reader.GetOrdinal("column_comment")),
                    ForeignKeyRelation = reader.IsDBNull(reader.GetOrdinal("referenced_table_fullname"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("referenced_table_fullname"))
                });
            }

            if (!flatData.Any())
            {
                Console.WriteLine("[Schema Generator] 未查询到任何表结构信息。");
                return;
            }

            // 4. 准备切片数据
            var chunksToInsert = new List<RagChunk>();

            // 按 Schema + 表名分组
            var tables = flatData.GroupBy(x => new { x.TableSchema, x.TableName });

            int chunkIndex = 0;
            foreach (var tableGroup in tables)
            {
                var md = new StringBuilder();
                var fullTableName = $"{tableGroup.Key.TableSchema}.{tableGroup.Key.TableName}";

                // 表头
                md.AppendLine($"# 数据库表结构: {fullTableName}");

                // 业务描述
                var tableComment = tableGroup.First().TableComment;
                if (!string.IsNullOrWhiteSpace(tableComment))
                {
                    var safeComment = tableComment.Replace("|", "\\|");
                    md.AppendLine($"> **业务描述**: {safeComment}");
                }

                // 字段详情表
                md.AppendLine();
                md.AppendLine("### 字段详情");
                // 修改点：表头改为“字段说明/关联”
                md.AppendLine("| 字段名 | 类型 | 必填 | 字段说明/关联 |");
                md.AppendLine("| :--- | :--- | :--- | :--- |");

                foreach (var col in tableGroup)
                {
                    var required = col.IsRequired ? "是" : "否";

                    // 修改点：逻辑调整，将外键信息拼接到说明中
                    var comment = string.IsNullOrWhiteSpace(col.ColumnComment)
                        ? ""
                        : col.ColumnComment.Replace("|", "\\|");

                    var relation = string.IsNullOrWhiteSpace(col.ForeignKeyRelation)
                        ? ""
                        : $"关联到: {col.ForeignKeyRelation.Replace("|", "\\|")}";

                    // 组合说明和关联信息
                    string description;
                    if (!string.IsNullOrEmpty(comment) && !string.IsNullOrEmpty(relation))
                    {
                        description = $"{comment}; {relation}";
                    }
                    else
                    {
                        description = comment + relation;
                    }

                    if (string.IsNullOrEmpty(description))
                    {
                        description = "-";
                    }

                    md.AppendLine($"| `{col.ColumnName}` | `{col.DataType}` | {required} | {description} |");
                }

                var text = md.ToString().Replace("\r\n", "\n").Replace("\r", "\n");

                var embeddings = await embeddingGenerator.GenerateAsync(text, cancellationToken: cancellationToken);

                chunksToInsert.Add(new RagChunk
                {
                    Id = Guid.CreateVersion7(),
                    DocumentId = document.Id,
                    ChunkIndex = chunkIndex,
                    Content = text,
                    TokenCount = CountTokens(text),
                    Embedding = new Vector(Normalize(embeddings.Vector.ToArray())),
                    CreatedOn = now,
                    ModifiedOn = now
                });
                chunkIndex++;
            }

            await dbContext.AddRangeAsync(chunksToInsert);
            await dbContext.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"[Schema Generator] 成功初始化 {tables.Count()} 张表，生成 {chunksToInsert.Count} 个切片。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Schema Generator] 初始化失败: {ex.Message}");
        }
    }

    // 用于接收 SQL 查询结果的临时类 (Keyless Entity)
    public class SchemaInfoTempDto
    {
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string TableComment { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsRequired { get; set; }
        public string ColumnComment { get; set; }
        public string ForeignKeyRelation { get; set; }
    }

    private async Task EnsureSeedDataAsync(CancellationToken cancellationToken)
    {
       await GenerateSchemaSlicesAsync(cancellationToken);
    }

    private static string[] CreateDefaultChunkTexts(string sourceId)
    {
        return
        [
            $"Source {sourceId}: primary entities and relationships summary.",
            $"Source {sourceId}: key fields, constraints and index hints for SQL generation.",
            $"Source {sourceId}: filtering and join examples used by retrieval debugging."
        ];
    }

    private string[] BuildChunkTexts(string sourceId, string? content, RagChunkingOptionsDto chunking)
    {
        if (string.IsNullOrWhiteSpace(content))
            return CreateDefaultChunkTexts(sourceId);

        var normalized = content.Replace("\r\n", "\n").Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            return CreateDefaultChunkTexts(sourceId);

        var maxChunkLength = Math.Clamp(chunking.MaxChunkLength, 100, 4000);
        var minChunkCount = Math.Clamp(chunking.MinChunkCount, 1, 20);

        var units = chunking.PreferParagraphFirst
            ? normalized.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            : normalized.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var cleanedUnits = units.Select(u => u.Trim())
                                .Where(u => u.Length > 0)
                                .ToArray();
        if (cleanedUnits.Length == 0)
            return CreateDefaultChunkTexts(sourceId);

        var chunks = MergeUnitsByLength(cleanedUnits, maxChunkLength);

        if (chunks.Count < minChunkCount)
        {
            chunks = SplitByTargetCount(normalized, maxChunkLength, minChunkCount);
        }

        return chunks.Count == 0 ? CreateDefaultChunkTexts(sourceId) : chunks.ToArray();
    }

    private async Task<RagChunkingOptionsDto> GetOrCreateChunkingOptions(CancellationToken cancellationToken)
    {
        var setting = await dbContext.RagChunkingSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        if (setting is not null)
        {
            return new RagChunkingOptionsDto
            {
                MaxChunkLength = setting.MaxChunkLength,
                PreferParagraphFirst = setting.PreferParagraphFirst,
                MinChunkCount = setting.MinChunkCount
            };
        }

        var defaults = serverApiSettings.RagChunking;
        var created = new RagChunkingSetting
        {
            Id = Guid.NewGuid(),
            MaxChunkLength = defaults.MaxChunkLength,
            PreferParagraphFirst = defaults.PreferParagraphFirst,
            MinChunkCount = defaults.MinChunkCount,
            ModifiedOn = DateTimeOffset.UtcNow
        };
        await dbContext.RagChunkingSettings.AddAsync(created, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RagChunkingOptionsDto
        {
            MaxChunkLength = created.MaxChunkLength,
            PreferParagraphFirst = created.PreferParagraphFirst,
            MinChunkCount = created.MinChunkCount
        };
    }

    private static List<string> MergeUnitsByLength(IEnumerable<string> units, int maxChunkLength)
    {
        var builder = new StringBuilder();
        var chunks = new List<string>();

        foreach (var unit in units)
        {
            var prepared = unit.Replace("\n", " ").Trim();
            if (prepared.Length == 0)
                continue;

            if (builder.Length + prepared.Length + 1 > maxChunkLength && builder.Length > 0)
            {
                chunks.Add(builder.ToString());
                builder.Clear();
            }

            if (builder.Length > 0)
                builder.Append(' ');

            if (prepared.Length > maxChunkLength)
            {
                var segmented = SplitLongText(prepared, maxChunkLength);
                if (builder.Length > 0)
                {
                    chunks.Add(builder.ToString());
                    builder.Clear();
                }

                chunks.AddRange(segmented);
            }
            else
            {
                builder.Append(prepared);
            }
        }

        if (builder.Length > 0)
            chunks.Add(builder.ToString());

        return chunks;
    }

    private static List<string> SplitByTargetCount(string normalized, int maxChunkLength, int minChunkCount)
    {
        var targetLength = Math.Max(100, Math.Min(maxChunkLength, (int)Math.Ceiling((double)normalized.Length / minChunkCount)));
        return SplitLongText(normalized, targetLength);
    }

    private static List<string> SplitLongText(string text, int segmentLength)
    {
        var chunks = new List<string>();
        var working = text.Replace("\n", " ").Trim();

        var start = 0;
        while (start < working.Length)
        {
            var length = Math.Min(segmentLength, working.Length - start);
            var end = start + length;

            if (end < working.Length)
            {
                var lastSpace = working.LastIndexOf(' ', end - 1, length);
                if (lastSpace > start + (segmentLength / 3))
                    end = lastSpace + 1;
            }

            var piece = working[start..end].Trim();
            if (piece.Length > 0)
                chunks.Add(piece);

            start = end;
        }

        return chunks;
    }

    private static float[] Normalize(float[] vector)
    {
        var sum = 0d;
        for (var i = 0; i < vector.Length; i++)
        {
            sum += vector[i] * vector[i];
        }

        var norm = Math.Sqrt(sum);
        if (norm <= 0)
            return vector;

        for (var i = 0; i < vector.Length; i++)
        {
            vector[i] = (float)(vector[i] / norm);
        }

        return vector;
    }

    private static int CountTokens(string text)
    {
        return text.Split([' ', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries).Length;
    }

    private static HashSet<string> Tokenize(string text)
    {
        return [.. text.Split([' ', ',', '.', ';', ':', '-', '_', '/', '\\', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries)
                       .Select(t => t.Trim().ToLowerInvariant())
                       .Where(t => t.Length > 1)];
    }

    private static double ComputeKeywordScore(string content, HashSet<string> queryKeywords)
    {
        if (queryKeywords.Count == 0 || string.IsNullOrWhiteSpace(content))
            return 0;

        var chunkKeywords = Tokenize(content);
        if (chunkKeywords.Count == 0)
            return 0;

        var matched = queryKeywords.Count(chunkKeywords.Contains);
        return (double)matched / queryKeywords.Count;
    }

    private async Task<DbConnection> OpenAppDbConnectionAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var connection = db.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);
        return connection;
    }
}


using AI.Boilerplate.Shared.Features.Todo;
using AI.Boilerplate.Shared.Features.Dashboard;
using AI.Boilerplate.Shared.Features.Products;
using AI.Boilerplate.Shared.Features.Categories;
using AI.Boilerplate.Shared.Features.Chatbot;
using AI.Boilerplate.Shared.Infrastructure.Dtos.SignalR;
using AI.Boilerplate.Shared.Features.Statistics;
using AI.Boilerplate.Shared.Features.Diagnostic;
using AI.Boilerplate.Shared.Features.Rag.Dtos;

namespace AI.Boilerplate.Shared.Infrastructure.Dtos;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(



  PropertyNameCaseInsensitive = true,
  PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
  DictionaryKeyPolicy = JsonKnownNamingPolicy.CamelCase,
  UseStringEnumConverter = true,
  WriteIndented = false,
  GenerationMode = JsonSourceGenerationMode.Metadata,
  AllowTrailingCommas = true,
  DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
)]


[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
[JsonSerializable(typeof(Dictionary<string, string?>))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(Guid[]))]
[JsonSerializable(typeof(GitHubStats))]
[JsonSerializable(typeof(NugetStatsDto))]
[JsonSerializable(typeof(AppProblemDetails))]
[JsonSerializable(typeof(PushNotificationSubscriptionDto))]
[JsonSerializable(typeof(TodoItemDto))]
[JsonSerializable(typeof(PagedResponse<TodoItemDto>))]
[JsonSerializable(typeof(List<TodoItemDto>))]
[JsonSerializable(typeof(CategoryDto))]
[JsonSerializable(typeof(List<CategoryDto>))]
[JsonSerializable(typeof(PagedResponse<CategoryDto>))]
[JsonSerializable(typeof(ProductDto))]
[JsonSerializable(typeof(List<ProductDto>))]
[JsonSerializable(typeof(PagedResponse<ProductDto>))]
[JsonSerializable(typeof(List<ProductsCountPerCategoryResponseDto>))]
[JsonSerializable(typeof(OverallAnalyticsStatsDataResponseDto))]
[JsonSerializable(typeof(List<ProductPercentagePerCategoryResponseDto>))]

[JsonSerializable(typeof(DiagnosticLogDto[]))]
[JsonSerializable(typeof(StartChatRequest))]
[JsonSerializable(typeof(List<SystemPromptDto>))]
[JsonSerializable(typeof(RagKnowledgeBaseDto))]
[JsonSerializable(typeof(List<RagKnowledgeBaseDto>))]
[JsonSerializable(typeof(RagKnowledgeBaseUpsertDto))]
[JsonSerializable(typeof(RagDocumentDto))]
[JsonSerializable(typeof(List<RagDocumentDto>))]
[JsonSerializable(typeof(RagDocumentUpsertDto))]
[JsonSerializable(typeof(RagChunkDto))]
[JsonSerializable(typeof(List<RagChunkDto>))]
[JsonSerializable(typeof(RagRetrievalDebugRequestDto))]
[JsonSerializable(typeof(RagRetrievalDebugResultDto))]
[JsonSerializable(typeof(RagChunkingOptionsDto))]
[JsonSerializable(typeof(List<RagRecycleBinItemDto>))]
[JsonSerializable(typeof(BackgroundJobProgressDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}

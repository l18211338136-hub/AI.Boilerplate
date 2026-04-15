using AI.Boilerplate.Shared.Features.Chatbot;
using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Chatbot;

public class SystemPrompt : AuditEntity
{
    public Guid Id { get; set; }

    public PromptKind? PromptKind { get; set; }

    public string? Markdown { get; set; }

    public long Version { get; set; }
}

using AI.Boilerplate.Shared.Features.Chatbot;

namespace AI.Boilerplate.Server.Api.Features.Chatbot;

public class SystemPrompt
{
    public Guid Id { get; set; }

    public PromptKind PromptKind { get; set; }

    [Required]
    public string? Markdown { get; set; }

    public long Version { get; set; }
}

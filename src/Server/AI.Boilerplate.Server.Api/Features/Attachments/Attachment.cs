using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using AI.Boilerplate.Shared.Features.Attachments;

namespace AI.Boilerplate.Server.Api.Features.Attachments;

public partial class Attachment : AuditEntity
{
    public Guid Id { get; set; }

    public AttachmentKind Kind { get; set; }

    public string? Path { get; set; }
}

using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Todo;

public partial class TodoItem : AuditEntity
{
    public string Id { get; set; }

        public string? Title { get; set; }
    public bool? IsDone { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    public Guid? UserId { get; set; }
}

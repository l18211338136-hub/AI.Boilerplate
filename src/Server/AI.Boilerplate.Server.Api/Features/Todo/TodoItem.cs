using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Todo;

public partial class TodoItem
{
    public new string Id { get; set; }
    public new DateTimeOffset? UpdatedAt { get; set; }

    [Required]
    public string? Title { get; set; }
    public bool IsDone { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    public Guid UserId { get; set; }
}

using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Identity.Models;

public class UserLogin : IdentityUserLogin<Guid>, IAuditableEntity, ISoftDelete
{
    public User? User { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}

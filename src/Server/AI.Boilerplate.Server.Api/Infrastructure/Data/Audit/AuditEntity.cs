namespace AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

public abstract class AuditEntity : IAuditableEntity, ISoftDelete
{
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}

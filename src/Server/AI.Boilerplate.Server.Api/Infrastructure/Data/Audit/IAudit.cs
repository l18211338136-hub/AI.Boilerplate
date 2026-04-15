namespace AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

public interface IAuditableEntity
{
    DateTimeOffset? CreatedOn { get; set; }
    Guid? CreatedBy { get; set; }
    DateTimeOffset? ModifiedOn { get; set; }
    Guid? ModifiedBy { get; set; }
}

public interface ISoftDelete
{
    bool? IsDeleted { get; set; }
    DateTimeOffset? DeletedOn { get; set; }
    Guid? DeletedBy { get; set; }
}

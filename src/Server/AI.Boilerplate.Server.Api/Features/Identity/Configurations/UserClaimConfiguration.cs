using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public partial class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable(t => t.HasComment("用户声明表"));
        builder.Property(userClaim => userClaim.Id).HasComment("主键ID");
        builder.Property(userClaim => userClaim.UserId).HasComment("用户ID");
        builder.Property(userClaim => userClaim.ClaimType).HasComment("声明类型");
        builder.Property(userClaim => userClaim.ClaimValue).HasComment("声明值");
        
        builder.Property(userClaim => userClaim.CreatedOn).HasComment("创建时间");
        builder.Property(userClaim => userClaim.CreatedBy).HasComment("创建人ID");
        builder.Property(userClaim => userClaim.ModifiedOn).HasComment("最后修改时间");
        builder.Property(userClaim => userClaim.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(userClaim => userClaim.IsDeleted).HasComment("是否删除");
        builder.Property(userClaim => userClaim.DeletedOn).HasComment("删除时间");
        builder.Property(userClaim => userClaim.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(userClaim => new { userClaim.UserId, userClaim.ClaimType, userClaim.ClaimValue });
    }
}

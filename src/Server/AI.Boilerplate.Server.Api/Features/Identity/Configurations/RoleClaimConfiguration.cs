using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public partial class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(t => t.HasComment("角色声明表"));
        builder.Property(roleClaim => roleClaim.Id).HasComment("主键ID");
        builder.Property(roleClaim => roleClaim.RoleId).HasComment("角色ID");
        builder.Property(roleClaim => roleClaim.ClaimType).HasComment("声明类型");
        builder.Property(roleClaim => roleClaim.ClaimValue).HasComment("声明值");
        
        builder.Property(roleClaim => roleClaim.CreatedOn).HasComment("创建时间");
        builder.Property(roleClaim => roleClaim.CreatedBy).HasComment("创建人ID");
        builder.Property(roleClaim => roleClaim.ModifiedOn).HasComment("最后修改时间");
        builder.Property(roleClaim => roleClaim.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(roleClaim => roleClaim.IsDeleted).HasComment("是否删除");
        builder.Property(roleClaim => roleClaim.DeletedOn).HasComment("删除时间");
        builder.Property(roleClaim => roleClaim.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(roleClaim => new { roleClaim.RoleId, roleClaim.ClaimType, roleClaim.ClaimValue });
    }
}

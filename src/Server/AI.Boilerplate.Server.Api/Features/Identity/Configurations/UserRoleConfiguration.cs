using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(t => t.HasComment("用户角色关联表"));
        builder.Property(userRole => userRole.UserId).HasComment("用户ID");
        builder.Property(userRole => userRole.RoleId).HasComment("角色ID");
        
        builder.Property(userRole => userRole.CreatedOn).HasComment("创建时间");
        builder.Property(userRole => userRole.CreatedBy).HasComment("创建人ID");
        builder.Property(userRole => userRole.ModifiedOn).HasComment("最后修改时间");
        builder.Property(userRole => userRole.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(userRole => userRole.IsDeleted).HasComment("是否删除");
        builder.Property(userRole => userRole.DeletedOn).HasComment("删除时间");
        builder.Property(userRole => userRole.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(userRole => new { userRole.RoleId, userRole.UserId }).IsUnique();

    }
}

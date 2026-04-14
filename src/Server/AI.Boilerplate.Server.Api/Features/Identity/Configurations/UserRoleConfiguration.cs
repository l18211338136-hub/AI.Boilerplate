using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(t => t.HasComment("用户角色关联表"));
        builder.Property(userRole => userRole.UserId).HasComment("用户ID");
        builder.Property(userRole => userRole.RoleId).HasComment("角色ID");

        builder.HasIndex(userRole => new { userRole.RoleId, userRole.UserId }).IsUnique();

    }
}

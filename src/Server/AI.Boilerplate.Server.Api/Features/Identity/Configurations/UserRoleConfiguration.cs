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

        var superAdminRoleId = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7");
        var defaultTestUserId = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7");

        builder.HasData(new UserRole { RoleId = superAdminRoleId, UserId = defaultTestUserId });
    }
}

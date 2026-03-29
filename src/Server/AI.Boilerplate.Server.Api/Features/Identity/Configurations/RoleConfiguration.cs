using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public partial class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(t => t.HasComment("角色表"));
        builder.Property(role => role.Id).HasComment("角色ID");
        builder.Property(role => role.Name).HasComment("角色名称");
        builder.Property(role => role.NormalizedName).HasComment("规范化角色名称");
        builder.Property(role => role.ConcurrencyStamp).HasComment("并发戳");

        builder.HasIndex(role => role.Name).IsUnique();
        builder.Property(role => role.Name).HasMaxLength(50);

        builder.HasMany(role => role.Users)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId);

        builder.HasMany(role => role.Claims)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId);

        builder.HasData(new Role { Id = Guid.Parse("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), Name = AppRoles.SuperAdmin, NormalizedName = AppRoles.SuperAdmin.ToUpperInvariant(), ConcurrencyStamp = "8ff71671-a1d6-5f97-abb9-d87d7b47d6e7" });

        builder.HasData(new Role { Id = Guid.Parse("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8"), Name = AppRoles.Demo, NormalizedName = AppRoles.Demo.ToUpperInvariant(), ConcurrencyStamp = "9ff71672-a1d5-4f97-abb7-d87d6b47d5e8" });
    }
}

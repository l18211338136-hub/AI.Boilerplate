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

    }
}

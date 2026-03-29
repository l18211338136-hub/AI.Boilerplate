using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Shared.Features.Identity.Dtos;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(t => t.HasComment("用户表"));
        builder.Property(user => user.Id).HasComment("用户ID");
        builder.Property(user => user.FullName).HasComment("姓名");
        builder.Property(user => user.Gender).HasComment("性别");
        builder.Property(user => user.BirthDate).HasComment("出生日期");
        builder.Property(user => user.EmailTokenRequestedOn).HasComment("邮箱验证码请求时间");
        builder.Property(user => user.PhoneNumberTokenRequestedOn).HasComment("手机号验证码请求时间");
        builder.Property(user => user.ResetPasswordTokenRequestedOn).HasComment("重置密码令牌请求时间");
        builder.Property(user => user.TwoFactorTokenRequestedOn).HasComment("双因子验证码请求时间");
        builder.Property(user => user.OtpRequestedOn).HasComment("一次性密码请求时间");
        builder.Property(user => user.ElevatedAccessTokenRequestedOn).HasComment("提权令牌请求时间");
        builder.Property(user => user.HasProfilePicture).HasComment("是否有头像");

        builder.HasMany(user => user.Roles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(user => user.Claims)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(user => user.Tokens)
            .WithOne(ut => ut.User)
            .HasForeignKey(ut => ut.UserId);

        builder.HasMany(user => user.Logins)
            .WithOne(ul => ul.User)
            .HasForeignKey(ul => ul.UserId);

        const string userName = "test";
        const string email = "761516331@qq.com";

        builder.HasData([new User
        {
            Id = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"),
            EmailConfirmed = true,
            LockoutEnabled = true,
            Gender = Gender.Other,
            BirthDate = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
            FullName = "AI.Boilerplate test account",
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            EmailTokenRequestedOn = new DateTimeOffset(new DateOnly(2023, 1, 1), default, default),
            PhoneNumber = "+31684207362",
            PhoneNumberConfirmed = true,
            SecurityStamp = "959ff4a9-4b07-4cc1-8141-c5fc033daf83",
            ConcurrencyStamp = "315e1a26-5b3a-4544-8e91-2760cd28e231",
            PasswordHash = "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", // 123456
        }]);

        builder
            .HasIndex(b => b.Email)
            .HasFilter($"'{nameof(User.Email)}' IS NOT NULL")
            .IsUnique();

        builder
            .HasIndex(b => b.PhoneNumber)
            .HasFilter($"'{nameof(User.PhoneNumber)}' IS NOT NULL")
            .IsUnique();
    }
}

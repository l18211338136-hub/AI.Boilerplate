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

        // ASP.NET Core Identity 内置字段注释
        builder.Property(user => user.UserName).HasComment("用户名");
        builder.Property(user => user.NormalizedUserName).HasComment("标准化用户名");
        builder.Property(user => user.Email).HasComment("邮箱地址");
        builder.Property(user => user.NormalizedEmail).HasComment("标准化邮箱地址");
        builder.Property(user => user.EmailConfirmed).HasComment("邮箱是否已确认");
        builder.Property(user => user.PasswordHash).HasComment("密码哈希值");
        builder.Property(user => user.SecurityStamp).HasComment("安全戳(凭据变更时刷新)");
        builder.Property(user => user.ConcurrencyStamp).HasComment("并发戳(乐观并发控制)");
        builder.Property(user => user.PhoneNumber).HasComment("手机号码");
        builder.Property(user => user.PhoneNumberConfirmed).HasComment("手机号是否已确认");
        builder.Property(user => user.TwoFactorEnabled).HasComment("是否启用双因子认证");
        builder.Property(user => user.LockoutEnd).HasComment("账号锁定结束时间");
        builder.Property(user => user.LockoutEnabled).HasComment("是否允许账号锁定");
        builder.Property(user => user.AccessFailedCount).HasComment("登录失败次数");
        
        builder.Property(user => user.CreatedOn).HasComment("创建时间");
        builder.Property(user => user.CreatedBy).HasComment("创建人ID");
        builder.Property(user => user.ModifiedOn).HasComment("最后修改时间");
        builder.Property(user => user.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(user => user.IsDeleted).HasComment("是否删除");
        builder.Property(user => user.DeletedOn).HasComment("删除时间");
        builder.Property(user => user.DeletedBy).HasComment("删除人ID");

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

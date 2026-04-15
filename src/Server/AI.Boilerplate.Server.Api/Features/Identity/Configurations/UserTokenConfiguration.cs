using AI.Boilerplate.Server.Api.Features.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(t => t.HasComment("用户令牌表(存储双因子等验证码/令牌)"));

        builder.Property(t => t.UserId).HasComment("关联的用户ID");
        builder.Property(t => t.LoginProvider).HasComment("登录提供商(如: Default, Authenticator)");
        builder.Property(t => t.Name).HasComment("令牌名称(如: PasswordResetToken, TwoFactorToken)");
        builder.Property(t => t.Value).HasComment("令牌的具体值");
        
        builder.Property(t => t.CreatedOn).HasComment("创建时间");
        builder.Property(t => t.CreatedBy).HasComment("创建人ID");
        builder.Property(t => t.ModifiedOn).HasComment("最后修改时间");
        builder.Property(t => t.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");
    }
}

using AI.Boilerplate.Server.Api.Features.Identity.Models;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public class WebAuthnCredentialConfiguration : IEntityTypeConfiguration<WebAuthnCredential>
{
    public void Configure(EntityTypeBuilder<WebAuthnCredential> builder)
    {
        builder.ToTable(t => t.HasComment("WebAuthn凭据表"));
        builder.Property(t => t.Id).HasComment("主键ID");
        builder.Property(t => t.UserId).HasComment("用户ID");
        builder.Property(t => t.PublicKey).HasComment("公钥");
        builder.Property(t => t.SignCount).HasComment("签名计数器");
        builder.Property(t => t.AaGuid).HasComment("认证器AAGUID");
        builder.Property(t => t.Transports).HasComment("传输通道");
        builder.Property(t => t.IsBackupEligible).HasComment("是否支持备份");
        builder.Property(t => t.IsBackedUp).HasComment("是否已备份");
        builder.Property(t => t.AttestationObject).HasComment("认证对象");
        builder.Property(t => t.AttestationClientDataJson).HasComment("认证客户端数据");
        builder.Property(t => t.UserHandle).HasComment("用户句柄");
        builder.Property(t => t.AttestationFormat).HasComment("认证格式");
        builder.Property(t => t.RegDate).HasComment("注册时间");

        builder.HasOne(t => t.User)
            .WithMany(u => u.WebAuthnCredentials)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

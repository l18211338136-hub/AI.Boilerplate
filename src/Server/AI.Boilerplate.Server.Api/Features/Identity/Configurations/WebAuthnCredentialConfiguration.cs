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
        builder.Property(t => t.CreatedOn).HasComment("创建时间");
        builder.Property(t => t.CreatedBy).HasComment("创建人ID");
        builder.Property(t => t.ModifiedOn).HasComment("最后修改时间");
        builder.Property(t => t.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(t => t.IsDeleted).HasComment("是否删除");
        builder.Property(t => t.DeletedOn).HasComment("删除时间");
        builder.Property(t => t.DeletedBy).HasComment("删除人ID");

        builder.HasOne(t => t.User)
            .WithMany(u => u.WebAuthnCredentials)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

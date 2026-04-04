using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public class DataProtectionKeyConfiguration : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
    {
        builder.ToTable(t => t.HasComment("数据保护密钥表(用于存储ASP.NET Core加密凭据/Cookie的密钥)"));

        builder.Property(k => k.Id).HasComment("密钥ID");
        builder.Property(k => k.FriendlyName).HasComment("密钥友好名称");
        builder.Property(k => k.Xml).HasComment("序列化后的XML格式密钥内容");
    }
}

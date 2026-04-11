using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Addresses;

public partial class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(t => t.HasComment("用户收货/账单地址表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.UserId).HasComment("用户ID");
        builder.Property(p => p.RecipientName).HasComment("收件人姓名");
        builder.Property(p => p.PhoneNumber).HasComment("联系电话");
        builder.Property(p => p.Province).HasComment("省");
        builder.Property(p => p.City).HasComment("市");
        builder.Property(p => p.District).HasComment("区/县");
        builder.Property(p => p.StreetAddress).HasComment("详细地址");
        builder.Property(p => p.PostalCode).HasComment("邮政编码");
        builder.Property(p => p.IsDefault).HasComment("是否默认地址");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.UpdatedOn).HasComment("更新时间");

        // 为每个用户配置默认地址的唯一性
        builder.HasIndex(p => new { p.UserId, p.IsDefault })
            .HasFilter("\"IsDefault\" = true")
            .IsUnique();
    }
}

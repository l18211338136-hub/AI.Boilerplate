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
        builder.Property(p => p.CreatedBy).HasComment("创建人ID");
        builder.Property(p => p.ModifiedOn).HasComment("最后修改时间");
        builder.Property(p => p.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");

        // 为每个用户配置默认地址的唯一性
        builder.HasIndex(p => new { p.UserId, p.IsDefault })
            .HasFilter("\"IsDefault\" = true")
            .IsUnique();
    }
}

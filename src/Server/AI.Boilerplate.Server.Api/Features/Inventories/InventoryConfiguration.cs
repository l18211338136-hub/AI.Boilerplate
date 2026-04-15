using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Inventories;

public partial class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable(t => t.HasComment("产品库存表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.ProductId).HasComment("产品ID");
        builder.Property(p => p.StockQuantity).HasComment("当前可用库存");
        builder.Property(p => p.ReservedQuantity).HasComment("预占库存(已下单未支付)");
        builder.Property(p => p.LowStockThreshold).HasComment("低库存预警阈值");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CreatedBy).HasComment("创建人ID");
        builder.Property(p => p.ModifiedOn).HasComment("最后修改时间");
        builder.Property(p => p.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");

        // 产品ID唯一索引，每个产品只能有一个库存记录
        builder.HasIndex(p => p.ProductId).IsUnique();
    }
}

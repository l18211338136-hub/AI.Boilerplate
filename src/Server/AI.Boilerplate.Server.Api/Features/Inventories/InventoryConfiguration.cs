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
        builder.Property(p => p.UpdatedOn).HasComment("最后更新时间");

        // 产品ID唯一索引，每个产品只能有一个库存记录
        builder.HasIndex(p => p.ProductId).IsUnique();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Orders;

public partial class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(t => t.HasComment("订单明细表（快照数据）"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.OrderId).HasComment("订单ID");
        builder.Property(p => p.ProductId).HasComment("产品ID");
        builder.Property(p => p.ProductName).HasComment("下单时产品名称快照");
        builder.Property(p => p.UnitPrice).HasComment("下单时单价快照");
        builder.Property(p => p.Quantity).HasComment("购买数量");
        builder.Property(p => p.SubTotal).HasComment("行小计金额");
        builder.Property(p => p.PrimaryImageAltText).HasComment("下单时主图替代文本快照");

        // 订单ID索引，用于查询订单的所有商品
        builder.HasIndex(p => p.OrderId);
        // 产品ID索引，用于查询产品的销售记录
        builder.HasIndex(p => p.ProductId);
    }
}

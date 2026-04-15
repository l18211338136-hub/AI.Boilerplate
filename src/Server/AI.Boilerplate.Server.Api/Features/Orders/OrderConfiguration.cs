using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Orders;

public partial class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(t => t.HasComment("订单主表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.OrderNo).HasComment("订单业务编号(唯一)");
        builder.Property(p => p.UserId).HasComment("用户ID");
        builder.Property(p => p.TotalAmount).HasComment("商品原价总额");
        builder.Property(p => p.DiscountAmount).HasComment("优惠抵扣金额");
        builder.Property(p => p.ShippingFee).HasComment("运费");
        builder.Property(p => p.PayableAmount).HasComment("实际应付金额");
        builder.Property(p => p.Status).HasComment("订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)");
        builder.Property(p => p.AddressId).HasComment("收货地址ID");
        builder.Property(p => p.Remark).HasComment("买家备注");
        builder.Property(p => p.PaidOn).HasComment("支付成功时间");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CreatedBy).HasComment("创建人ID");
        builder.Property(p => p.ModifiedOn).HasComment("最后修改时间");
        builder.Property(p => p.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除标识");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");

        // 订单号唯一索引
        builder.HasIndex(p => p.OrderNo).IsUnique();
        // 用户ID索引，用于查询用户的所有订单
        builder.HasIndex(p => p.UserId);
        // 状态索引，用于查询不同状态的订单
        builder.HasIndex(p => p.Status);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Payments;

public partial class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(t => t.HasComment("支付流水记录表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.OrderId).HasComment("订单ID");
        builder.Property(p => p.UserId).HasComment("用户ID");
        builder.Property(p => p.Amount).HasComment("实际支付金额");
        builder.Property(p => p.PaymentMethod).HasComment("支付渠道(Alipay, WeChatPay, UnionPay等)");
        builder.Property(p => p.TransactionId).HasComment("第三方支付平台流水号");
        builder.Property(p => p.Status).HasComment("支付状态(0:待支付 1:成功 2:失败 3:已退款)");
        builder.Property(p => p.PaidOn).HasComment("支付成功时间");
        builder.Property(p => p.CreatedOn).HasComment("记录创建时间");

        // 订单ID索引，用于查询订单的所有支付记录
        builder.HasIndex(p => p.OrderId);
        // 用户ID索引，用于查询用户的所有支付记录
        builder.HasIndex(p => p.UserId);
        // 交易ID索引，用于查询第三方支付平台的交易记录
        builder.HasIndex(p => p.TransactionId);
        // 状态索引，用于查询不同状态的支付记录
        builder.HasIndex(p => p.Status);
    }
}

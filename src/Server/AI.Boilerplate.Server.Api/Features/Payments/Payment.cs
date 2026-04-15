using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Server.Api.Features.Orders;

namespace AI.Boilerplate.Server.Api.Features.Payments;

public partial class Payment : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    public Guid? OrderId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public Guid? UserId { get; set; }

    [Precision(18, 3)]
    [Comment("实际支付金额")]
    public decimal? Amount { get; set; }

    [MaxLength(32)]
    [Comment("支付渠道(Alipay, WeChatPay, UnionPay等)")]
    public string? PaymentMethod { get; set; }

    [MaxLength(128)]
    [Comment("第三方支付平台流水号")]
    public string? TransactionId { get; set; }

    [Comment("支付状态(0:待支付 1:成功 2:失败 3:已退款)")]
    public short? Status { get; set; }

    [Comment("支付成功时间")]
    public DateTimeOffset? PaidOn { get; set; }

}

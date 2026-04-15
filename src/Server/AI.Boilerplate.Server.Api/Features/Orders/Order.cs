using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Addresses;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Server.Api.Features.Payments;
using AI.Boilerplate.Server.Api.Features.ProductReviews;

namespace AI.Boilerplate.Server.Api.Features.Orders;

public partial class Order : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(32)]
    [Comment("订单业务编号(唯一)")]
    public string? OrderNo { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public Guid UserId { get; set; }

    [Required]
    [Precision(18, 3)]
    [Comment("商品原价总额")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Precision(18, 3)]
    [Comment("优惠抵扣金额")]
    public decimal DiscountAmount { get; set; }

    [Required]
    [Precision(18, 3)]
    [Comment("运费")]
    public decimal ShippingFee { get; set; }

    [Required]
    [Precision(18, 3)]
    [Comment("实际应付金额")]
    public decimal PayableAmount { get; set; }

    [Required]
    [Comment("订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)")]
    public short Status { get; set; }

    [Required]
    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

    public Guid AddressId { get; set; }

    [MaxLength(512)]
    [Comment("买家备注")]
    public string? Remark { get; set; }

    [Comment("支付成功时间")]
    public DateTimeOffset? PaidOn { get; set; }

    // 导航属性
    public IList<OrderItem> OrderItems { get; set; } = [];
    public IList<ProductReview> ProductReviews { get; set; } = [];
    public IList<Payment> Payments { get; set; } = [];
}

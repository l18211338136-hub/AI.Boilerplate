using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api.Features.Orders;

public partial class OrderItem : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    public Guid OrderId { get; set; }

    [Required]
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid ProductId { get; set; }

    [Required]
    [MaxLength(128)]
    [Comment("下单时产品名称快照")]
    public string? ProductName { get; set; }

    [Required]
    [Precision(18, 3)]
    [Comment("下单时单价快照")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Comment("购买数量")]
    public int Quantity { get; set; }

    [Required]
    [Precision(18, 3)]
    [Comment("行小计金额")]
    public decimal SubTotal { get; set; }

    [Comment("下单时主图替代文本快照")]
    public string? PrimaryImageAltText { get; set; }
}

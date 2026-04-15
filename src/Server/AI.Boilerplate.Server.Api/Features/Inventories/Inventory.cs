using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api.Features.Inventories;

public partial class Inventory : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid ProductId { get; set; }

    [Required]
    [Comment("当前可用库存")]
    public int StockQuantity { get; set; }

    [Required]
    [Comment("预占库存(已下单未支付)")]
    public int ReservedQuantity { get; set; }

    [Required]
    [Comment("低库存预警阈值")]
    public int LowStockThreshold { get; set; }

}

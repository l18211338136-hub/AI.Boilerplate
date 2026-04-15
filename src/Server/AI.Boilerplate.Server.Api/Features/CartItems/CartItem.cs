using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api.Features.CartItems;

public partial class CartItem : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public Guid? UserId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid? ProductId { get; set; }

    [Comment("数量")]
    public int? Quantity { get; set; }

    [Comment("结算是否勾选")]
    public bool? Selected { get; set; }

}

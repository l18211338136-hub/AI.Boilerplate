using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api.Features.CartItems;

public partial class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public Guid UserId { get; set; }

    [Required]
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid ProductId { get; set; }

    [Required]
    [Comment("数量")]
    public int Quantity { get; set; }

    [Required]
    [Comment("结算是否勾选")]
    public bool Selected { get; set; }

    [Required]
    [Comment("加入购物车时间")]
    public DateTimeOffset AddedOn { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Comment("更新时间")]
    public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.UtcNow;
}

using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Server.Api.Features.Orders;
using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api.Features.ProductReviews;

public partial class ProductReview : AuditEntity
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
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public Guid UserId { get; set; }

    [Required]
    [Comment("评分(1-5)")]
    public short Rating { get; set; }

    [MaxLength(1024)]
    [Comment("评价内容")]
    public string? Comment { get; set; }

    [Required]
    [Comment("是否匿名显示")]
    public bool IsAnonymous { get; set; }

}

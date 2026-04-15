using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AI.Boilerplate.Server.Api.Features.Products;

public partial class ProductImage : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid ProductId { get; set; }

    [Required]
    [MaxLength(512)]
    [Comment("图片存储URL")]
    public string? ImageUrl { get; set; }

    [MaxLength(128)]
    [Comment("图片替代文本")]
    public string? AltText { get; set; }

    [Required]
    [Comment("排序权重(升序)")]
    public int SortOrder { get; set; }

    [Required]
    [Comment("是否为主图")]
    public bool IsPrimary { get; set; }

}

using AI.Boilerplate.Server.Api.Features.Categories;

using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Products;

public partial class Product : AuditEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// The product's ShortId is used to create a more human-friendly URL.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int? ShortId { get; set; }

    [MaxLength(64)]
    public string? Name { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }

    [MaxLength(4096)]
    public string? DescriptionHTML { get; set; }

    [MaxLength(4096)]
    public string? DescriptionText { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    public Guid? CategoryId { get; set; }

    public long Version { get; set; }

    public bool? HasPrimaryImage { get; set; }

    public string? PrimaryImageAltText { get; set; }

    public Pgvector.Vector? Embedding { get; set; }
}

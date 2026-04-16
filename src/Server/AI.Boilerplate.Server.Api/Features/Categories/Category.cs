using AI.Boilerplate.Server.Api.Features.Products;

using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.Categories;

public partial class Category : AuditEntity
{
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string? Name { get; set; }

    public string? Color { get; set; }
    
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }

    public long Version { get; set; }

    public IList<Product> Products { get; set; } = [];
}

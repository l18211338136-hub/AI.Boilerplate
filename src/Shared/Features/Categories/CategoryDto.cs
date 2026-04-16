namespace AI.Boilerplate.Shared.Features.Categories;

[DtoResourceType(typeof(AppStrings))]
public partial class CategoryDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Name))]
    [MaxLength(64, ErrorMessage = nameof(AppStrings.MaxLengthAttribute_InvalidMaxLength))]
    public string? Name { get; set; }

    [Display(Name = nameof(AppStrings.Color))]
    public string? Color { get; set; } = "#FFFFFF";
    
    public Guid? ParentId { get; set; }

    public List<CategoryDto> Children { get; set; } = [];

    public int ProductsCount { get; set; }

    public long Version { get; set; }
}

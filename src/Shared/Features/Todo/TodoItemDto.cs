namespace AI.Boilerplate.Shared.Features.Todo;

[DtoResourceType(typeof(AppStrings))]
public partial class TodoItemDto
{
    public new string Id { get; set; }
    public new DateTimeOffset? UpdatedAt { get; set; }

    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    [Display(Name = nameof(AppStrings.Title))]
    public string? Title { get; set; }

    public bool IsDone { get; set; }

    [JsonIgnore, NotMapped]
    public bool IsInEditMode { get; set; }
}

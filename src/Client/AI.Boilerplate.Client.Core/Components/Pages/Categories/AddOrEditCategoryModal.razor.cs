using AI.Boilerplate.Shared.Features.Categories;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace AI.Boilerplate.Client.Core.Components.Pages.Categories;

public partial class AddOrEditCategoryModal
{
    [AutoInject] ICategoryController categoryController = default!;

    [Parameter] public EventCallback OnSave { get; set; }

    private bool isOpen;
    private bool isSaving;
    private bool isColorPickerOpen;
    private CategoryDto category = new();
    private EditForm editForm = default!;
    private AppDataAnnotationsValidator validatorRef = default!;
    private List<BitDropdownItem<Guid?>> parentCategories = [];

    private bool isChanged => editForm?.EditContext?.IsModified() is true;

    public async Task ShowModal(CategoryDto categoryToShow)
    {
        await InvokeAsync(async () =>
        {
            categoryToShow.Patch(category);
            category.ParentId = categoryToShow.ParentId; // Explicitly copy ParentId to ensure it's not missed
            
            await LoadParentCategories();
            
            isOpen = true;
            StateHasChanged();
        });
    }

    private async Task LoadParentCategories()
    {
        var allCategories = await categoryController.Get(CurrentCancellationToken);
        
        // Filter out current category and all its descendants to prevent circular references
        var descendants = new HashSet<Guid>();
        if (category.Id != default)
        {
            GetDescendants(allCategories, category.Id, descendants);
        }

        var availableCategories = allCategories
            .Where(c => c.Id != category.Id && descendants.Contains(c.Id) == false)
            .ToList();
        
        parentCategories = [new BitDropdownItem<Guid?> { Text = Localizer[nameof(AppStrings.None)], Value = null }];
        
        BuildTree(availableCategories, null, 0);
    }

    private void GetDescendants(List<CategoryDto> all, Guid parentId, HashSet<Guid> descendants)
    {
        var childrenIds = all.Where(c => c.ParentId == parentId).Select(c => c.Id);
        foreach (var id in childrenIds)
        {
            if (descendants.Add(id))
            {
                GetDescendants(all, id, descendants);
            }
        }
    }

    private void BuildTree(List<CategoryDto> all, Guid? parentId, int level)
    {
        var children = all.Where(c => c.ParentId == parentId).OrderBy(c => c.Name);
        foreach (var child in children)
        {
            parentCategories.Add(new BitDropdownItem<Guid?>
            {
                Text = new string('\u00A0', level * 4) + child.Name,
                Value = child.Id
            });
            BuildTree(all, child.Id, level + 1);
        }
    }

    private void SetCategoryColor(string color)
    {
        category.Color = color;
    }

    private async Task Save()
    {
        if (isSaving) return;

        isSaving = true;

        try
        {
            if (category.Id == default)
            {
                await categoryController.Create(category, CurrentCancellationToken);
            }
            else
            {
                await categoryController.Update(category, CurrentCancellationToken);
            }

            await OnSave.InvokeAsync();
            isOpen = false;
        }
        catch (ResourceValidationException e)
        {
            validatorRef.DisplayErrors(e);
        }
        catch (KnownException e)
        {
            SnackBarService.Error(e.Message);
        }
        finally
        {
            isSaving = false;
        }
    }

    private void OnNavigation(LocationChangingContext args)
    {
        args.PreventNavigation();
        if (isChanged) return;
        isOpen = false;
    }
}

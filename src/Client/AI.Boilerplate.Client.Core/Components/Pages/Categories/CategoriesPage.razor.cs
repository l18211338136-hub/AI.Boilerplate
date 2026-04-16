using AI.Boilerplate.Shared.Features.Categories;

namespace AI.Boilerplate.Client.Core.Components.Pages.Categories;

public class CategoryDisplayItem
{
    public CategoryDto Category { get; set; } = default!;
    public int Level { get; set; }
}

public partial class CategoriesPage
{
    private bool isLoading;
    private bool isDeleteDialogOpen;
    private CategoryDto? deletingCategory;
    private AddOrEditCategoryModal? modal;
    private string categoryNameFilter = string.Empty;

    private BitDataGrid<CategoryDisplayItem>? dataGrid;
    private BitDataGridItemsProvider<CategoryDisplayItem> categoriesProvider = default!;
    private BitDataGridPaginationState pagination = new() { ItemsPerPage = 10 };


    [AutoInject] ICategoryController categoryController = default!;


    private string CategoryNameFilter
    {
        get => categoryNameFilter;
        set
        {
            categoryNameFilter = value;
            _ = RefreshData();
        }
    }


    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        PrepareGridDataProvider();
    }


    private List<CategoryDisplayItem> flattenedCategories = [];

    private void PrepareGridDataProvider()
    {
        categoriesProvider = async req =>
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                var allCategories = await categoryController.Get(req.CancellationToken);
                
                flattenedCategories.Clear();
                BuildFlattenedTree(allCategories, null, 0);

                var query = flattenedCategories.AsQueryable();

                if (string.IsNullOrEmpty(CategoryNameFilter) is false)
                {
                    query = query.Where(i => i.Category.Name!.Contains(CategoryNameFilter, StringComparison.OrdinalIgnoreCase));
                }

                var totalCount = query.Count();
                var items = query.Skip(req.StartIndex).Take(req.Count ?? 10).ToArray();

                return BitDataGridItemsProviderResult.From(items, totalCount);
            }
            catch (Exception exp)
            {
                ExceptionHandler.Handle(exp);
                return BitDataGridItemsProviderResult.From(new List<CategoryDisplayItem> { }, 0);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        };
    }

    private void BuildFlattenedTree(List<CategoryDto> all, Guid? parentId, int level)
    {
        var children = all.Where(c => c.ParentId == parentId).OrderBy(c => c.Name);
        foreach (var child in children)
        {
            flattenedCategories.Add(new CategoryDisplayItem
            {
                Category = child,
                Level = level
            });
            BuildFlattenedTree(all, child.Id, level + 1);
        }
    }

    private async Task RefreshData()
    {
        await dataGrid!.RefreshDataAsync();
    }

    private async Task CreateCategory()
    {
        await modal!.ShowModal(new CategoryDto());
    }

    private async Task EditCategory(CategoryDto category)
    {
        await modal!.ShowModal(category);
    }

    private async Task DeleteCategory()
    {
        if (deletingCategory is null) return;

        try
        {
            await categoryController.Delete(deletingCategory.Id, deletingCategory.Version, CurrentCancellationToken);

            await RefreshData();
        }
        catch (KnownException exp)
        {
            ExceptionHandler.Handle(exp);
        }
        finally
        {
            deletingCategory = null;
        }
    }
}





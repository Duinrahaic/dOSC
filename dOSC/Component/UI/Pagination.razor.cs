using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI;

public partial class Pagination<T>
{
    [Parameter] public RenderFragment TableHeaders { get; set; }
    [Parameter] public RenderFragment PaginationHeader { get; set; }
    [Parameter] public RenderFragment? EmptyContent { get; set; } = null;
    [Parameter] public RenderFragment<T> ChildContent { get; set; }
    [Parameter] public List<T> Items { get; set; }
    [Parameter] public int ItemsPerPage { get; set; }
    [Parameter] public int CurrentPage { get; set; }
    [Parameter] public string RowHeight { get; set; } = "auto";
    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }
    [Parameter] public Func<T, bool> Filter { get; set; } = _ => true;

    private IEnumerable<T> FilteredItems => Items.Where(Filter);

    private int TotalPages => (int)Math.Ceiling((double)FilteredItems.Count() / ItemsPerPage);

    private async Task SetPage(int page)
    {
        if (page < 1 || page > TotalPages) return;
        CurrentPage = page;
        await CurrentPageChanged.InvokeAsync(page);
    }

    private IEnumerable<T> PaginatedItems => FilteredItems
        .Skip((CurrentPage - 1) * ItemsPerPage)
        .Take(ItemsPerPage);

    protected override void OnParametersSet()
    {
        if (CurrentPage > TotalPages)
        {
            CurrentPage = TotalPages > 0 ? TotalPages : 1;
        }
    }
}
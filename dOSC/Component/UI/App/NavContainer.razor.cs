using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace dOSC.Component.UI.App;

public partial class NavContainer : IDisposable
{
    [Parameter] public List<NavItem> Apps { get; set; } = new();

    [Parameter] public NavItem? SelectedItem { get; set; }

    [Inject] private NavigationManager Nm { get; set; } 

 
    [Parameter] public RenderFragment? Indicators { get; set; }

    public void Dispose()
    {
        Nm!.LocationChanged -= NavContainer_LocationChanged;
    }

    protected override void OnInitialized()
    {
        Nm!.LocationChanged += NavContainer_LocationChanged;
    }

    private void NavContainer_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Update();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender) Update();
    }

    private void Update()
    {
        var route = Nm.Uri.Replace(Nm.BaseUri, "");
        if (route.ToLower().StartsWith("apps"))
            SelectedItem = Apps.FirstOrDefault(x => x.Name.ToLower() == "apps");
        else if (route.ToLower().StartsWith("settings"))
            SelectedItem = Apps.FirstOrDefault(x => x.Name.ToLower() == "settings");
        else if (route.ToLower().StartsWith("editor"))
            SelectedItem = Apps.FirstOrDefault(x => x.Name.ToLower() == "editor");
        else
            SelectedItem = null;
        StateHasChanged();
    }


    private void OnNavItemSelected(NavItem item)
    {
        if (SelectedItem?.Name == item.Name)
            return;
        Update();
        Nm!.NavigateTo($"{item.Navigation}");
    }
}
using dOSC.Component.Wiresheet;
using dOSC.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Component.UI.App;

public partial class AppProfileItem
{
    [Inject] public NavigationManager NM { get; set; } = default!;

    [Inject] public IJSRuntime JS { get; set; } = default!;

    [Parameter] public WiresheetDiagram App { get; set; } = new();

    [Parameter] public EventCallback<WiresheetDiagram> AppChanged { get; set; }

    [Parameter] public EventCallback<WiresheetDiagram> OnShowSettings { get; set; }

    [Parameter] public EventCallback<WiresheetDiagram> OnPlayPause { get; set; }


    private async Task TogglePlayPause()
    {
        if (App.State == LiveSheet.LiveSheetState.Loaded)
            App.Unload(); 
        else
            App.Load(); 

        await OnPlayPause.InvokeAsync(App);
    }

    private void ShowSettings() => OnShowSettings.InvokeAsync(App);

    private async Task DownloadApp() => await JS.DownloadApp(App);

    private void EditApp() => NM.NavigateTo($"/appeditor/{App.Guid}");
}
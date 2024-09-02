using System.Runtime.InteropServices;
using dOSC.Component.Wiresheet;
using dOSC.Drivers;
using dOSC.Utilities;
using LiveSheet;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Component.UI.App;

public partial class AppProfileItem : IDisposable
{
    [Inject] private NavigationManager Nm { get; set; } = default!;
    [Inject] private IJSRuntime Js { get; set; } = default!;
    [Inject] private WiresheetService Engine { get; set; } = default!;
    [Parameter] public required WiresheetDiagram App { get; set; } 
    [Parameter] public required EventCallback<WiresheetDiagram> OnShowSettings { get; set; }

    protected override void OnInitialized()
    {
        App.OnSheetStateChange += OnUpdate;
    }

    private void TogglePlayPause()
    {
        try
        {
            if (App.State == LiveSheet.LiveSheetState.Loaded)
                Engine.StopApp(App.Guid);
            else
                Engine.StartApp(App.Guid);
        }
        catch
        {
           // ignore 
        }
  
    }

    private string GetAppStateIcon()
    {
        return App.State == LiveSheetState.Loaded ? "fa-solid fa-play" : "fa-solid fa-stop";
    }
    
    

    private void OnUpdate(LiveSheetState state)
    {
        if (state != App.State)
        {
            InvokeAsync(StateHasChanged);
        }
    } 

    private void ShowSettings() => OnShowSettings.InvokeAsync(App);

    private async Task DownloadApp() => await Js.DownloadApp(App);

    private void EditApp() => Nm.NavigateTo($"/editor/{App.Guid}");

    public void Dispose()
    {
        App.OnSheetStateChange -= OnUpdate;
    }
}
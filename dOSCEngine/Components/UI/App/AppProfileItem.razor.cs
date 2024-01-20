using dOSCEngine.Engine;
using dOSCEngine.Services;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSCEngine.Components.UI.App
{
    public partial class AppProfileItem
    {
        [Inject]
        public NavigationManager? NM { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        [Parameter]
        public AppLogic App { get; set; }
        [Parameter]
        public EventCallback<AppLogic> AppChanged { get; set; }
        [Parameter]
        public EventCallback<AppLogic> OnShowSettings { get; set; }
        [Parameter]
        public EventCallback<AppLogic> OnPlayPause { get; set; }



        private async Task TogglePlayPause()
        {
            if (App != null)
            {
                if (App.IsEnabled())
                {
                    await App.DisableApp();
                }
                else
                {
                    await App.EnableApp();
                }

                await OnPlayPause.InvokeAsync(App);
            }
        }

        private void ShowSettings()
        {
            if (App != null)
            {
                OnShowSettings.InvokeAsync(App);
            }
        }

        private async Task DownloadApp()
        {
            if (App != null)
            {
                await JS.DownloadApp(App);
            }
        }

        private void EditApp()
        {
            if (NM != null)
            {
                if (App != null)
                {
                    NM.NavigateTo($"editor/{App.AppGuid}");
                }
            }
        }
    }
}

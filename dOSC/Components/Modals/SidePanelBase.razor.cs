using dOSCEngine.Services;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;



namespace dOSC.Components.Modals
{
    public partial class SidePanelBase
    {
        [Inject]
        private dOSCService? Engine { get; set; }
        [Inject]
        private IJSRuntime? JS { get; set; }


        [Parameter]
        public dOSCWiresheet dOSCWiresheet { get; set; }

        [Parameter]
        public EventCallback<dOSCWiresheet> OnUpdate { get; set; }

        private bool Show = false;
        private bool CloseAnimation = false;

        public void Open()
        {
            Show = true;
            this.StateHasChanged();
        }
        public async Task Close() 
        {
            CloseAnimation = true;
            await Task.Delay(250);
            Show = false;
            CloseAnimation = false;
        }

        private async Task DownloadApp()
        {
            if (dOSCWiresheet != null)
            {

                await FileSystem.DownloadWiresheet(JS, dOSCWiresheet);
            }

        }
    }
}

using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.UI
{
    public partial class AppProfileItem
    {
        [Inject]
        public NavigationManager? NM { get; set; }
        [Parameter]
        public dOSCWiresheet dOSCWiresheet { get; set; }
        [Parameter]
        public EventCallback<dOSCWiresheet> dOSCWiresheetChanged { get; set; }
        [Parameter]
        public EventCallback<dOSCWiresheet> OnShowSettings { get; set; }
        [Parameter]
        public EventCallback<dOSCWiresheet> OnPlayPause { get; set; }

        private void TogglePlayPause()
        {
            if (dOSCWiresheet != null)
            {
                if (dOSCWiresheet.IsPlaying)
                {
                    dOSCWiresheet.Desconstruct();
                }
                else
                {
                    dOSCWiresheet.Build();
                }

                OnPlayPause.InvokeAsync(dOSCWiresheet);
            }
        }

        private void ShowSettings()
        {
            if(dOSCWiresheet != null)
            {
                OnShowSettings.InvokeAsync(dOSCWiresheet);
            }
        }
        private void EditApp()
        {
            if (NM != null)
            {
                if (dOSCWiresheet != null)
                {
                    NM.NavigateTo($"apps/editor/{dOSCWiresheet.AppGuid}");
                }
            }
        }
    }
}

using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.UI
{
    public partial class AppNavItem
    {

        [Parameter]
        public dOSCWiresheet? Wiresheet { get; set; }

        [Parameter]
        public EventCallback<dOSCWiresheet> SelectedEvent { get; set; }

        [Parameter]
        public dOSCWiresheet? Selected { get; set; } = null;

        private string ActiveCSS => Selected == Wiresheet ? "active" : "";
        private string StatusCSS => GetStatus();

        private string Marquee => Selected == Wiresheet ? "enabled" : "";   


        private async Task OnClick(EventArgs e)
        {
            if (SelectedEvent.HasDelegate)
            {
                await SelectedEvent.InvokeAsync(Wiresheet);
            }
        }

        private string GetStatus()
        {
            if(Wiresheet != null)
            {
                if(Wiresheet.HasError)
                {
                    return "error";
                }
                else
                {
                    if(Wiresheet.IsPlaying)
                    {
                        return "running";
                    }
                    else
                    {
                        return "stopped";
                    }
                }
            }
            return "";
        }

    }

}

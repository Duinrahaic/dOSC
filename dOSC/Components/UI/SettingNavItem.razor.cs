using dOSCEngine.Services;
using dOSCEngine.Services.User;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.UI
{
    public partial class SettingNavItem
    {
        [Parameter]
        public SettingBase? SettingBase { get; set; }
        [Parameter]
        public EventCallback<SettingBase> SelectedEvent { get; set; }
        [Parameter]
        public SettingBase? Selected { get; set; } = null;
        private string ActiveCSS => Selected == SettingBase ? "active" : "";
        private string StatusCSS => GetStatus();
        private string Marquee => Selected == SettingBase ? "enabled" : "";

        private async Task OnClick(EventArgs e)
        {
            if (SelectedEvent.HasDelegate)
            {
                await SelectedEvent.InvokeAsync(SettingBase);
            }
        }

        private string GetStatus()
        {
            if (SettingBase != null)
            {
                if (SettingBase.IsEnabled)
                {
                    return "running";
                }
                else
                {
                    return "stopped";
                }
            }
            return "";
        }
    }
}

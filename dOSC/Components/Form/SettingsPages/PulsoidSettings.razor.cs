using dOSC.Services.ElectronFramework;
using dOSC.Services.User;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Form.SettingsPages
{
    public partial class PulsoidSettings
    {
        [Parameter]
        public PulsoidSetting? Setting { get; set; }
        [Parameter]
        public EventCallback<PulsoidSetting> OnValidSubmit { get; set; }

        private void Submit()
        {
            if (Setting == null)
                return;
            Setting.IsConfigured = true;
            OnValidSubmit.InvokeAsync(Setting);
        }

        private void OpenHelp()
        {
            ElectronFramework.OpenExternal("https://pulsoid.net/ui/keys");
        }
    }
}

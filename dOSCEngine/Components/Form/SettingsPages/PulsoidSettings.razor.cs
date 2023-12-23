using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.User;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Valve.VR;

namespace dOSCEngine.Components.Form.SettingsPages
{
    public partial class PulsoidSettings
    {
        [Parameter]
        public PulsoidSetting? Setting { get; set; }

        [Parameter]
        public EventCallback<PulsoidSetting> OnValidSubmit { get; set; }

        void FormSubmitted(EditContext editContext)
        {
            bool formIsValid = editContext.Validate();
            if (formIsValid)
            {
                OnValidSubmit.InvokeAsync(Setting);
            }
        }
        private void OpenHelp()
        {
            WebUtilities.OpenUrl("https://pulsoid.net/ui/keys");
        }
    }
}

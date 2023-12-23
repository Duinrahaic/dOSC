using dOSCEngine.Services.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace dOSCEngine.Components.Form.SettingsPages
{
    public partial class OSCSettings
    {
        [Parameter]
        public OSCSetting? Setting { get; set; }

        [Parameter]
        public EventCallback<OSCSetting> OnValidSubmit { get; set; }

        void FormSubmitted(EditContext editContext)
        {
            bool formIsValid = editContext.Validate();
            if (formIsValid)
            {
                OnValidSubmit.InvokeAsync(Setting);
            }
        }
    }
}

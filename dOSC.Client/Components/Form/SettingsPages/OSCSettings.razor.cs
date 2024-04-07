using dOSC.Shared.Models.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace dOSC.Client.Components.Form.SettingsPages;

public partial class OSCSettings
{
    [Parameter] public OSCSetting? Setting { get; set; }

    [Parameter] public EventCallback<OSCSetting> OnValidSubmit { get; set; }

    private void FormSubmitted(EditContext editContext)
    {
        var formIsValid = editContext.Validate();
        if (formIsValid) OnValidSubmit.InvokeAsync(Setting);
    }
}
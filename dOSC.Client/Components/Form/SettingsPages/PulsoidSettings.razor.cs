using dOSC.Shared.Models.Settings;
using dOSC.Shared.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace dOSC.Client.Components.Form.SettingsPages;

public partial class PulsoidSettings
{
    [Parameter] public PulsoidSetting? Setting { get; set; }

    [Parameter] public EventCallback<PulsoidSetting> OnValidSubmit { get; set; }

    private void FormSubmitted(EditContext editContext)
    {
        var formIsValid = editContext.Validate();
        if (formIsValid) OnValidSubmit.InvokeAsync(Setting);
    }

    private void OpenHelp()
    {
        WebUtilities.OpenUrl("https://pulsoid.net/ui/keys");
    }
}
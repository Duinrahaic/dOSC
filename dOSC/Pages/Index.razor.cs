using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace dOSC.Pages
{
    public partial class Index
    {
        [Inject]
        private IJSRuntime JS { get; set; }
        private void OpenDiscord() {
            WebUtilities.OpenUrl("https://discord.gg/aZQfy6H9fA");
        }
        private void OpenPatreon() {
            WebUtilities.OpenUrl("https://www.patreon.com/Duinrahaic");
        }
        private void OpenKofi() {
            WebUtilities.OpenUrl("https://ko-fi.com/duinrahaic");
        }
        private void OpenTwitter() {
            WebUtilities.OpenUrl("https://twitter.com/duinrahaic");
        }
        private void OpenGithub() {
            WebUtilities.OpenUrl("https://github.com/Duinrahaic/dOSC");
        }
        private void OpenUpdates() {
            WebUtilities.OpenUrl("https://twitter.com/duinrahaic");
        }
        private void OpenDocumentation() {
            WebUtilities.OpenUrl("https://twitter.com/duinrahaic");
        }

        

    }
}

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
        
        //private void OpenDiscord() => ElectronFramework.OpenExternal("https://discord.gg/UU6brdUdKj");
        //private void OpenPatreon() => ElectronFramework.OpenExternal("https://www.patreon.com/Duinrahaic");
        //private void OpenTwitter() => ElectronFramework.OpenExternal("https://x.com/duinrahaic");
        //private void OpenGithub() => ElectronFramework.OpenExternal("https://github.com/Duinrahaic/dOSC");
        //private void OpenUpdates() => ElectronFramework.OpenExternal("https://github.com/Duinrahaic/dOSC/releases");
        //private void OpenDocumentation() => ElectronFramework.OpenExternal("https://github.com/Duinrahaic/dOSC/wiki");
        private void OpenDiscord() { }
        private void OpenPatreon() {
            OpenUrl("https://www.patreon.com/Duinrahaic");
        }
        private void OpenKofi() {
            OpenUrl("https://ko-fi.com/duinrahaic");
        }
        private async Task OpenTwitter() {
            OpenUrl("https://twitter.com/duinrahaic");
        }
        private void OpenGithub() {
            OpenUrl("https://github.com/Duinrahaic/dOSC");
        }
        private void OpenUpdates() {
            OpenUrl("https://twitter.com/duinrahaic");
        }
        private void OpenDocumentation() {
            OpenUrl("https://twitter.com/duinrahaic");
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

    }
}

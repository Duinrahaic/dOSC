using dOSC.Components;
using dOSC.Services;
using dOSC.Services.ElectronFramework;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Pages
{
    public partial class Index 
    {
        private void OpenDiscord() => ElectronFramework.OpenExternal("https://discord.gg/UU6brdUdKj");
        private void OpenPatreon() => ElectronFramework.OpenExternal("https://www.patreon.com/Duinrahaic");
        private void OpenTwitter() => ElectronFramework.OpenExternal("https://x.com/duinrahaic");
        private void OpenGithub() => ElectronFramework.OpenExternal("https://github.com/Duinrahaic/dOSC");
        private void OpenUpdates() => ElectronFramework.OpenExternal("https://github.com/Duinrahaic/dOSC/releases");
        private void OpenDocumentation() => ElectronFramework.OpenExternal("https://github.com/Duinrahaic/dOSC/wiki");
    }
}

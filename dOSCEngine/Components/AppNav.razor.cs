using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components
{
    public partial class AppNav
    {


        [Inject]
        public dOSCService? Engine { get; set; }
        public List<NavItem> Apps { get; set; } = new List<NavItem>();

        protected override void OnInitialized()
        {
            Apps.Add(new NavItem("Home", "oi oi-home", "/", NavItemType.Home));
            Apps.Add(new NavItem("Apps", "oi oi-code", "/apps", NavItemType.App));
            Apps.Add(new NavItem("Editor", "icon icon-pencil-ruler", "/editor", NavItemType.App));
            Apps.Add(new NavItem("Settings", "oi oi-cog", "/settings", NavItemType.Settings));
        }




    }
}

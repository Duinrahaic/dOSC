using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components
{
	public partial class AppNav
	{


        [Inject]
        public dOSCService? Engine { get; set; }
        public List<NavItem> Apps { get; set; } = new List<NavItem>();

        protected override void OnInitialized()
        {
            Apps.Add(new NavItem("Home", "oi oi-home","/" , NavItemType.Home));
            Apps.Add(new NavItem("Apps", "oi oi-code", "/apps", NavItemType.App));
            Apps.Add(new NavItem("Settings", "oi oi-cog", "/settings", NavItemType.Settings));
        }

     


    }
}

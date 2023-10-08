using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components
{
	public partial class AppNav
	{


        [Inject]
        public dOSCEngine? Engine { get; set; }
        public List<NavItem> Apps { get; set; } = new List<NavItem>();

        protected override void OnInitialized()
        {
            Apps.Add(new NavItem("Home", "mdi mdi-ethernet-cable"));

            var wsm = Engine?.GetWireSheets();
            if (wsm != null)
            {
                foreach (var ws in wsm)
                {
                    Apps.Add(new NavItem(ws.AppGuid.ToString(), "mdi mdi-ethernet-cable", ws));
                }
            }


        }

     


    }
}

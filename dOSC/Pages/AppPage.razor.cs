using dOSC.Components.Modals;
using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Pages
{
    public partial class AppPage
    {

        private dOSCWiresheet? Wiresheet;
        [Inject]
        public dOSCEngine? Engine { get; set; }

        private List<dOSCWiresheet> Wiresheets = new();

        protected override void OnInitialized()
        {
            Wiresheets = Engine?.GetWireSheets() ?? new();
        }

        private void OnSelected(dOSCWiresheet wiresheet)
        {
            Wiresheet = wiresheet;
        }




        // Modals 
        private ModalBase NewAppModal { get; set; }


    }
}

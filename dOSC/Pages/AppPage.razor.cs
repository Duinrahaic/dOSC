using dOSC.Components.Modals;
using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Pages
{
    public partial class AppPage
    {

        [Inject]
        public dOSCEngine? Engine { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }

        [Parameter]
        public Guid? AppId { get; set; }

        private List<dOSCWiresheet> Wiresheets = new();
        private dOSCWiresheet? Wiresheet;

        protected override void OnInitialized()
        {
            Wiresheets = Engine?.GetWireSheets() ?? new();
        }

        protected override void OnParametersSet()
        {
            if (Engine == null) return;
            if (AppId.HasValue)
            {
                Wiresheet = Engine.GetWiresheet(AppId.Value);
            }
        }

        private void OnSelected(dOSCWiresheet wiresheet)
        {
            Wiresheet = wiresheet;
        }

        private void NewApp()
        {
            NewAppModal.Close();
            if(NM != null)
            {
                NM.NavigateTo($"apps/editor/");
            }
            
        }


        // Modals 
        private ModalBase NewAppModal { get; set; }


    }
}

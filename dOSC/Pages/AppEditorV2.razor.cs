using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Pages
{
    public partial class AppEditorV2
    {
        [Inject]
        public dOSCService? Engine { get; set; }
        [Inject]
        public IJSRuntime? JS { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }

        [Parameter]
        public Guid? AppId { get; set; }

        private List<dOSCWiresheet> Wiresheets = new();
        private dOSCWiresheet? Wiresheet;

        protected override void OnInitialized()
        {
            Wiresheets = Engine?.GetWireSheets() ?? new();
            Wiresheet = Wiresheets.FirstOrDefault();
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
    }
}

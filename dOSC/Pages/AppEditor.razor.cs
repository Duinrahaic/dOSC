using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Pages
{
    public partial class AppEditor
    {
        [Parameter]
        public Guid? AppId { get; set; }
        [Parameter]
        public dOSCWiresheet? Wiresheet { get; set; }
        [Inject]
        public dOSCEngine? Engine { get; set; }

        protected override void OnParametersSet()
        {
            if (Engine == null) return;
            if (AppId.HasValue)
            {
                Wiresheet = Engine.GetWiresheet(AppId.Value);
                this.StateHasChanged();
            }
            else
            {
                Wiresheet = Engine.GetWireSheets().First();
            }
        }
    }
}

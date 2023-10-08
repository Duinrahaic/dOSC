using Blazor.Diagrams;
using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet
{
    public partial class DiagramRenderer
    {
        [Parameter]
        public BlazorDiagram? Diagram
        {
            get
            {

                return _Diagram;
            }

            set
            {
                _Diagram = value;
                StateHasChanged();
            }
        }
        private BlazorDiagram? _Diagram = new(dOSCWiresheetConfiguration.Options);
    }
}

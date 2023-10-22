using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Wiresheet.Blocks.Math
{
    public partial class NegativeBlock
    {
        [Parameter] public NegativeNode Node { get; set; } = null;
        public string LinkValue(PortModel? port)
        {
            if (port.Links.Count > 0)
            {
                var l = port.Links[0];
                var v = Node.InputValue(port, l);
                return v.ToString();
            }
            return "0";
        }
        public void ShowLinksLabel()
        {
            Node?.ShowLinksLabel();
            StateHasChanged();
        }
    }
}

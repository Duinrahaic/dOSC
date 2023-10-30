using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes.Math;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Math
{
    public partial class MaxBlock
    {
        [Parameter] public MaxNode Node { get; set; } = null;
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
    }
}

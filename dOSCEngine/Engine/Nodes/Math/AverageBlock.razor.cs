using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Nodes.Math
{
    public partial class AverageBlock
    {
        [Parameter] public AverageNode Node { get; set; } = null;
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
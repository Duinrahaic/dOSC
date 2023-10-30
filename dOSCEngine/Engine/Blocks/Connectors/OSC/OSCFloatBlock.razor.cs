using dOSCEngine.Engine.Nodes.Connector.OSC;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Engine.Blocks.Connectors.OSC
{
    public partial class OSCFloatBlock
    {
        [Parameter]
        public OSCFloatNode Node { get; set; } = null;

        private void OnValueChanged(string value)
        {
            Node.SelectedOption = value;
        }
    }
}

using Microsoft.AspNetCore.Components;

namespace dOSC.Components.UI.Table
{
    public partial class ConnectorTable
    {
        [Parameter]
        public EventCallback<(string,ServiceCommand)> OnConnectorClick { get; set; }
    }
}

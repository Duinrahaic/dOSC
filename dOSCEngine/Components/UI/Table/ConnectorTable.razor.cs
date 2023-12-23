using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components.UI.Table
{
    public partial class ConnectorTable
    {
        [Parameter]
        public EventCallback<(string, ServiceCommand)> OnConnectorClick { get; set; }
    }
}

using dOSC.Shared.Models.Websocket;
using dOSCEngine.Components;
using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components
{
    public partial class AppNav: IDisposable
    {
        
        
        
        [Inject] public WebsocketClient Client { get; set; } = default!;
        public List<NavItem> Apps { get; set; } = new List<NavItem>();
        public ConnectionState ConnectionState { get; private set; } = ConnectionState.Unknown;
        
        public string ConnectionClass => ConnectionState switch
        {
            ConnectionState.Closed => "--error",
            ConnectionState.Reconnecting => "--warning",
            ConnectionState.Open => "--success",
            _ => "--offline"
        };
        
        
        protected override void OnInitialized()
        {
            Client.OnStateChanged += UpdateWebsocketState;
            ConnectionState = Client.State;
            Apps.Add(new NavItem("Home", "oi oi-home", "/", NavItemType.Home));
            Apps.Add(new NavItem("Apps", "oi oi-code", "/apps", NavItemType.App));
            Apps.Add(new NavItem("Editor", "icon icon-pencil-ruler", "/editor", NavItemType.App));
            Apps.Add(new NavItem("Settings", "oi oi-cog", "/settings", NavItemType.Settings));
        }

        private void UpdateWebsocketState( ConnectionState state)
        {
            ConnectionState = state;
            InvokeAsync(StateHasChanged);
        }
        
        private async Task ResetConnection()
        {
            await Client.SendAsync("disconnect");
        }
        
        public void Dispose()
        {
            Client.OnStateChanged -= UpdateWebsocketState;
        }
    }
}

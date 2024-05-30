using dOSC.Shared.Models.Websocket;
using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.App;

public partial class AppNav : IDisposable
{
    //[Inject] public WebsocketClient Client { get; set; } = default!;
    public List<NavItem> Apps { get; set; } = new();
    public ConnectionState ConnectionState { get; private set; } = ConnectionState.Unknown;

    public string ConnectionClass => ConnectionState switch
    {
        ConnectionState.Closed => "--error",
        ConnectionState.Reconnecting => "--warning",
        ConnectionState.Open => "--success",
        _ => "--offline"
    };

    public void Dispose()
    {
        //Client.OnStateChanged -= UpdateWebsocketState;
    }


    protected override void OnInitialized()
    {
        //Client.OnStateChanged += UpdateWebsocketState;
        //ConnectionState = Client.State;
        Apps.Add(new NavItem("Home", "oi oi-home", "/", NavItemType.Home));
        Apps.Add(new NavItem("Apps", "oi oi-code", "/apps", NavItemType.App));
        Apps.Add(new NavItem("Editor", "icon icon-pencil-ruler", "/editor", NavItemType.App));
        Apps.Add(new NavItem("Settings", "oi oi-cog", "/settings", NavItemType.Settings));
    }

    private void UpdateWebsocketState(ConnectionState state)
    {
        ConnectionState = state;
        InvokeAsync(StateHasChanged);
    }

    private async Task ResetConnection()
    {
        /*try
        {
            if (Client.State == ConnectionState.Open)
            {
                await Client.SendAsync("disconnect");
            }
            else
            {
                await Client.ConnectAsync();
            }

        }
        catch
        {
            // ignored
        }*/
    }
}
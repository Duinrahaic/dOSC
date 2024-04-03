using System.Net.WebSockets;

namespace dOSCEngine.Websocket;

public class WebsocketClient
{
    private ClientWebSocket _client = new ClientWebSocket();
    public async Task Connect(string url)
    {
        _client = new ClientWebSocket();
        await _client.ConnectAsync(new Uri(url), CancellationToken.None);
        await Task.CompletedTask;
    }
    
    
}
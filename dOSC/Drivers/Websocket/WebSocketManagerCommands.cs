using System.Net.WebSockets;
using dOSC.Client.Models.Websocket;
using dOSC.Client.Models.Commands;

namespace dOSC.Drivers.Websocket;

public partial class WebSocketManager
{
    public static async Task Broadcast(Command apiDataObject)
    {
        foreach (var socket in _sessions.Values.ToList())
            if (socket.State == WebSocketState.Open)
                await socket.SendAsync(apiDataObject.WritePacket(), WebSocketMessageType.Text, true,
                    CancellationToken.None);
    }
    
    public static async Task SendAsync(WebSocket socket, Command apiDataObject)
    {
        if (socket.State == WebSocketState.Open)
            await socket.SendAsync(apiDataObject.WritePacket(), WebSocketMessageType.Text, true,
                CancellationToken.None);
    }
}
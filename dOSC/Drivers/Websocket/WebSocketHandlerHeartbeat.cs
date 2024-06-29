using System.Net.WebSockets;
using System.Text;
using dOSC.Client.Models.Commands;

namespace dOSC.Drivers.Websocket;

public partial class WebSocketHandler
{
    private async Task SendHeartbeatAsync(WebSocket socket, Guid socketId)
    {
        var heartbeatInterval = TimeSpan.FromSeconds(5);
        var buffer = Encoding.UTF8.GetBytes("ping");

        string origin = "Websocket";
        string target = "All Clients";
        string message = "Websocket Heartbeat at " + DateTime.Now.ToString("HH:mm:ss");
        var data = new Log(DateTime.Now.ToString(), origin, DoscLogLevel.Info, message); 
        Command heartbeat = new(origin, target, "Heartbeat", "Log", data:data);
        
        while (socket.State == WebSocketState.Open)
        {
            await Task.Delay(heartbeatInterval);

            if (socket.State == WebSocketState.Open)
            {
                await Broadcast(heartbeat);
            }
        }

        _sessions.TryRemove(socketId, out _);
    }
}
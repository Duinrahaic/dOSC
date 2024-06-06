using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using dOSC.Middlewear;
using dOSC.Shared.Models.Commands;

namespace dOSC.Drivers.Websocket;

public partial class WebSocketManager
{
    public delegate void DataReceiveEventHandler(Command e);

    private static DataReceiveEventHandler? _dataReceivedHandler;

    private static readonly ConcurrentDictionary<Guid, WebSocket> _sessions = new();
    public async Task AcceptWebSocketAsync(WebSocket socket)
    {
        var socketId = Guid.NewGuid();
        _sessions[socketId] = socket;
        _ = Task.Run(() => SendHeartbeatAsync(socket, socketId));  // Start heartbeat task
        await Receive(socket, socketId);
    }

    private async Task Receive(WebSocket socket, Guid socketId)
    {
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = buffer.ReadPacket<string>() ?? string.Empty;
                if (message.Equals("disconnect", StringComparison.OrdinalIgnoreCase))
                {

                    _sessions.TryRemove(socketId, out _);

                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect requested",
                        CancellationToken.None);
                }
                else
                {
                    try
                    {
                        // Deserialize the JSON message
                        var command = buffer.ReadPacket<Command>();
                        if (command != null) _dataReceivedHandler?.Invoke(command);
                    }
                    catch
                    {
                        // ignored
                    }
                    
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                _sessions.TryRemove(socketId, out _);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager", CancellationToken.None);
            }
        }
    }

    private IEnumerable<WebSocket> GetAllConnections()
    {
        return _sessions.Values;
    }

    private async Task CloseAllConnectionsAsync()
    {
        foreach (var socket in _sessions.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager", CancellationToken.None);
            }
        }

        _sessions.Clear();
        
    }
    
    
    private async Task SendMessageToAllAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sessions.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
    

    
}
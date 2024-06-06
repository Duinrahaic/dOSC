using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Shared.Models.Commands;
using Microsoft.AspNetCore.Http;

namespace dOSC.Middlewear;

public class WebSocketServer
{
    public delegate void DataReceiveEventHandler(Command e);

    private static DataReceiveEventHandler? _dataReceivedHandler;

    private static readonly ConcurrentDictionary<WebSocket, Guid> _sessions = new();


    public static async Task HandleWebSocket(HttpContext context)
    {
        using var socket = await context.WebSockets.AcceptWebSocketAsync();

        // Generate a unique session ID
        var sessionId = Guid.NewGuid();

        // Add the new session to the dictionary
        _sessions.TryAdd(socket, sessionId);


        var connectionAlive = true;
        var webSocketPayload = new List<byte>(1024 * 4);
        var tempMessage = new byte[1024 * 4];

        while (connectionAlive)
            try
            {
                // Empty the container
                webSocketPayload.Clear();

                // Message handler
                WebSocketReceiveResult? webSocketResponse;

                // Read message in a loop until fully read
                do
                {
                    // Wait until client sends message
                    webSocketResponse = await socket.ReceiveAsync(tempMessage, CancellationToken.None);

                    // Save bytes
                    webSocketPayload.AddRange(new ArraySegment<byte>(tempMessage, 0, webSocketResponse.Count));
                } while (webSocketResponse.EndOfMessage == false);

                // Process the message
                if (webSocketResponse.MessageType == WebSocketMessageType.Text)
                {
                    var currentSessionId = _sessions[socket];

                    // 3. Convert textual message from bytes to string
                    var message = webSocketPayload.ReadPacket<string>() ?? string.Empty;

                    if (message.Equals("disconnect", StringComparison.OrdinalIgnoreCase))
                    {
                        connectionAlive = false;

                        _sessions.TryRemove(socket, out _);

                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect requested",
                            CancellationToken.None);
                    }
                    else
                    {
                        // Deserialize the JSON message
                        var command = webSocketPayload.ReadPacket<Command>();
                        if (command != null) _dataReceivedHandler?.Invoke(command);
                    }
                }
                else if (webSocketResponse.MessageType == WebSocketMessageType.Close)
                {
                    // 4. Close the connection
                    connectionAlive = false;

                    // Remove the session from the dictionary
                    _sessions.TryRemove(socket, out _);
                }
            }
            catch (WebSocketException)
            {
                // The client has disconnected unexpectedly
                // Close the connection
                connectionAlive = false;

                // Remove the session from the dictionary
                _sessions.TryRemove(socket, out _);
                // Perform any necessary cleanup
            }
    }

    public static async Task Broadcast(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sessions.Keys)
            if (socket.State == WebSocketState.Open)
            {
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
    }

    public static async Task Broadcast(Command apiDataObject)
    {
        foreach (var socket in _sessions.Keys)
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
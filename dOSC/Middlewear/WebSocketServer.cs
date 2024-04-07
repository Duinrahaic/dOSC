using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using dOSC.Shared.Models.Commands;
using dOSCEngine.Websocket.Commands;
using Newtonsoft.Json;

namespace dOSC.Hub.Middlewear;

public class WebSocketServer
{
    public delegate void DataReceiveEventHandler(dOSCCommandDTO e);
    private static DataReceiveEventHandler? _dataReceivedHandler;
    
    private static ConcurrentDictionary<WebSocket, Guid> _sessions = new ConcurrentDictionary<WebSocket, Guid>();
    
    
    
 
    
    public static async Task HandleWebSocket(HttpContext context)
    {
        using var socket = await context.WebSockets.AcceptWebSocketAsync();

        // Generate a unique session ID
        Guid sessionId = Guid.NewGuid();

        // Add the new session to the dictionary
        _sessions.TryAdd(socket, sessionId);


        bool connectionAlive = true;
        List<byte> webSocketPayload = new List<byte>(1024 * 4);
        byte[] tempMessage = new byte[1024 * 4];

        while (connectionAlive)
        {
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
                    Guid currentSessionId = _sessions[socket];

                    // 3. Convert textual message from bytes to string
                    string message = webSocketPayload.ReadPacket<string>() ?? string.Empty;

                    if(message.Equals("disconnect", StringComparison.OrdinalIgnoreCase))
                    {
                        connectionAlive = false;
                        
                        _sessions.TryRemove(socket, out _);

                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect requested", CancellationToken.None);
                    }
                    else
                    {
                        // Deserialize the JSON message
                        dOSCCommandDTO? command = webSocketPayload.ReadPacket<dOSCCommandDTO>();
                        if (command != null)
                        {
                            _dataReceivedHandler?.Invoke(command);
                        }
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
    }

    public static async Task Broadcast(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sessions.Keys)
        {
            if (socket.State == WebSocketState.Open)
            {
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
    
    public static async Task Broadcast(dOSCCommandDTO apiDataObject)
    {
        foreach (var socket in _sessions.Keys)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(apiDataObject.WritePacket(), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
 
    
    
    
    
    
    public static async Task SendAsync(WebSocket socket, dOSCCommandDTO apiDataObject)
    {
        if (socket.State == WebSocketState.Open)
        {
            await socket.SendAsync(apiDataObject.WritePacket(), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    
    
} 

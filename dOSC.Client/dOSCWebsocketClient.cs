using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Client.Models.Commands;
using dOSC.Client.Models.Websocket;
using dOSC.Shared.Models.Websocket;

namespace dOSC.Client;

public class dOSCWebsocketClient
{
    public delegate void DataReceiveEventHandler(Command e);
    public event DataReceiveEventHandler? OnDataReceived;

    public delegate void StateChangeEventHandler(ConnectionState state);
    public event StateChangeEventHandler? OnStateChanged;
    private readonly CancellationTokenSource _CTS = new();

    private readonly string _identity = string.Empty;
    private readonly string _key;
    private readonly int _port = 5880;

    private ClientWebSocket? _socket;


    private ConnectionState _state = ConnectionState.Unknown;

    public dOSCWebsocketClient(string key, string identity, int port = 5880)
    {
        _key = key;
        _port = port;
        _identity = identity;
    }

    public ConnectionState State
    {
        get => _state;
        private set
        {
            if (_state != value)
            {
                _state = value;
                OnStateChanged?.Invoke(_state);
            }
        }
    }

    private string GetConnectionString()
    {
        return $"ws://localhost:{_port} /ws?apiKey={_key}&?identity={_identity}";
    }



    public async Task ConnectAsync()
    {
        _socket = new ClientWebSocket();
        await _socket.ConnectAsync(new Uri(GetConnectionString()), CancellationToken.None);
        await ReceiveMessagesAsync();
    }

    private async Task ReceiveMessagesAsync()
    {
        var webSocketPayload = new List<byte>(1024 * 4);
        var tempMessage = new byte[1024 * 4];
        while (_socket?.State == WebSocketState.Open)
        {
            State = ConnectionState.Open;

            try
            {
                webSocketPayload.Clear();
                WebSocketReceiveResult? webSocketResponse;

                do
                {
                    webSocketResponse = await _socket.ReceiveAsync(tempMessage, CancellationToken.None);
                    // Save bytes
                    webSocketPayload.AddRange(new ArraySegment<byte>(tempMessage, 0, webSocketResponse.Count));
                } while (webSocketResponse.EndOfMessage == false);

                // Process the message
                if (webSocketResponse.MessageType == WebSocketMessageType.Text)
                {
                    // 3. Convert textual message from bytes to string
                    var message = Encoding.UTF8.GetString(webSocketPayload.ToArray());

                    if (message.Equals("disconnect", StringComparison.OrdinalIgnoreCase))
                        await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect requested",
                            CancellationToken.None);
                    else
                        try
                        {
                            // Deserialize the JSON message
                            var command = webSocketPayload.ReadPacket<Command>();
                            if (command != null) OnDataReceived?.Invoke(command);
                        }
                        catch (Exception e)
                        {
                        }
                }
                else if (webSocketResponse.MessageType == WebSocketMessageType.Close)
                {
                    State = ConnectionState.Closed;
                    break;
                }
            }
            catch (WebSocketException)
            {
                State = ConnectionState.Closed;
                break;
            }
        }


        // Attempt to reconnect after a delay
        await Task.Delay(TimeSpan.FromSeconds(1));
        State = ConnectionState.Reconnecting;
        await ConnectAsync();
    }


    public async Task SendAsync(string data)
    {
        var buffer = Encoding.UTF8.GetBytes(data);
        await _socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true,
            CancellationToken.None);
    }

    public async Task SendAsync(Command command)
    {
        await _socket.SendAsync(command.WritePacket(), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private void Disconnect()
    {
        if (_socket != null)
        {
            _socket.Abort();
            _CTS.Cancel();
            _socket = null;
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}
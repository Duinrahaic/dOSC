using System.Net.WebSockets;
using System.Text;
using dOSC.Middlewear;
using dOSC.Shared.Models.Commands;
using dOSC.Shared.Models.Settings;
using dOSC.Shared.Models.Websocket;
using dOSC.Shared.Utilities;
using Microsoft.Extensions.Hosting;

namespace dOSC.Client;

public class WebsocketClient : IHostedService
{
    public delegate void DataReceiveEventHandler(dOSCCommandDTO e);

    public delegate void StateChangeEventHandler(ConnectionState state);

    private readonly CancellationTokenSource _CTS = new();
    private readonly string _key;

    private readonly ILogger<WebsocketClient> _logger;

    private ConnectionState _state = ConnectionState.Unknown;
    private dOSCSetting? Setting;

    private ClientWebSocket? socket;

    public WebsocketClient(IServiceProvider services)
    {
        _key = EncryptionHelper.Key;
        _logger = services.GetService<ILogger<WebsocketClient>>()!;
        _logger.LogInformation("Starting dOSC Websocket Service");
        //LoadSetting();
        Task.Run(async () => await ConnectAsync());
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

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public event DataReceiveEventHandler? OnDataReceived;
    public event StateChangeEventHandler? OnStateChanged;

    public async Task ConnectAsync()
    {
        if (Setting == null)
            return;
        socket = new ClientWebSocket();
        await socket.ConnectAsync(new Uri(Setting.GetHubServerUri(_key)), CancellationToken.None);
        await ReceiveMessagesAsync();
    }

    private async Task ReceiveMessagesAsync()
    {
        var webSocketPayload = new List<byte>(1024 * 4);
        var tempMessage = new byte[1024 * 4];
        while (socket?.State == WebSocketState.Open)
        {
            State = ConnectionState.Open;

            try
            {
                webSocketPayload.Clear();
                WebSocketReceiveResult? webSocketResponse;

                do
                {
                    webSocketResponse = await socket.ReceiveAsync(tempMessage, CancellationToken.None);
                    // Save bytes
                    webSocketPayload.AddRange(new ArraySegment<byte>(tempMessage, 0, webSocketResponse.Count));
                } while (webSocketResponse.EndOfMessage == false);

                // Process the message
                if (webSocketResponse.MessageType == WebSocketMessageType.Text)
                {
                    // 3. Convert textual message from bytes to string
                    var message = Encoding.UTF8.GetString(webSocketPayload.ToArray());

                    if (message.Equals("disconnect", StringComparison.OrdinalIgnoreCase))
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect requested",
                            CancellationToken.None);
                    }
                    else
                    {
                        // Deserialize the JSON message
                        var command = webSocketPayload.ReadPacket<dOSCCommandDTO>();
                        if (command != null) OnDataReceived?.Invoke(command);
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
        await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task SendAsync(dOSCCommandDTO command)
    {
        await socket.SendAsync(command.WritePacket(), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public void LoadSetting()
    {
        Setting = (dOSCFileSystem.LoadSettings() ?? new UserSettings()).dOSC;
    }

    public dOSCSetting GetSetting()
    {
        return Setting ?? new dOSCSetting();
    }

    private void Disconnect()
    {
        if (socket != null)
        {
            socket.Abort();
            _CTS.Cancel();
            socket = null;
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}
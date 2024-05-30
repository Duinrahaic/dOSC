using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Shared.Models.Settings;
using dOSC.Shared.Utilities;
using dOSC.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dOSC.Drivers.Pulsoid;

public class PulsoidService : ConnectorBase, IDisposable, IHostedService
{
    public delegate void PulsoidSubscriptionEventHandler(PulsoidReading e);

    private const int ReceiveBufferSize = 256;
    private const string _url = @"wss://dev.pulsoid.net/api/v1/data/real_time";
    private const string _scope = "data:heart_rate:read";

    private ClientWebSocket? _client;
    private readonly CancellationTokenSource _CTS = new();
    private readonly ILogger<PulsoidService> _logger;
    private PulsoidSetting? Setting;


    public PulsoidService(IServiceProvider services)
    {
        _logger = services.GetService<ILogger<PulsoidService>>()!;
        _logger.LogInformation("Initialized Pulsoid Service");
        LoadSetting();
        if (Setting != null)
            if (Setting.IsEnabled)
                StartService();
    }

    public override string ServiceName => "Pulsoid";
    public override string IconRef => @"_content/dOSCEngine/images/Pulsoid-Logo-500x281.png";
    public override string Description => "A real time heart rate for streaming";
    private Uri _URI => new($"{_url}?access_token={Setting.AccessToken}");

    public void Dispose()
    {
        Disconnect();
    }

    public event PulsoidSubscriptionEventHandler? OnPulsoidMessageReceived;

    public override void LoadSetting()
    {
        Setting = (AppFileSystem.LoadSettings() ?? new UserSettings()).Pulsoid;
    }

    public override SettingBase GetSetting()
    {
        return Setting ?? new PulsoidSetting();
    }


    public override void StartService()
    {
        if (Setting != null)
            if (Setting.IsConfigured)
            {
                Task.Run(() => Connect());
                Setting.IsEnabled = true;
                Running = true;
                UpdateSetting(Setting);
            }
    }

    public override void StopService()
    {
        if (Setting != null)
        {
            Disconnect();
            Setting.IsEnabled = false;
            Running = false;
            UpdateSetting(Setting);
        }
    }

    private async Task Connect()
    {
        if (_client != null)
            _client.Dispose();
        _client = new ClientWebSocket();

        await _client.ConnectAsync(_URI, _CTS.Token);

        var buffer = new byte[ReceiveBufferSize];

        if (_client.State == WebSocketState.Open) await SendMessage();
        while (_client.State == WebSocketState.Open)
            try
            {
                var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _CTS.Token);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogDebug("Pulsoid closed websocket ... disconnecting");
                    await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _CTS.Token);
                    Disconnect();
                }
                else
                {
                    HandleMessage(buffer, result.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Pulsoid encountered an error while listening for data: {ex}");
            }
    }

    private async Task SendMessage()
    {
        if (_client == null) return;
        var bytes = new byte[_scope.Length * sizeof(char)];
        Buffer.BlockCopy(_scope.ToCharArray(), 0, bytes, 0, bytes.Length);
        await _client.SendAsync(bytes, WebSocketMessageType.Text, true, _CTS.Token);
    }

    private void HandleMessage(byte[] buffer, int count)
    {
        var json = Encoding.Default.GetString(buffer, 0, count);
        var jobject = JObject.Parse(json);
        PulsoidReading? result = null;
        try
        {
            result = JsonConvert.DeserializeObject<PulsoidReading>(jobject.ToString());
            if (result != null)
            {
                OnPulsoidMessageReceived?.Invoke(result);
                _logger.LogDebug($"Pulsoid Sent: {result.Data.HeartRate} bpm");
            }
        }
        catch
        {
        }
    }

    private void Disconnect()
    {
        if (_client != null)
        {
            _client.Abort();
            _CTS.Cancel();
            _client = null;
            Setting.IsEnabled = false;
            UpdateSetting(Setting);
        }
    }
}
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Attributes;
using dOSC.Client.Models.Commands;
using dOSC.Drivers.Settings;
using dOSC.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dOSC.Drivers.Pulsoid;

public class PulsoidService : ConnectorBase, IDisposable, IHostedService
{
   
    private const string Url = @"wss://dev.pulsoid.net/api/v1/data/real_time";
    private const string Scope = "data:heart_rate:read";

    public override string ServiceName => "Pulsoid";
    public override string IconRef => @"_content/dOSCEngine/images/Pulsoid-Logo-500x281.png";
    public override string Description => "A real time heart rate for streaming";
    private Uri Uri => new($"{Url}?access_token={_setting.AccessToken}");

    
    private ClientWebSocket? _client;
    private readonly CancellationTokenSource _cts = new();
    private readonly ILogger<PulsoidService> _logger;
    private PulsoidSetting? _setting ;

    
    
    private void UpdateSetting(PulsoidSetting setting)
    {
        var settings = AppFileSystem.LoadSettings() ?? new UserSettings();
        settings.Pulsoid = setting;
        AppFileSystem.SaveSettings(settings);
    }

    public PulsoidService(IServiceProvider services):base(services)
    {
        _logger = services.GetService<ILogger<PulsoidService>>()!;
        _logger.LogInformation("Initialized Pulsoid Service");
        LoadSetting();
        

        
        
        if (_setting != null)
            StartService();
        
    }

  
    public void Dispose()
    {
        Disconnect();
    }


    public override void LoadSetting()
    {
        _setting = (AppFileSystem.LoadSettings() ?? new UserSettings()).Pulsoid;
    }

    public override SettingBase GetSetting()
    {
        return _setting ?? new PulsoidSetting();
    }


    public override void StartService()
    {
        if (_setting != null)
        {
            Task.Run(() => Connect());
            _setting.Enabled = true;
            Running = true;
            UpdateSetting(_setting);
        }
    }

    public override void StopService()
    {
        if (_setting != null)
        {
            Disconnect();
            _setting.Enabled = false;
            Running = false;
            UpdateSetting(_setting);
        }
    }


    private bool _status = false;
    [ConfigLogicEndpoint(Owner = "Pulsoid", Name = "Status", Description = "Pulsoid Connection Status", Permissions = Permissions.ReadOnly, 
        DefaultValue = false, TrueLabel = "Connected", FalseLabel = "Disconnected")] 
    public bool Status {
        get => _status;
        set
        {
            if (!_status.Equals(value))
            {
                _status = value;
                UpdateEndpointValue("Status", _status);
            }
        }
    } 


    private decimal _heartRate = 0;
    [ConfigNumericEndpoint(Owner = "Pulsoid", Name = "Heart Rate", Description = "Real time heart rate data", Unit = "bpm", Permissions = Permissions.ReadOnly)]
    public decimal HeartRate
    {
        get => _heartRate;
        set
        {
            if (!_heartRate.Equals(value))
            {
                _heartRate = value;
                UpdateEndpointValue("Heart Rate", _heartRate);
            }
        }
    } 
    
    
    
    
    
    
    
    private async Task Connect()
    {
        if (_client != null)
            _client.Dispose();
        _client = new ClientWebSocket();

        await _client.ConnectAsync(Uri, _cts.Token);

        var buffer = new byte[256];
        Status = true;

        if (_client.State == WebSocketState.Open) await SendMessage();
        while (_client.State == WebSocketState.Open)
        {

            try
            {
                var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogDebug("Pulsoid closed websocket ... disconnecting");
                    await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _cts.Token);
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

        Status = false;
    }

    private async Task SendMessage()
    {
        if (_client == null) return;
        var bytes = new byte[Scope.Length * sizeof(char)];
        Buffer.BlockCopy(Scope.ToCharArray(), 0, bytes, 0, bytes.Length);
        await _client.SendAsync(bytes, WebSocketMessageType.Text, true, _cts.Token);
    }

    private object _lock = new();
    private void HandleMessage(byte[] buffer, int count)
    {
        lock (_lock)
        {
            var json = Encoding.Default.GetString(buffer, 0, count);
            var jobject = JObject.Parse(json);
            PulsoidReading? result = null;
            try
            {
                result = JsonConvert.DeserializeObject<PulsoidReading>(jobject.ToString());
                if (result != null)
                {
                    HeartRate = result.Data.HeartRate;
                    _logger.LogDebug($"Pulsoid Sent: {result.Data.HeartRate} bpm");
                }
            }
            catch { }
        }
        
    }

    private void Disconnect()
    {
        if (_client != null)
        {
            _client.Abort();
            _cts.Cancel();
            _client = null;
            _setting.Enabled = false;
            UpdateSetting(_setting);
        }
    }
}
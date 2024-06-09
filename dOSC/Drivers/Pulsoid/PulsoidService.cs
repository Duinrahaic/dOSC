using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Attributes;
using dOSC.Client.Models.Commands;
using dOSC.Shared.Models.Settings;
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
    private Uri Uri => new($"{Url}?access_token={Setting.AccessToken}");

    
    private ClientWebSocket? _client;
    private readonly CancellationTokenSource _cts = new();
    private readonly ILogger<PulsoidService> _logger;
    private PulsoidSetting? Setting ;

    
    
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
        

        
        
        if (Setting != null)
            StartService();
        
    }

  
    public void Dispose()
    {
        Disconnect();
    }


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

    private static DataEndpoint GetHeartRateEndpoint() => new DataEndpoint
    {
        Owner = "Pulsoid",
        Name = "Heart Rate",
        Description = "Real time heart rate data",
        Type = DataType.Numeric,
        Permissions = Permissions.ReadOnly,
        Labels = new NumericDataLabels
        {
            Unit = "bpm",
        },
        DefaultValue = "0"
    };
    public void UpdateHeartRateEndpoint(decimal value)
    {
        var ev = GetHeartRateEndpoint().ToDataEndpointValue();
        ev.UpdateValue(value);
        HubService.UpdateEndpointValue(ev);
    }

    [ConfigLogicEndpoint(Owner = "Pulsoid", Name = "Status", Description = "Pulsoid Connection Status", Permissions = Permissions.ReadOnly, 
        DefaultValue = false, TrueLabel = "Connected", FalseLabel = "Disconnected")] 
    public bool Status { get; set; } = false;
    
    [ConfigNumericEndpoint(Owner = "Pulsoid", Name = "Heart Rate", Description = "Real time heart rate data", Unit = "bpm", Permissions = Permissions.ReadOnly)]
    public decimal HeartRate { get; set; } = 0;
    
    
    
    
    private static DataEndpoint GetStatusEndpoint() => new DataEndpoint
    {
        Owner = "Pulsoid",
        Name = "Status",
        Description = "Pulsoid connection status",
        Type = DataType.Logic,
        Permissions = Permissions.ReadOnly,
        Labels = new LogicDataLabels
        {
            TrueLabel = "Connected",
            FalseLabel = "Disconnected"
        },
        DefaultValue = "False"
    };

    public void UpdateStatusEndpoint(bool state)
    {
        var ev = GetStatusEndpoint().ToDataEndpointValue();
        ev.UpdateValue(state);
        HubService.UpdateEndpointValue(ev);
    }
    
    
    private async Task Connect()
    {
        if (_client != null)
            _client.Dispose();
        _client = new ClientWebSocket();

        await _client.ConnectAsync(Uri, _cts.Token);

        var buffer = new byte[256];
        UpdateStatusEndpoint(true);

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
        UpdateStatusEndpoint(false);
    }

    private async Task SendMessage()
    {
        if (_client == null) return;
        var bytes = new byte[Scope.Length * sizeof(char)];
        Buffer.BlockCopy(Scope.ToCharArray(), 0, bytes, 0, bytes.Length);
        await _client.SendAsync(bytes, WebSocketMessageType.Text, true, _cts.Token);
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
                UpdateHeartRateEndpoint(result.Data.HeartRate);
                _logger.LogDebug($"Pulsoid Sent: {result.Data.HeartRate} bpm");
            }
        }
        catch { }
    }

    private void Disconnect()
    {
        if (_client != null)
        {
            _client.Abort();
            _cts.Cancel();
            _client = null;
            Setting.IsEnabled = false;
            UpdateSetting(Setting);
        }
    }
}
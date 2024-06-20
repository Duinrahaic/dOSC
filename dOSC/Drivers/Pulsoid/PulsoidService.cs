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

public class PulsoidService : ConnectorBase, IDisposable
{
   
    private const string Url = @"wss://dev.pulsoid.net/api/v1/data/real_time";
    private const string Scope = "data:heart_rate:read";

    public override string Name => "Pulsoid";
    public override string IconRef => @"/images/Pulsoid-Logo-500x281.png";
    public override string Description => "A real-time heart rate service for streaming";
    private Uri Uri => new($"{Url}?access_token={GetConfiguration().Key}");

    
    private ClientWebSocket? _client;
    private CancellationTokenSource _cts = new();
    private readonly ILogger<PulsoidService> _logger;
    private PulsoidSetting GetConfiguration() => (PulsoidSetting) Configuration;

    private string _key = string.Empty;
    public string Key
    {
        get => _key;
        set
        {
            if (!_key.Equals(value))
            {
                _key = value;
                var configuration = GetConfiguration();
                configuration.Key = value;
                SaveConfiguration(Configuration);
            }
        }
    }
    
    public override bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                var config = GetConfiguration();
                config.Enabled = value;
                SaveConfiguration(config);
                if (_enabled)
                {
                    StartService();
                }
                else
                {
                    StopService();
                }
            }
        }
    }
    
    
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
        Configuration = AppFileSystem.LoadSettings().Pulsoid;
        var configuration = GetConfiguration();
        _key = configuration.Key;
        
    }
    
    public void Dispose()
    {
        StopService();
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
    [ConfigNumericEndpoint(Owner = "Pulsoid", Name = "Heart Rate", Description = "Real time heart rate data", Unit = "bpm", Permissions = Permissions.ReadOnly, Precision = 0)]
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

    
    public override void StartService()
    {
        if (!Running)
        {
            StartAsync(CancellationToken.None);
        }
    }

    public override void StopService()
    {
        if (Running)
        {
            _client?.Abort();
            _cts.Cancel();
        }
    }


    private async Task Run(CancellationToken stoppingToken)
    {
        if (_client != null)
            _client.Dispose();
        _client = new ClientWebSocket();

        if(string.IsNullOrEmpty(Key))
        {
            _logger.LogError("Pulsoid key is not set. Please set the key in the settings");
            HubService.Log(new()
            {
                
                Origin = "Pulsoid",
                Message = "Pulsoid key is not set. Please set the key in the settings",
                Level = DoscLogLevel.Error,
            });
            return;
        }
        
        await _client.ConnectAsync(Uri, _cts.Token);

        var buffer = new byte[256];
        Status = true;
        Running = true;
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
                    StopService();
                }
                else
                {
                    HandleMessage(buffer, result.Count);
                }
            }
            catch (TaskCanceledException ex)
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Pulsoid encountered an error while listening for data: {ex}");
                HubService.Log(new()
                {
                
                    Origin = "Pulsoid",
                    Message = "Pulsoid encountered an error while listening for data",
                    Level = DoscLogLevel.Error,
                    Details = ex.ToString()
                });
            }
        }

        Status = false;
        Running = false;
    }


    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ignore if not enabled
        if (!Configuration.Enabled)
        {
            return;
        }
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        HubService.Log(new()
        {
                
            Origin = "Pulsoid",
            Message = "Pulsoid Service Started",
            Level = DoscLogLevel.Info,
        });
        Task.Run(() => Run(_cts.Token), _cts.Token);
  
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Running)
        {
            HubService.Log(new()
            {
                Origin = "Pulsoid",
                Message = "Pulsoid Service Stopped",
                Level = DoscLogLevel.Info,
            });
            Running = false;
        }
    }
}
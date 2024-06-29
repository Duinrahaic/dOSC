using System.Net;
using System.Net.WebSockets;
using dOSC.Drivers.Settings;
using dOSC.Shared.Models.Websocket;
using dOSC.Utilities;
using Newtonsoft.Json;

namespace dOSC.Drivers.Websocket;

public class WebSocketService : ConnectorBase
{
    public delegate void ConnectionCountChanged(int count);
    public event ConnectionCountChanged? OnConnectionCountChanged;
    
    private readonly WebSocketHandler _webSocketHandler;
    private HttpListener _listener = new();
    private CancellationTokenSource _cts;

    private Dictionary<string, WebSocket> _activeIdentities = new();

    public override string Name => "External Access";
    public override string Description => "Enables 3rd-party applications the ability to connect and communicate to the hub by websocket";
    private WebsocketSetting GetConfiguration() => (WebsocketSetting) Configuration;

    public int ActiveConnections
    {
        get => _activeConnections;
        private set
        {
            if (_activeConnections != value)
            {
                _activeConnections = value;
                OnConnectionCountChanged?.Invoke(value);
            }
        }
    } 
    private int _activeConnections = 0;

    private List<string> GetActiveConnections => _activeIdentities.Keys.ToList();
    
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
                    ActiveConnections = 0;
                    StopService();
                }
            }
        }
    }
    
    private int _port = 60065;
    public int Port
    {
        get => _port;
        set
        {
            if (_port != value)
            {
                _port = value;
                var config = GetConfiguration();
                config.Port = value;
                SaveConfiguration(config);
            }
        }
    }
    
    private string _key = "default";
    public string Key
    {
        get => _key;
        set
        {
            if (_key != value)
            {
                _key = value;
                var config = GetConfiguration();
                config.Key = value;
                SaveConfiguration(config);
            }
        }
    }



    public WebSocketService(IServiceProvider services) : base(services)
    {
        _webSocketHandler = services.GetRequiredService<WebSocketHandler>();
        Configuration = AppFileSystem.LoadSettings().Websocket;
        var config = GetConfiguration();
        _port = config.Port;
        _key = config.Key;
        
    }
    
    public static int GetDefaultPort() => new WebsocketSetting().Port;
    
    public string GetWebsocketUri() => $"ws://localhost:{_port}/";

    private async Task Run(CancellationToken stoppingToken)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{_port}/");
        _listener.Start();
        Running = true;

        while (!stoppingToken.IsCancellationRequested)
        {
            var context = await _listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var request = context.Request;
                var key = request.QueryString["apiKey"] ?? string.Empty;
                var identity = request.QueryString["identity"] ?? string.Empty;

                if (key != _key )
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.Close();
                    continue;
                }
                
                if (_activeIdentities.ContainsKey(identity) || string.IsNullOrEmpty(identity))
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict; 
                    context.Response.Close();
                    continue;
                }

                var wsContext = await context.AcceptWebSocketAsync(null);
                _activeIdentities[identity] = wsContext.WebSocket; 
                ActiveConnections++;
                Task.Run(async () =>
                {
                    try
                    {
                        await _webSocketHandler.AcceptWebSocketAsync(wsContext.WebSocket);
                    }
                    finally
                    {
                        ActiveConnections--;
                        _activeIdentities.Remove(identity);
                    }
                });
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }

        _listener.Stop();
        Running = false;
    }
    public override void StopService()
    {
        if (Running)
        {
            _listener.Stop();
            Running = false;
        }
    }
    public override void StartService()
    {
        if (!Running)
        {
            StartAsync(CancellationToken.None);
        }
    }
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ignore if not enabled
        if (!Configuration.Enabled)
        {
            return;
        }
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        Task.Run(() => Run(_cts.Token), _cts.Token);
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Running)
        {
            _listener.Stop();
            Running = false;
        }
    }
}
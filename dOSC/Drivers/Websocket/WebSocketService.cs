using System.Net;
using System.Net.WebSockets;
using dOSC.Drivers.Settings;
using dOSC.Shared.Models.Websocket;
using dOSC.Utilities;
using Newtonsoft.Json;

namespace dOSC.Drivers.Websocket;

public class WebSocketService : IHostedService
{
    public delegate void StateChanged(bool running);
    public event StateChanged? OnStateChanged;
    public delegate void ConnectionCountChanged(int count);
    public event ConnectionCountChanged? OnConnectionCountChanged;
    
    private readonly WebSocketManager _webSocketManager;
    private HttpListener _listener = new();
    private CancellationTokenSource _cts;

    private WebsocketSetting _configuration = new();
    private Dictionary<string, WebSocket> _activeIdentities = new(); 


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
    

    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (_isRunning != value)
            {
                _isRunning = value;
                OnStateChanged?.Invoke(value);
            }
        }
    }
    
    private bool _enabled = true;
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                _configuration.Enabled = value;
                SaveConfiguration(_configuration);
                if (_enabled)
                {
                    StartWebSocketService();
                }
                else
                {
                    ActiveConnections = 0;
                    StopWebSocketService();
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
                _configuration.Port = value;
                SaveConfiguration(_configuration);
            }
        }
    }
    
    private string _key = "default";
    private IHostedService _hostedServiceImplementation;

    public string Key
    {
        get => _key;
        set
        {
            if (_key != value)
            {
                _key = value;
                _configuration.Key = value;
                SaveConfiguration(_configuration);
            }
        }
    }



    public WebSocketService(IServiceProvider services)
    {
        _webSocketManager = services.GetRequiredService<WebSocketManager>();
        _configuration = AppFileSystem.LoadSettings().Websocket;
        _enabled = _configuration.Enabled;
        _port = _configuration.Port;
        _key = _configuration.Key;
        StartAsync(CancellationToken.None);
    }
    
    public int GetPort() => _configuration.Port;
    public static int GetDefaultPort() => new WebsocketSetting().Port;
    
    public void SetPort(int port)
    {
        _configuration.Port = port;
        SaveConfiguration(_configuration);
    }

    private void SaveConfiguration(WebsocketSetting configuration)
    {
        AppFileSystem.SaveSetting(configuration);
        _configuration = configuration;
    }

    public string GetWebsocketURI() => $"ws://localhost:{_port}/";

    private async Task Run(CancellationToken stoppingToken)
    {
        _listener.Prefixes.Add($"http://localhost:{_port}/");
        _listener.Start();
        IsRunning = true;

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
                        await _webSocketManager.AcceptWebSocketAsync(wsContext.WebSocket);
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
        IsRunning = false;
    }
    private void StopWebSocketService()
    {
        if (IsRunning)
        {
            _listener.Stop();
            IsRunning = false;
        }
    }
    private void StartWebSocketService()
    {
        if (!IsRunning)
        {
            StartAsync(CancellationToken.None);
        }
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ignore if not enabled
        if (!_configuration.Enabled)
        {
            return;
        }
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        Task.Run(() => Run(_cts.Token), _cts.Token);
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (IsRunning)
        {
            _listener.Stop();
            IsRunning = false;
        }
    }
}
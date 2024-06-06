using System.Net;
using dOSC.Shared.Models.Websocket;

namespace dOSC.Drivers.Websocket;

public class WebSocketService : BackgroundService
{
    public delegate void StateChanged(bool running);
    public event StateChanged? OnStateChanged;
    
    private readonly WebSocketManager _webSocketManager;
    private HttpListener _listener = new();
    
    private WebSocketConfiguration _configuration = new();

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
    
    public WebSocketService(IServiceProvider services)
    {
        _webSocketManager = services.GetRequiredService<WebSocketManager>();
        
        
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener.Prefixes.Add($"http://localhost:{_configuration.Port}/");
        _listener.Start();
        IsRunning = true;
        while (!stoppingToken.IsCancellationRequested)
        {
            var context = await _listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var request = context.Request;
                var found = request.QueryString["apiKey"];
                if (found != "1234")
                {
                    context.Response.StatusCode = 401;
                    context.Response.Close();
                    continue;
                }

                
                
                var wsContext = await context.AcceptWebSocketAsync(null);
                await _webSocketManager.AcceptWebSocketAsync(wsContext.WebSocket);
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
    
}
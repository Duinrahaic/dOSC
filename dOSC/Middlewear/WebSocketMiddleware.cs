using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Http;
using Timer = System.Timers.Timer;

namespace dOSCEngine.Websocket;
public partial class WebSocketMiddleware: IDisposable
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    private Timer _heartbeatTimer;
    private LogSink _sink;
    private readonly string _authKey;
    public WebSocketMiddleware(RequestDelegate next, LogSink sink, string authKey, IServiceProvider serviceProvider)
    {
        
        _sink = sink;
        _sink.LogEventReceived+= LogReceived;
        _authKey = authKey;
        _next = next;
        _serviceProvider = serviceProvider;
        _heartbeatTimer = new Timer(1000);
        _heartbeatTimer.AutoReset = true;
        _heartbeatTimer.Elapsed += Heartbeat;
        _heartbeatTimer.Start();

    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var request = context.Request;
            bool found = request.Query.TryGetValue("apiKey", out var apiKeyValue);

            /*
            if(found)
            {
                if (apiKeyValue != _authKey)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
                return;
            }
            */
            
            await WebSocketServer.HandleWebSocket(context);
        }
        else
        {
            await _next(context);
        }
    }

    public void Dispose()
    {
        _sink.LogEventReceived-= LogReceived;
        _heartbeatTimer.Dispose();
    }
}
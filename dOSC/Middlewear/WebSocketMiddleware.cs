using System;
using System.Threading.Tasks;
using dOSC.Shared.Utilities;
using dOSC.Utilities;
using Microsoft.AspNetCore.Http;
using Timer = System.Timers.Timer;

namespace dOSC.Middlewear;

public partial class WebSocketMiddleware : IDisposable
{
    private readonly string _authKey;
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    private readonly Timer _heartbeatTimer;
    private readonly LogSink _sink;

    public WebSocketMiddleware(RequestDelegate next,IServiceProvider serviceProvider)
    {
        _sink = LogPool.Sink;
        _sink.LogEventReceived += LogReceived;
        _authKey = EncryptionHelper.Key;
        _next = next;
        _serviceProvider = serviceProvider;
        _heartbeatTimer = new Timer(1000 * 10);
        _heartbeatTimer.AutoReset = true;
        _heartbeatTimer.Elapsed += Heartbeat;
        _heartbeatTimer.Start();
    }

    public void Dispose()
    {
        _sink.LogEventReceived -= LogReceived;
        _heartbeatTimer.Dispose();
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var request = context.Request;
            var found = request.Query.TryGetValue("apiKey", out var apiKeyValue);

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
}
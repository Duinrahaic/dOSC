using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks.Dataflow;
using dOSCEngine.Utilities;
using dOSCEngine.Websocket;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace dOSCEngine;

public class WebSocketMiddleware: IDisposable
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    private Timer _heartbeatTimer;
    private LogSink _sink;
    
    public WebSocketMiddleware(RequestDelegate next, LogSink sink, IServiceProvider serviceProvider)
    {
        
        _sink = serviceProvider.GetRequiredService<LogSink>();
        _sink.LogEventReceived+= async (sender, e) =>
        {
            if (e.Level == Serilog.Events.LogEventLevel.Error)
            {
                await WebsocketServer.Broadcast("dOSC Hub Error: " + e.MessageTemplate.Text);
            }
            await WebsocketServer.Broadcast("dOSC Hub Error: " + e.MessageTemplate.Text);
        };
        
        _next = next;
        _serviceProvider = serviceProvider;
        _heartbeatTimer = new Timer(1000);
        _heartbeatTimer.AutoReset = true;
        _heartbeatTimer.Elapsed += async (sender, e) =>
        {
            await WebsocketServer.Broadcast("dOSC Hub Time: " + DateTime.Now.ToString("HH:mm:ss"));

        };
        _heartbeatTimer.Start();

    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            await WebsocketServer.HandleWebSocket(context);
        }
        else
        {
            await _next(context);
        }
    }
    
    
    
    
    
    

    public void Dispose()
    {
        _heartbeatTimer.Dispose();
    }
}
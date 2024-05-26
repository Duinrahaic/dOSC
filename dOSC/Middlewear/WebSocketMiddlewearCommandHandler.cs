using System;
using System.Timers;

namespace dOSC.Middlewear;

public partial class WebSocketMiddleware
{
    private async void Heartbeat(object? sender, ElapsedEventArgs e)
    {
        await WebSocketServer.Broadcast("dOSC dOSC.Hub Time: " + DateTime.Now.ToString("HH:mm:ss"));
    }
}
using System;
using System.Timers;
using dOSC.Shared.Models.Commands;
using LogLevel = dOSC.Shared.Models.Commands.LogLevel;

namespace dOSC.Middlewear;

public partial class WebSocketMiddleware
{
    private async void Heartbeat(object? sender, ElapsedEventArgs e)
    {
        string origin = "Hub";
        string target = "All Clients";
        string message = "Hub Heartbeat at " + DateTime.Now.ToString("HH:mm:ss");
        var data = new Log(DateTime.Now.ToString(), origin, LogLevel.Info, message); 
        Command heartbeat = new(origin, target, "Heartbeat", "Log", data:data);
        await WebSocketServer.Broadcast(heartbeat);
    }
}
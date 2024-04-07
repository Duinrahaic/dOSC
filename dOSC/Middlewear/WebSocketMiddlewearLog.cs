using Serilog.Events;

namespace dOSC.Middlewear;

public partial class WebSocketMiddleware
{
    private async void LogReceived(object? sender, LogEvent e)
    {
        await WebSocketServer.Broadcast("dOSC dOSC.Hub Error: " + e.MessageTemplate.Text);
    }
}
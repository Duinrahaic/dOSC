using Serilog.Core;
using Serilog.Events;

namespace dOSCEngine.Utilities;

public class MergeLogSink: ILogEventSink
{
    public event EventHandler<LogEvent> LogEventReceived;

    
    public void Emit(LogEvent logEvent)
    {
        LoggerProvider.Logger.Write(logEvent);
        LogEventReceived?.Invoke(this, logEvent);
    }
}
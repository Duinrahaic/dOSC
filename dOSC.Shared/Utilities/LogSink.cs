using Serilog.Core;
using Serilog.Events;

namespace dOSCEngine.Utilities;

public class LogSink: ILogEventSink
{
    public event EventHandler<LogEvent>? LogEventReceived;
    
    private Queue<LogEvent> _buffer;

    private int _size;
    public LogSink()
    {
        this._size = 1000;
        _buffer = new Queue<LogEvent>(1000);
    }
    public LogSink(int size)
    {
        this._size = size;
        _buffer = new Queue<LogEvent>(size);
    }
    
    public void Emit(LogEvent logEvent)
    {
        LogEventReceived?.Invoke(this, logEvent);
    }
}
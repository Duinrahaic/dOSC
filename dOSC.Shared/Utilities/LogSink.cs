using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace dOSC.Shared.Utilities;

public class LogSink : ILogEventSink
{
    private Queue<LogEvent> _buffer;

    private int _size;

    public LogSink()
    {
        _size = 1000;
        _buffer = new Queue<LogEvent>(1000);
    }

    public LogSink(int size)
    {
        _size = size;
        _buffer = new Queue<LogEvent>(size);
    }

    public void Emit(LogEvent logEvent)
    {
        LogEventReceived?.Invoke(this, logEvent);
    }

    public event EventHandler<LogEvent>? LogEventReceived;
}
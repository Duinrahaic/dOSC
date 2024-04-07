using Serilog.Events;

namespace dOSCEngine.Websocket.Commands;

public class dOSCLog: dOSCData
{
    public string TimeStamp { get; set; }
    public string Origin { get; set; }
    public string Source { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    
    public dOSCLog(string timeStamp, string origin, string source, string level, string message)
    {
        TimeStamp = timeStamp;
        Origin = origin;
        Source = source;
        Level = level;
        Message = message;
    }
    
    public override string ToString()
    {
        return $"[{TimeStamp}] {Origin} {Source} {Level}: {Message}";
    }
    
    public static dOSCLog FromLogEvent(LogEvent logEvent)
    {
        return new dOSCLog(logEvent.Timestamp.ToString(), logEvent.Properties["Origin"].ToString(), logEvent.Properties["Source"].ToString(), logEvent.Level.ToString(), logEvent.MessageTemplate.Text);
    }
}
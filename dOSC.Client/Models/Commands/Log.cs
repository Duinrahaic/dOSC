using System;
using System.Globalization;

namespace dOSC.Client.Models.Commands;

public class Log : Data
{
    
    
    public Log()
    {
    }

    public Log(string origin, DoscLogLevel level, string message)
    {
        Origin = origin;
        Level = level;
        Message = message;
    }

    public Log(string origin, DoscLogLevel level, string message, string details)
    {
        Origin = origin;
        Level = level;
        Message = message;
        Details = details;
    }

    public Log(string timeStamp, string origin, DoscLogLevel level, string message)
    {
        TimeStamp = timeStamp;
        Origin = origin;
        Level = level;
        Message = message;
    }

    public Log(string timeStamp, string origin, DoscLogLevel level, string message, string details)
    {
        TimeStamp = timeStamp;
        Origin = origin;
        Level = level;
        Message = message;
        Details = string.Empty;
    }
    
    public override CommandType Type { get; set; } = CommandType.Log;
    public string TimeStamp { get; set; } = DateTime.Now.ToString(CultureInfo.CurrentCulture);
    public string Origin { get; set; } = string.Empty;
    public DoscLogLevel Level { get; set; } = DoscLogLevel.Info;
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{TimeStamp}] {Origin} {Level}: {Message}";
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }
}
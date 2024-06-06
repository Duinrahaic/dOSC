using dOSC.Shared.Models.Commands;

namespace dOSC.Drivers;

public partial class HubService
{
    public delegate void OnLogReceived(Log log);

    public OnLogReceived? LogReceived;
    
    private Queue<Log> _logQueue = new();
    public void Log(Log log)
    {
        _logQueue.Enqueue(log);
        while(_logQueue.Count > _config.MaxLogHistory)
        {
            _logQueue.Dequeue();
        }
        LogReceived?.Invoke(log);
    }
    
    public Queue<Log> GetLogs() => _logQueue;
    public uint GetLogCount() => (uint)_logQueue.Count;
    public uint GetMaxLogHistory() => _config.MaxLogHistory;
}
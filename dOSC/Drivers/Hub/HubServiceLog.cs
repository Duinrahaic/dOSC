using dOSC.Client.Models.Commands;

namespace dOSC.Drivers.Hub;

public partial class HubService
{
    public delegate void OnLogReceived(Log log);

    public OnLogReceived? LogReceived;
    
    private Queue<Log> _logQueue = new();
    public void Log(Log log)
    {
        try
        {
            _logQueue.Enqueue(log);
            while(_logQueue.Count > _config.MaxLogHistory)
            {
                _logQueue.Dequeue();
            }
            LogReceived?.Invoke(log);
        }
        catch
        {
            // Ignored
        }
    }
    
    public Queue<Log> GetLogs() => _logQueue;
    public void ClearLogs() => _logQueue.Clear();
    public uint GetLogCount() => (uint)_logQueue.Count;
    public uint GetMaxLogHistory() => _config.MaxLogHistory;
}
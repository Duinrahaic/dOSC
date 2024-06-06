using dOSC.Shared.Models.Commands;

namespace dOSC.Drivers;

public partial class HubService: BackgroundService
{    
    public delegate void AlarmStateChanged(bool inAlarm);
    public event AlarmStateChanged? OnAlarmStateChanged;

    private bool _alarmState = false;
    public bool IsInAlarm
    {
        get => _alarmState;
        set
        {
            if (_alarmState != value)
            {
                _alarmState = value;
                OnAlarmStateChanged?.Invoke(value);
            }
        }
    }
    
    private readonly ILogger<HubService> _logger;
    
    private HubConfiguration _config = new();
    
    public HubService(ILogger<HubService> logger,IServiceProvider services)
    {
        _logger = logger;
        _logger.LogInformation("Initialized HubService Service");
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Ignore
    }
}
using System;
using System.Timers;

namespace dOSC.Shared.Utilities;

public static class VisualUpdateTimer
{
    private static readonly Timer timer = new();
    public static bool IsRunning => timer.Enabled;
    public static event Action? OnTimerElapsed;

    public static void Start()
    {
        timer.Start();
    }

    public static void Stop()
    {
        timer.Stop();
    }

    public static void Initialize()
    {
        timer.Elapsed += (sender, e) => OnTimerElapsed?.Invoke();
        timer.Interval = 100;
        timer.AutoReset = true;
        //timer.Listen();
    }
}
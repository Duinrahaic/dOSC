using System.Diagnostics;

namespace dOSC.Shared.Utilities;

public static class GlobalStopwatch
{
    private static readonly Stopwatch Stopwatch = new();
    public static bool IsRunning => Stopwatch.IsRunning;

    public static void Start()
    {
        Stopwatch.Start();
    }

    public static void Stop()
    {
        Stopwatch.Stop();
    }

    public static void Reset()
    {
        Stopwatch.Restart();
    }

    public static long GetTicks()
    {
        return Stopwatch.Elapsed.Ticks;
    }
}
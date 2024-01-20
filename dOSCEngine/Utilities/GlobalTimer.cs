using System.Timers;

namespace dOSCEngine.Utilities
{
    public static class GlobalTimer
    {
        public static event Action? OnTimerElapsed;
        private static System.Timers.Timer timer = new();
        public static bool IsRunning { get { return timer.Enabled; } }
        public static void Start() => timer.Start();
        public static void Stop() => timer.Stop();

        public static void Initialize()
        {
            timer.Elapsed += (sender, e) => OnTimerElapsed?.Invoke();
            timer.Interval = 100;
            timer.AutoReset = true;
            timer.Start();
        }


    }
}

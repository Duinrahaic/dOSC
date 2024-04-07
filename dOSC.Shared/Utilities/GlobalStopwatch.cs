using System.Diagnostics;

namespace dOSCEngine.Utilities
{
    public static class GlobalStopwatch
    {
        private static Stopwatch Stopwatch = new Stopwatch();
        public static bool IsRunning { get { return Stopwatch.IsRunning; } }
        public static void Start() => Stopwatch.Start();
        public static void Stop() => Stopwatch.Stop();
        public static void Reset() => Stopwatch.Restart();
        public static long GetTicks() => Stopwatch.Elapsed.Ticks;
    }
}

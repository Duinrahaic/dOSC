using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Utilities
{
    public static class VisualUpdateTimer
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
            //timer.Listen();
        }


    }
}

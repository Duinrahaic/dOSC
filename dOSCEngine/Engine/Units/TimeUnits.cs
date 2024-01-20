using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Units
{
    public enum TimeUnits : int
    {
        Millisecond = 1,
        Second = 1000,
        Minute = 60 * Second,
        Hour = 60 * Minute,
        Day = 24 * Hour,
    }
}

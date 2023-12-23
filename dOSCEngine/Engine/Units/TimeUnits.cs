using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Units
{
    public enum TimeUnits : int
    {
        millisecond = 1,
        second = 1000,
        minute = 60 * second,
        hour = 60 * minute,
        day = 24 * hour,
    }
}

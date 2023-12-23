using dOSCEngine.Engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Utilities
{
    public static class BeautifyString
    {
        public static string BeautifyMilliseconds(int totalMilliseconds)
        {
            // Ensure input is a positive number
            totalMilliseconds = Math.Abs(totalMilliseconds);

            // Calculate units
            int days = totalMilliseconds / (int)TimeUnits.day;
            int hours = (totalMilliseconds % (int)TimeUnits.day) / (int)TimeUnits.hour;
            int minutes = (totalMilliseconds % (int)TimeUnits.hour) / (int)TimeUnits.minute;
            int seconds = (totalMilliseconds % (int)TimeUnits.minute) / (int)TimeUnits.second;


            // Build the human-readable string
            string result = "";

            if (days > 0)
            {
                result = $"{days} {(days == 1 ? "day" : "days")}";
                if (hours > 0) result += $" {hours} {(hours == 1 ? "hr" : "hrs")}";
            }
            else if (hours > 0)
            {
                result = $"{hours} {(hours == 1 ? "hr" : "hrs")}";
                if (minutes > 0) result += $" {minutes} {(minutes == 1 ? "min" : "mins")}";
            }
            else if (minutes > 0)
            {
                result = $"{minutes} {(minutes == 1 ? "min" : "mins")}";
                if (seconds > 0) result += $" {seconds} {(seconds == 1 ? "sec" : "secs")}";
            }
            else if (totalMilliseconds < (int)TimeUnits.second)
            {
                result = "< 1 second";
            }
            else
            {
                result = $"{seconds} {(seconds == 1 ? "sec" : "secs")}";
            }

            return result;
        }
    }
}

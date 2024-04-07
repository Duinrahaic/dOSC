using System;

namespace dOSC.Shared.Utilities;

public static class BeautifyString
{
    public static string BeautifyMilliseconds(TimeSpan timeSpan, bool NumbersOnly = false)
    {
        // Build the human-readable string
        string result;

        if (NumbersOnly) // Numbers Only
        {
            if (timeSpan.Days > 0)
                result =
                    $"{(long)timeSpan.TotalDays:0}:{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            else if (timeSpan.Hours > 0)
                result = $"{timeSpan.Hours:0}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            else if (timeSpan.Minutes > 0)
                result = $"{timeSpan.Minutes:0}:{timeSpan.Seconds:00}";
            else
                result = $"{timeSpan.Seconds}.{timeSpan.Milliseconds:000}";
        }
        else // Text
        {
            if (timeSpan.Days > 0)
                result =
                    $"{(long)timeSpan.TotalDays:0}d {timeSpan.Hours:00}h {timeSpan.Minutes:00}m {timeSpan.Seconds:00}s";
            else if (timeSpan.Hours > 0)
                result = $"{timeSpan.Hours:0}h {timeSpan.Minutes:00}m {timeSpan.Seconds:00}s";
            else if (timeSpan.Minutes > 0)
                result = $"{timeSpan.Minutes:0}m {timeSpan.Seconds:00}s";
            else
                result = $"{timeSpan.Seconds}s.{timeSpan.Milliseconds:000}ms";
        }

        return result;
    }
}
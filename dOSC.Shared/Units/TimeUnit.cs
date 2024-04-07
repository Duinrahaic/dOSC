namespace dOSCEngine.Units;

public enum TimeUnit: int
{
    Millisecond = 1,
    Second = 1000,
    Minute = 60 * Second,
    Hour = 60 * Minute,
    Day = 24 * Hour,
}
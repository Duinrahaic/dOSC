using dOSC.Shared.Utilities;
using LiteDB;

namespace dOSC.Component.Wiresheet.Nodes.Utility;

public class DelayAction
{
    public string Guid = System.Guid.NewGuid().ToString();

    public DelayAction(BsonValue Value, TimeSpan Duration)
    {
        this.Value = Value;
        this.Duration = Duration;
    }

    public BsonValue Value { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public async Task Start()
    {
        StartTime = DateTime.Now;
        EndTime = DateTime.Now.AddMilliseconds(Duration.TotalMilliseconds);
        await Task.CompletedTask;
    }

    public string IndicatorToString(bool Percent = false, bool NumbersOnly = false)
    {
        if (CalculateRemainingPercent() == 0) return "Waiting";

        if (Percent) return $"{CalculateRemainingPercent().ToString("N1")}%";

        var remainingTime = EndTime - DateTime.Now;
        return BeautifyString.BeautifyMilliseconds(remainingTime, NumbersOnly);
    }

    public double CalculateRemainingPercent()
    {
        var remainingTime = CalculateRemainingTime();
        // Calculate percentage completion
        var percentRemaining = remainingTime.TotalMilliseconds / Duration.TotalMilliseconds * 100;
        return Math.Clamp(percentRemaining, 0.0, 100.0);
    }

    public TimeSpan CalculateRemainingTime()
    {
        var remainingTime = EndTime - DateTime.Now;
        if (remainingTime < TimeSpan.Zero)
            return TimeSpan.Zero;
        return remainingTime;
    }
}
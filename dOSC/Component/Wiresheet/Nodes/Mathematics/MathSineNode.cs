using dOSC.Utilities;
using LiveSheet.Parts.Events;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;
using LiveSheet.Utilities;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathSineNode: MathNode
{
    public MathSineNode() : base()
    {
        AddPort(new LiveNumericPort(this, false, name: "Output"));
        SyncedTimer.TimeUpdated += GetSineWave;

    }

    public override string NodeName => "Sine";
    public override string Icon => "~";
    public override bool IconIsText => true;
    
    [LiveSerialize]
    public decimal Frequency { get; set; } = 1;
    [LiveSerialize]
    public decimal Amplitude { get; set; } = 1;
    
    private readonly object _generate = new();
    private void GetSineWave(object? sender, TimeEventArgs e)
    {
        lock (_generate)
        {
            DateTime currentTime = e.CurrentTime;
            var time = (decimal)currentTime.TimeOfDay.TotalSeconds; // Current time in seconds
            Value = Math.Round(Amplitude * DecimalSin(2 * (decimal)Math.PI * Frequency * time),3);
        }
    }

    private static decimal DecimalSin(decimal angleInRadians)
    {
        double angleDouble = (double)angleInRadians;
        double sinDouble = Math.Sin(angleDouble);
        return (decimal)sinDouble;
    }

    private static decimal DegreesToRadians(decimal degrees)
    {
        return degrees * (decimal)Math.PI / 180.0m;
    }
    
    
    public override void Dispose()
    {
        SyncedTimer.TimeUpdated -= GetSineWave;
        base.Dispose();
    }

}
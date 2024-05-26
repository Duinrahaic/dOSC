using dOSC.Shared.Utilities;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathSineNode: MathNode
{
    public MathSineNode() : base()
    {
        AddPort(new LiveNumericPort(this, false, name: "Output"));
        GlobalTimer.OnTimerElapsed += GetSineWave;

    }

    public override string NodeName => "Power";
    public override string Icon => "~";
    public override bool IconIsText => true;
    
    [LiveSerialize]
    public double Frequency { get; set; } = 1.0;
    [LiveSerialize]
    public double Amplitude { get; set; } = 1.0;
    
    private readonly object _generate = new();
    private void GetSineWave()
    {
        lock (_generate)
        {
            var time = DateTime.Now.TimeOfDay.TotalSeconds; // Current time in seconds
            Value = Amplitude * Math.Sin(2 * Math.PI * Frequency * time);
        }
    }

    public override void Dispose()
    {
        GlobalTimer.OnTimerElapsed -= GetSineWave;
        base.Dispose();
    }

}
using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Utilities;

namespace dOSC.Client.Engine.Nodes.Mathematics;

public class SineNode : BaseNode
{
    private readonly double _amplitude;
    private readonly double _frequency;


    private readonly object Generate = new();

    public SineNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null) : base(guid, position, properties)
    {
        AddPort(new NumericPort(PortGuids.Port_1, this, false, "Output"));

        Properties.TryInitializeProperty(EntityPropertyEnum.Amplitude, 1.0);
        Properties.TryInitializeProperty(EntityPropertyEnum.Frequency, 1.0);

        _amplitude = Properties.GetProperty<double>(EntityPropertyEnum.Amplitude);
        _frequency = Properties.GetProperty<double>(EntityPropertyEnum.Frequency);
        GlobalTimer.OnTimerElapsed += GetSineWave;
    }

    public override string Name => "Sine Wave";
    public override string Category => NodeCategoryType.Math;
    public override string TextIcon => "∿";

    private void GetSineWave()
    {
        lock (Generate)
        {
            var time = DateTime.Now.TimeOfDay.TotalSeconds; // Current time in seconds
            Value = _amplitude * Math.Sin(2 * Math.PI * _frequency * time);
        }
    }

    public override void OnDispose()
    {
        GlobalTimer.OnTimerElapsed -= GetSineWave;
    }
}
using Blazor.Diagrams.Core.Geometry;
using Newtonsoft.Json;

namespace dOSC.Client.Engine.Nodes.Utility;

public class CounterNode : BaseNode
{
    [JsonProperty] private uint _Count;

    private uint _StartPoint;

    public CounterNode(Guid? guid = null, Point? position = null) : base(guid ?? Guid.NewGuid(),
        position ?? new Point(0, 0))
    {
        // AddPort(new LogicPort(PortGuids.Port_2, this, true , "Count Up")); // Count Up
        // AddPort(new LogicPort(PortGuids.Port_3, this, true)); // Count Down
        // AddPort(new LogicPort(PortGuids.Port_4, this, true)); // Reset
        // AddPort(new NumericPort(PortGuids.Port_5, this, false));
    }

    public uint Count
    {
        get => _Count;
        set => _Count = value;
    }

    public override void CalculateValue()
    {
        var Input = Ports[0];

        if (Input != null)
            if (Input.Links.Any())
                _StartPoint = GetInputValue(Input, Links.First());
        var CountUP = Ports[1];
        var CountDown = Ports[2];
        var Reset = Ports[3];
    }

    public void CountUp()
    {
        _Count++;
    }

    public void CountDown()
    {
        _Count--;
    }
}
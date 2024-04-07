using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Logic;

public class NotNode : BaseNode
{
    public NotNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null) : base(guid, position, properties)
    {
        AddPort(new LogicPort(PortGuids.Port_1, this, true, "Value A"));
        AddPort(new LogicPort(PortGuids.Port_2, this, false, "Output"));
    }

    public override string Name => "Not";
    public override string Category => NodeCategoryType.Logic;
    public override string TextIcon => "!A";

    public override void CalculateValue()
    {
        var inA = Ports[0];
        if (inA.Links.Any())
        {
            var l1 = inA.Links.First();
            var ValA = GetInputValue(inA, l1);

            if (ValA != null) Value = !Convert.ToBoolean(ValA);
        }
        else
        {
            Value = false;
        }
    }
}
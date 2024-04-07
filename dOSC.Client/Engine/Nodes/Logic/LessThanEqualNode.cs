using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Logic;

public class LessThanEqualNode : BaseNode
{
    public LessThanEqualNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null) : base(guid, position, properties)
    {
        AddPort(new NumericPort(PortGuids.Port_1, this, true, "Value A"));
        AddPort(new NumericPort(PortGuids.Port_2, this, true, "Value B"));
        AddPort(new LogicPort(PortGuids.Port_3, this, false, "Output"));
    }

    public override string Name => "Less Than Equal To";
    public override string Category => NodeCategoryType.Logic;
    public override string TextIcon => "<=";

    public override void CalculateValue()
    {
        var inA = Ports[0];
        var inB = Ports[1];
        if (inA.Links.Any() && inB.Links.Any())
        {
            var l1 = inA.Links.First();
            var l2 = inB.Links.First();
            var ValA = GetInputValue(inA, l1);
            var ValB = GetInputValue(inB, l2);

            if (ValA != null && ValB != null) Value = Convert.ToBoolean(ValA) <= Convert.ToBoolean(ValB);
        }
        else
        {
            Value = false;
        }
    }
}
﻿using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Constant
{
    public class NumericNode : BaseNode
    {
        public NumericNode(double value = 0.0, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            Value = value;
        }
        public NumericNode(Guid guid, double value = 0.0, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            Value = value;            
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";

    }
}

﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Math
{
    public class DivisionNode : BaseNode
    {
        public DivisionNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, true));
            AddPort(new NumericPort(this, true));
            AddPort(new NumericPort(this, false));
        }
        public override string BlockTypeClass => "numericblock";

        public override void Refresh()
        {
            var i1 = Ports[0];
            var i2 = Ports[1];
            if (i1.Links.Count > 0 && i2.Links.Count > 0)
            {
                var l1 = i1.Links[0];
                var l2 = i2.Links[0];
                var v1 = GetInputValue(i1, l1);
                var v2 = GetInputValue(i2, l2);
                Value = v1 / v2;
            }
            else if (i1.Links.Count > 0)
            {
                var l1 = i1.Links[0];
                var v1 = GetInputValue(i1, l1);
                Value = v1;
            }
            else if (i2.Links.Count > 0)
            {
                var l2 = i2.Links[0];
                var v2 = GetInputValue(i2, l2);
                Value = v2;
            }
            else
            {
                Value = 0;
            }
            base.Refresh();
        }

    }
}

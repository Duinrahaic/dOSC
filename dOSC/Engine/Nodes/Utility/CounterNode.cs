using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Utility
{
    public class CounterNode : BaseNode
    {
        public CounterNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, true)); // Input
            AddPort(new LogicPort(this, true)); // Count Up
            AddPort(new LogicPort(this, true)); // Count Down
            AddPort(new LogicPort(this, true)); // Reset
            AddPort(new NumericPort(this, false));
        }
        public override string BlockTypeClass => "numericblock";
        private uint _Count = 0;
        public uint Count { get => _Count; set => _Count = value; }

        private uint _StartPoint = 0;

        public override void Refresh()
        {
            var Input = Ports[0];

            if (Input != null)
            {
                if (Input.Links.Any())
                {
                    _StartPoint = GetInputValue(Input, Links.First());
                }
            }




            var CountUP = Ports[1];
            var CountDown = Ports[2];
            var Reset = Ports[3];

           
            



            base.Refresh();
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
}

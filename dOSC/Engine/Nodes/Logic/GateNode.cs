using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Logic
{
   
    public class GateNode : BaseNode
    {
        [Obsolete("Has been depricated. Nodes broken out.")]
        public GateNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        public GateNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        [JsonProperty]
        public override string NodeClass => this.GetType().Name.ToString();
        public override string BlockTypeClass => "logicblock";

        public enum GateOptions
        {
            GreaterThen,
            LessThen,
            GreaterThenOrEqualTo,
            LessThenOrEqualTo,
            EqualTo,
            NotEqualTo
        }

        public GateOptions SelectedGateOption = GateOptions.GreaterThen;
        public string SelectedGateOptionText => Options.FirstOrDefault(x => x.Key == SelectedGateOption).Value;

        public void SetOption(GateOptions option)
        {
            SelectedGateOption = option;
        }



        public Dictionary<GateOptions, string > Options = new(){
            { GateOptions.GreaterThen ,"Greater Then"},
            { GateOptions.LessThen ,"Less Then"},
            { GateOptions.GreaterThenOrEqualTo ,"Greater Then Or Equal To"},
            { GateOptions.LessThenOrEqualTo ,"Less Then Or Equal To"},
            { GateOptions.EqualTo ,"Equal To"},
            { GateOptions.NotEqualTo ,"Not Equal To"}
        };

        public override void Refresh()
        {
            var inA = Ports[0];
            var inB = Ports[1];
            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();

                var valA = GetInputValue(inA, l1);
                var valB = GetInputValue(inB, l2);


                switch (SelectedGateOption){
                    case GateOptions.GreaterThen:
                        Value =  valA > valB;
                        break;
                    case GateOptions.LessThen:
                        Value = valA < valB;
                        break;
                    case GateOptions.NotEqualTo:
                        Value = valA != valB;
                        break;
                    case GateOptions.GreaterThenOrEqualTo:
                        Value = valA >= valB;
                        break;
                    case GateOptions.LessThenOrEqualTo:
                        Value = valA <= valB;
                        break;
                    case GateOptions.EqualTo:
                        Value = valA == valB;
                        break;
                    default:
                        Value = false;
                        break;
                }
            }
            else
            {
                Value = false;
            }
            base.Refresh();
        }


    }
}

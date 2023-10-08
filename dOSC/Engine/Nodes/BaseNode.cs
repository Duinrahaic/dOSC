using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using System.Xml.Linq;

namespace dOSC.Engine.Nodes
{
    public abstract class BaseNode : NodeModel
    {
        private dynamic _value;
        private bool _showLinksLabel = false;
        public event Action<BaseNode>? ValueChanged;



        protected BaseNode(Point position) : base(position)
        {

            this.ShowLinksLabel(true);

        }
        public string BlockHeaderClass => $"block {BlockTypeClass} {ErrorClass}";
        public virtual string BlockTypeClass => string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool Error { get; set; } = false;
        private string ErrorClass => Error ? "error" : string.Empty;
        public dynamic Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;
                _value = value;
                ValueChanged?.Invoke(this);
            }
        }

        protected virtual dynamic GetInputValue(PortModel port, BaseLinkModel link)
        { 
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Source as SinglePortAnchor)!;
            var p = sp.Port == port ? tp : sp;
            return (p.Port.Parent as BaseNode)!.Value;
        }

        public virtual void ResetValue()
        {
            Value = null!;
        }
       

        public double InputValue(PortModel port, BaseLinkModel link)
        {
            return GetInputValue(port, link);
        }



        public void ShowLinksLabel(bool? show = null)
        {
            if (show == null)
            {
                _showLinksLabel = !_showLinksLabel;
            }
            else
            {
                _showLinksLabel = show.Value;
            }



            foreach (var port in Ports)
            {



                if (port.Links.Count > 0)
                {
                    var l = port.Links[0];
                    if (_showLinksLabel)
                    {
                        double v = 0;
                        double distance = 0.5;
                        var cpm = port as NumericPort;
                        if (cpm == null)
                        { //unknown
                            distance = 0.5;
                            v = Value;
                        }
                        else if (cpm.Input)
                        { //input
                            distance = 0.8;
                            v = GetInputValue(port, l);
                        }
                        else
                        { //output
                            distance = 0.2;
                            v = Value;
                        }



                        l.AddLabel(v.ToString(), distance);
                    }
                    else
                    {
                        l.Labels.Clear();
                    }
                    Refresh();
                }
            }

        }
    }
}

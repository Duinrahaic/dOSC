using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using System.Xml.Linq;
using Newtonsoft.Json;
using dOSCEngine.Engine.Ports;
using System.Reflection.Emit;
using dOSCEngine.Engine.Links;

namespace dOSCEngine.Engine.Nodes
{
    public abstract class BaseNode : NodeModel
    {
        private dynamic _value;
        public event Action<BaseNode>? ValueChanged;

        protected BaseNode(Point position) : base(position)
        {

        }

        protected BaseNode(Guid guid, Point position) : base(position)
        {
            Guid = guid;

        }
        public IReadOnlyList<BasePort> GetPorts()
        {
            return (IReadOnlyList<BasePort>)Ports;
        }

        public Guid Guid { get; set; } = Guid.NewGuid();
        public virtual string NodeClass => "base";
        public virtual string Option => string.Empty;
        public string BlockHeaderClass => $"block {BlockTypeClass} {ErrorClass}";
        public virtual string BlockTypeClass => string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool Error { get; set; } = false;
        private string ErrorClass => Error ? "error" : string.Empty;
        private DateTime _LastUpdate = DateTime.MinValue;
        public DateTime LastUpdate => _LastUpdate;
        public virtual string OriginalName { get;set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                return OriginalName;
            }
            return DisplayName;
        }

        
        public dynamic Value
        {
            get => _value;
            set
            {
                
                if (_value == value)
                    return;
                _value = value;
                ValueChanged?.Invoke(this);
                _LastUpdate = DateTime.Now;
            }
            
        }


        public void SetValue(dynamic value, bool Refresh = true)
        {
            if (Refresh)
            {
                Value = value;
            }
            else
            {
                _value = value;
            }
        }

        public virtual void CalculateValue()
        {

        }


        protected virtual dynamic GetInputValue(PortModel port, BaseLinkModel link)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;
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

  
        public BaseNodeDTO GetDTO()
        {
            return new BaseNodeDTO(this);
        }
    }
}

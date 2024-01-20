using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{
    public class ClampNode : BaseNode
    {
        public ClampNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {

            AddPort(new NumericPort(PortGuids.Port_1, this, true, name:"Value"));
            AddPort(new NumericPort(PortGuids.Port_2, this, false, name: "Output"));

            Properties.TryInitializeProperty(EntityPropertyEnum.Max, 1.0);
            Properties.TryInitializeProperty(EntityPropertyEnum.Min, -1.0);
            Properties.TryInitializeProperty(EntityPropertyEnum.NoMax, false);
            Properties.TryInitializeProperty(EntityPropertyEnum.NoMin, false);
            
        }
        public override string Name => "Clamp";
        public override string Category => NodeCategoryType.Math;
        public override string TextIcon 
        {
            get
            {
                if (_noMax && _noMin)
                {
                    return "[∞,∞]";
                }
                else if (!_noMin && _noMax)
                {
                    return "[a,∞]";
                }
                else if (!_noMax && _noMin)
                {
                    return "[∞,b]";
                }
                else
                {
                    return "[a,b]";
                }
            }
        }

        private bool _noMax;
        private bool _noMin;
        private double _max;
        private double _min;
        
        
        public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
        {
            if(property == EntityPropertyEnum.NoMax)
            {
                _noMax = value;
            }
            else if(property == EntityPropertyEnum.NoMin)
            {
                _noMin = value;
            }
            else if(property == EntityPropertyEnum.Max)
            {
                _max = value;
            }
            else if(property == EntityPropertyEnum.Min)
            {
                _min = value;
            }
           
        }

        public override void CalculateValue()
        {
            var input = Ports[0];
            if (!input.Links.Any())
            {
                SetValue(null!, false); 
            }
            else
            {
                
                var input_val = GetInputValue(input, input.Links.First());
                
                if(input ==  null)
                {
                    SetValue(null!, false);
                }
                else
                {
                
                    if (_noMax && _noMin)
                    {
                        Value = input_val;
                    }
                    else if( !_noMax && !_noMin)
                    {
                        Value = System.Math.Clamp(input_val, _min, _max);
                    }
                    else if (_noMin == false) 
                    {
                        Value = System.Math.Max(_max, input_val);
                    }
                    else if(_noMax == false)
                    {
                        Value = System.Math.Min(_min,input_val);  
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}

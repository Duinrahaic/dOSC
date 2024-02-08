using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{
    public class ClampNode : BaseNode
    {
        public ClampNode(Guid? guid = null, ConcurrentDictionary<EntityProperty, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {

            AddPort(new NumericPort(PortGuids.Port_1, this, true, name:"Value"));
            AddPort(new NumericPort(PortGuids.Port_2, this, false, name: "Output"));

            Properties.TryInitializeProperty(EntityProperty.Max, 1.0);
            Properties.TryInitializeProperty(EntityProperty.Min, -1.0);
            Properties.TryInitializeProperty(EntityProperty.NoMax, false);
            Properties.TryInitializeProperty(EntityProperty.NoMin, false);
            
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
        
        
        public override void PropertyNotifyEvent(EntityProperty property, dynamic? value)
        {
            if(property == EntityProperty.NoMax)
            {
                _noMax = value;
            }
            else if(property == EntityProperty.NoMin)
            {
                _noMin = value;
            }
            else if(property == EntityProperty.Max)
            {
                _max = value;
            }
            else if(property == EntityProperty.Min)
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


                double? input_val;

                try
                {
                    input_val = Convert.ToDouble(GetInputValue(input, input.Links.First()));
                }
                catch
                {
                    input_val = null;
                }
                
                if(input_val.HasValue)
                {
                    SetValue(null!, false);
                }
                else
                {
                    if(input_val == null)
                    {
                        SetValue(null!, false);
                        return;
                    }
                    
                
                    if (_noMax && _noMin)
                    {
                        Value = input_val.Value;
                    }
                    else if( !_noMax && !_noMin)
                    {
                        Value = Math.Clamp(input_val.Value, _min, _max);
                    }
                    else if (_noMin == false) 
                    {
                        Value = Math.Max(_max, input_val.Value);
                    }
                    else if(_noMax == false)
                    {
                        Value = Math.Min(_min,input_val.Value);  
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}

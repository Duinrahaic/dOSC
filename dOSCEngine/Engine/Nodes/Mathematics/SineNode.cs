using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Utilities;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{
    public class SineNode : BaseNode
    {
        public SineNode(Guid? guid = null, ConcurrentDictionary<EntityProperty, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false, name: "Output"));

            Properties.TryInitializeProperty(EntityProperty.Amplitude, 1.0);
            Properties.TryInitializeProperty(EntityProperty.Frequency, 1.0);
            
            _amplitude = Properties.GetProperty<double>(EntityProperty.Amplitude);
            _frequency = Properties.GetProperty<double>(EntityProperty.Frequency);
            GlobalTimer.OnTimerElapsed += GetSineWave;
        }
        public override string Name => "Sine Wave";
        public override string Category => NodeCategoryType.Math;
        public override string TextIcon => "∿";
        private double _amplitude;
        private double _frequency;
        
        
        private object Generate = new();
        private void GetSineWave()
        {
            lock(Generate)
            {
                double time = DateTime.Now.TimeOfDay.TotalSeconds; // Current time in seconds
                Value = _amplitude * Math.Sin(2 * Math.PI * _frequency * time);
            }
        }

        public override void OnDispose()
        {
            GlobalTimer.OnTimerElapsed -= GetSineWave;
        }
    }
}

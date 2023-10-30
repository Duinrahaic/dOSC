using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Links
{
    public class BaseLink : LinkModel
    {
        public BaseLink(BasePort sourcePort, BasePort targetPort) : base(sourcePort, targetPort)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
            StartLabel = new LinkLabelModel(this,string.Empty);
        }
        public BaseLink(Guid guid, BasePort sourcePort, BasePort targetPort) : base(sourcePort, targetPort)
        {
            Guid = guid;
            SourcePort = sourcePort;
            TargetPort = targetPort;
            StartLabel = new LinkLabelModel(this, string.Empty);
        }




        public Guid Guid { get; set; } = Guid.NewGuid();
        public BasePort SourcePort { get; set; }
        public BasePort TargetPort { get; set; }
        public LinkLabelModel StartLabel { get; set; }
        public BaseLinkDTO GetDTO()
        {
            return new BaseLinkDTO(this);
        }

    }
}

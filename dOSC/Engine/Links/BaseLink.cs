using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Engine.Links
{
    public class BaseLink : LinkModel
    {
        public BaseLink(BasePort sourcePort, BasePort targetPort) : base(sourcePort, targetPort)
        {
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }
        public BaseLink(Guid guid, BasePort sourcePort, BasePort targetPort) : base(sourcePort, targetPort)
        {
            Guid = guid;
            SourcePort = sourcePort;
            TargetPort = targetPort;
        }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public BasePort SourcePort { get; set;}
        public BasePort TargetPort { get; set;}

        public BaseLinkDTO GetDTO()
        {
            return new BaseLinkDTO(this);
        }
        
    }
}

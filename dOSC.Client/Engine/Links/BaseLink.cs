using Blazor.Diagrams.Core.Models;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Links;

public class BaseLink : LinkModel
{
    public BaseLink(BasePort sourcePort, BasePort targetPort) : base(sourcePort, targetPort)
    {
        SourcePort = sourcePort;
        TargetPort = targetPort;
        StartLabel = new LinkLabelModel(this, string.Empty);
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
        return new BaseLinkDTO
        {
            Guid = Guid,
            SourceNode = SourcePort.ParentGuid,
            SourcePort = SourcePort.Guid,
            TargetNode = TargetPort.ParentGuid,
            TargetPort = TargetPort.Guid
        };
    }
}
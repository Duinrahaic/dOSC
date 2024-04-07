using System;

namespace dOSC.Shared.Models.Wiresheet;

public class BaseLinkDTO
{
    public BaseLinkDTO(Guid guid, Guid targetNode, Guid targetPort, Guid sourceNode, Guid sourcePort)
    {
        Guid = guid;
        TargetNode = targetNode;
        TargetPort = targetPort;
        SourceNode = sourceNode;
        SourcePort = sourcePort;
    }

    public BaseLinkDTO()
    {
    }

    public Guid Guid { get; set; }
    public Guid TargetNode { get; set; }
    public Guid TargetPort { get; set; }
    public Guid SourceNode { get; set; }
    public Guid SourcePort { get; set; }
}
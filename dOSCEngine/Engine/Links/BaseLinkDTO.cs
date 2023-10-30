namespace dOSCEngine.Engine.Links
{
    public class BaseLinkDTO
    {
        public Guid Guid { get; set; }
        public Guid TargetNode { get; set; }
        public Guid TargetPort { get; set; }
        public Guid SourceNode { get; set; }
        public Guid SourcePort { get; set; }

        public BaseLinkDTO(BaseLink link)
        {
            Guid = link.Guid;
            TargetNode = link.TargetPort.ParentGuid;
            TargetPort = link.TargetPort.Guid;
            SourceNode = link.SourcePort.ParentGuid;
            SourcePort = link.SourcePort.Guid;
        }

        public BaseLinkDTO(Guid guid, Guid targetNode, Guid targetPort, Guid sourceNode, Guid sourcePort)
        {
            Guid = guid;
            TargetNode = targetNode;
            TargetPort = targetPort;
            SourceNode = sourceNode;
            SourcePort = sourcePort;
        }
        public BaseLinkDTO() { }
    }
}

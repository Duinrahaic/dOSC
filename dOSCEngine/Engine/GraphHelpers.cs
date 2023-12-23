using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Links;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Services;

namespace dOSCEngine.Engine
{
    public static class GraphHelpers
    {
        public static Point CenterOfScreen(BlazorDiagram diagram)
        {
            if (diagram == null)
            {
                return Point.Zero;
            }
            else
            {
                var x = (diagram.Container.Width / 2 - diagram.Pan.X) / diagram.Zoom;
                var y = (diagram.Container.Height / 2 - diagram.Pan.Y) / diagram.Zoom;
                return new Point(x, y);
            }
        }
        public static Point GetCenterOfScreen(this BlazorDiagram diagram)
        {
            if (diagram == null)
            {
                return Point.Zero;
            }
            else
            {
                var x = (diagram.Container.Width / 2 - diagram.Pan.X) / diagram.Zoom;
                var y = (diagram.Container.Height / 2 - diagram.Pan.Y) / diagram.Zoom;
                return new Point(x, y);
            }
        }

        public static dOSCData DeserializeDTO(this dOSCDataDTO dto, ServiceBundle sb)
        {
            dOSCData dOSCWiresheet = new dOSCData(dto);
            var cNodes = dto.Nodes.Select(x => x.ConvertNode(sb)).Where(x => x != null);
            var cLinks = dto.Links;
            foreach (var n in cNodes)
            {
                if (n != null)
                {
                    dOSCWiresheet.AddNode(n);
                }
            }
            foreach (var l in cLinks)
            {
                if (l != null)
                {
                    var sourcePort = cNodes.FirstOrDefault(x => x.Guid == l.SourceNode)?.Ports.Select(x => x as BasePort).First(x => x.Guid == l.SourcePort);
                    var targetPort = cNodes.FirstOrDefault(x => x.Guid == l.TargetNode)?.Ports.Select(x => x as BasePort).First(x => x.Guid == l.TargetPort);

                    if (sourcePort != null && targetPort != null)
                    {
                        dOSCWiresheet.AddRelationship(sourcePort, targetPort);
                    }
                }
            }

            return dOSCWiresheet;
        }

        public static (List<BaseNode> Nodes, List<BaseLink> Links) ExtractData(this BlazorDiagram diagram )
        {
            List<BaseNode> Nodes = new List<BaseNode>();
            List<BaseLink> Links = new List<BaseLink>();
            diagram.Nodes.ToList().ForEach(node =>
            {
                var n = node as BaseNode;
                Nodes.Add(n);
            });
            diagram.Links.ToList().ForEach(link =>
            {
                var s = link.Source.Model as BasePort;
                var t = link.Target.Model as BasePort;

                if (diagram.Nodes.Any(x => (x as BaseNode)?.Guid == s!.ParentGuid) == false
                    || Nodes.Any(x => x.Guid == t!.ParentGuid) == false)
                {

                }
                else
                {
                    Links.Add(new(s, t));
                }


            });

            return (Nodes, Links);
        }


    }
}

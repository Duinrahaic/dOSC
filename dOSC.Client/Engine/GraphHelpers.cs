using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Links;
using dOSC.Client.Engine.Nodes;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using dOSCData = dOSC.Client.Services.dOSCData;
using ServiceBundle = dOSC.Client.Services.ServiceBundle;

namespace dOSC.Client.Engine;

public static class GraphHelpers
{
    public static Point CenterOfScreen(BlazorDiagram diagram)
    {
        if (diagram == null) return Point.Zero;

        var x = (diagram.Container.Width / 2 - diagram.Pan.X) / diagram.Zoom;
        var y = (diagram.Container.Height / 2 - diagram.Pan.Y) / diagram.Zoom;
        return new Point(x, y);
    }

    public static Point GetCenterOfScreen(this BlazorDiagram diagram)
    {
        if (diagram == null) return Point.Zero;

        var x = (diagram.Container.Width / 2 - diagram.Pan.X) / diagram.Zoom;
        var y = (diagram.Container.Height / 2 - diagram.Pan.Y) / diagram.Zoom;
        return new Point(x, y);
    }

    public static dynamic? GetInputValue(this BasePort port)
    {
        var link = port.GetAllBaseLinks().FirstOrDefault();
        if (link == null)
            return null;

        var sp = (link.Source as SinglePortAnchor)!;
        var tp = (link.Target as SinglePortAnchor)!;
        var p = sp.Port == port ? tp : sp;
        try
        {
            return (p.Port.Parent as BaseNode)!.Value;
        }
        catch
        {
            return null;
        }
    }

    public static List<dynamic?> GetAllInputValues(this BasePort port)
    {
        List<dynamic?> values = new();
        var links = port.GetAllBaseLinks();
        if (links.Any())
            return values;
        foreach (var link in links)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;
            var p = sp.Port == port ? tp : sp;
            try
            {
                values.Add((p.Port.Parent as BaseNode)!.Value);
            }
            catch
            {
                values.Add(null);
            }
        }

        return values;
    }

    public static dOSCData DeserializeDTO(this dOSCDataDTO dto, ServiceBundle sb)
    {
        var dOSCWiresheet = new dOSCData(dto);
        var cNodes = dto.Nodes.Select(x => x.ConvertNode(sb)).Where(x => x != null);
        var cLinks = dto.Links;
        foreach (var n in cNodes)
            if (n != null)
                dOSCWiresheet.AddNode(n);
        foreach (var l in cLinks)
            if (l != null)
            {
                var sourcePort = cNodes.FirstOrDefault(x => x.Guid == l.SourceNode)?.Ports.Select(x => x as BasePort)
                    .First(x => x.Guid == l.SourcePort);
                var targetPort = cNodes.FirstOrDefault(x => x.Guid == l.TargetNode)?.Ports.Select(x => x as BasePort)
                    .First(x => x.Guid == l.TargetPort);

                if (sourcePort != null && targetPort != null) dOSCWiresheet.AddRelationship(sourcePort, targetPort);
            }

        return dOSCWiresheet;
    }

    public static (List<BaseNode> Nodes, List<BaseLink> Links) ExtractData(this BlazorDiagram diagram)
    {
        var Nodes = new List<BaseNode>();
        var Links = new List<BaseLink>();
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
                Links.Add(new BaseLink(s, t));
            }
        });

        return (Nodes, Links);
    }
}
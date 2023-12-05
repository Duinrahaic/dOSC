using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Blocks.Connectors.Activity;
using dOSCEngine.Engine.Blocks.Connectors.OSC;
using dOSCEngine.Engine.Blocks.Connectors.VRChat;
using dOSCEngine.Engine.Blocks.Constant;
using dOSCEngine.Engine.Blocks.Logic;
using dOSCEngine.Engine.Blocks.Math;
using dOSCEngine.Engine.Blocks.Utility;
using dOSCEngine.Engine.Nodes.Connector.Activity;
using dOSCEngine.Engine.Nodes.Connector.OSC;
using dOSCEngine.Engine.Nodes.Connector.VRChat;
using dOSCEngine.Engine.Nodes.Constant;
using dOSCEngine.Engine.Nodes.Logic;
using dOSCEngine.Engine.Nodes.Math;
using dOSCEngine.Engine.Nodes.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

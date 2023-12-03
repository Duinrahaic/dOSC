using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
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


    }
}

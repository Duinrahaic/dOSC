using Blazor.Diagrams;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;

namespace dOSCEngine.Engine
{
    public class GraphUtilities
    {

        private Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>();
 
        public GraphUtilities()
        {
            adjacencyList.Clear();  
        }


        private void AddEdge(string source, string target)
        {
            if (!adjacencyList.ContainsKey(source))
            {
                adjacencyList[source] = new List<string>();
            }
            adjacencyList[source].Add(target);
        }

        private bool HasCycle()
        {
            HashSet<string> visited = new HashSet<string>();
            HashSet<string> stack = new HashSet<string>();

            foreach (var node in adjacencyList.Keys)
            {
                if (!visited.Contains(node))
                {
                    if (DFS(node, visited, stack))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool DFS(string node, HashSet<string> visited, HashSet<string> stack)
        {
            visited.Add(node);
            stack.Add(node);

            if (adjacencyList.ContainsKey(node))
            {
                foreach (var neighbor in adjacencyList[node])
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (DFS(neighbor, visited, stack))
                        {
                            return true;
                        }
                    }
                    else if (stack.Contains(neighbor))
                    {
                        return true;
                    }
                }
            }

            stack.Remove(node);
            return false;
        }

        public bool CheckForCircularLinks(BlazorDiagram diagram)
        {
            adjacencyList.Clear();
            // Get all links
            foreach (var l in diagram.Links.ToList())
            {
                var sp = (l.Source.Model as BasePort)!;
                var tp = (l.Target.Model as BasePort)!;
                if (sp != null && tp != null)
                {
                    // Get Value Node and Output Node
                    var SourceIsInput = sp!.Input ? true : false;
                    var sn = (sp.Parent as BaseNode)!;
                    var tn = (tp.Parent as BaseNode)!;

                    if(sn != null && tn != null)
                    {
                        if (SourceIsInput)
                        {
                            AddEdge(sn.Guid.ToString(), tn.Guid.ToString());
                        }
                        else
                        {
                            AddEdge(tn.Guid.ToString(), sn.Guid.ToString());
                        }
                    }
                }
            }
            return HasCycle();
        }

        
        
        
        
    }
}

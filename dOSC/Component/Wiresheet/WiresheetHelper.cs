using System.Reflection;
using dOSC.Component.Wiresheet.Nodes;
using dOSC.Component.Wiresheet.Nodes.Utility;
using dOSC.Component.Wiresheet.Widgets;

namespace dOSC.Component.Wiresheet;

public static class WiresheetHelper
{
    public static TimeSpan UpdateInterval = TimeSpan.FromMilliseconds(100);
    public static void RegisterNodes(this WiresheetDiagram diagram)
    {
        // Specific
        diagram.RegisterComponent<UtilityNodeNote, UtilityNoteWidget>();
        diagram.RegisterComponent<VariableNode, VariableNodeWidget>();
        diagram.RegisterComponent<DataNode, DataNodeWidget>();

        // Default
        diagram.RegisterComponent<WiresheetNode, DefaultNodeWidget>();
        
    }

    private const string RootNamespace = "dOSC.Component.Wiresheet.Nodes";
    private static List<Type> GetNodeNamespaces()
    {
        List<Type> nodeTypes = new();
        Assembly assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var filteredTypes = (types.Where(t => t.Namespace != null && t.Namespace.StartsWith(RootNamespace) && !t.Namespace.Equals(RootNamespace)).ToList());
        foreach (Type type in filteredTypes)
        {
            if (!type.IsAbstract)
            {
                nodeTypes.Add(type);
            }
        }
        return nodeTypes;
    }
    public static List<WiresheetNode> GetAllNodes()
    {
        List<WiresheetNode> nodes = new();  
        Type targetType = typeof(WiresheetNode);
        var acceptedNamespaces = GetNodeNamespaces();
        foreach (Type type in acceptedNamespaces)
        {
            if (type.Namespace != null && targetType.IsAssignableFrom(type) && !type.IsAbstract && !type.Namespace.Equals(RootNamespace))
            {
                if (Activator.CreateInstance(type) is WiresheetNode node)
                {
                    nodes.Add(node);
                }
            }
        }

        return nodes;
    }
    
}
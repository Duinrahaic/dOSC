using System.Reflection;
using dOSC.Component.Wiresheet.Nodes;
using dOSC.Component.Wiresheet.Nodes.Utility;
using dOSC.Component.Wiresheet.Widgets;
using LiveSheet;

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
    private static string _rootNamespace = "dOSC.Component.Wiresheet.Nodes";
    private static List<Type> GetNodeNamespaces()
    {
        List<Type> nodeTypes = new();
        Assembly assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var filteredTypes = (types.Where(t => t.Namespace.StartsWith(_rootNamespace) && !t.Namespace.Equals(_rootNamespace)).ToList() ?? new List<Type>());
        foreach (Type type in filteredTypes ?? new List<Type>())
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
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type targetType = typeof(WiresheetNode);
        var acceptedNamespaces = GetNodeNamespaces();
        foreach (Type type in acceptedNamespaces)
        {
            if (targetType.IsAssignableFrom(type) && !type.IsAbstract && !type.Namespace.Equals(_rootNamespace))
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
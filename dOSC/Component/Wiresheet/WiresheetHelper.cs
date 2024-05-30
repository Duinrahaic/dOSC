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
        // Default
        diagram.RegisterComponent<WiresheetNode, DefaultNodeWidget>();
        
        // Specific
        diagram.RegisterComponent<UtilityNodeNote, UtilityNoteWidget>();
        
    }
    
    private static List<string> NodeNameSpaces = new()
    {
        "dOSC.Component.Wiresheet.Nodes.Data",
        "dOSC.Component.Wiresheet.Nodes.Utility",
        "dOSC.Component.Wiresheet.Nodes.Variables",
        "dOSC.Component.Wiresheet.Nodes.Mathematics",
        "dOSC.Component.Wiresheet.Nodes.Logic",
    };

    public static List<WiresheetNode> GetAllNodes()
    {
        List<WiresheetNode> nodes = new();  
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type targetType = typeof(WiresheetNode);
        
        foreach (string ns in NodeNameSpaces)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace == ns && targetType.IsAssignableFrom(type))
                {
                    if (Activator.CreateInstance(type) is WiresheetNode node)
                    {
                        nodes.Add(node);
                    }
                }
            }
        }

        return nodes;
    }
    
}
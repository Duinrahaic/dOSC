using System.ComponentModel.DataAnnotations;
using Blazor.Diagrams;
using dOSC.Client.Engine;
using dOSC.Client.Engine.Links;
using dOSC.Client.Engine.Nodes;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Utilities;
using Newtonsoft.Json;

namespace dOSC.Client.Services;

public class dOSCData
{
    public List<BaseLink> _Links = new();
    public List<BaseNode> _Nodes = new();

    public dOSCData()
    {
        AppGuid = Guid.NewGuid();
        Diagram = new BlazorDiagram(GraphSettings.Options);
        Diagram.RegisterBlocks();
    }

    public dOSCData(Guid AppGuid)
    {
        Diagram = new BlazorDiagram(GraphSettings.Options);
        Diagram.RegisterBlocks();
        this.AppGuid = AppGuid;
    }

    public dOSCData(dOSCDataDTO dto)
    {
        Diagram = new BlazorDiagram(GraphSettings.Options);
        Diagram.RegisterBlocks();
        AppGuid = dto.AppGuid;
        AppName = dto.AppName;
        AppVersion = dto.AppVersion;
        AppDescription = dto.AppDescription;
        Created = dto.Created;
        Modified = dto.Modified;
        AppIcon = dto.AppIcon;
    }

    public BlazorDiagram Diagram { get; set; }


    public Guid AppGuid { get; set; } = Guid.NewGuid();

    [Required] public string AppName { get; set; } = string.Empty;

    public int AppVersion { get; set; } = 1;
    public string AppDescription { get; set; } = string.Empty;
    public string AppAuthor { get; set; } = string.Empty;
    public string PrefabLocation { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; } = DateTime.Now;
    public List<string> ConnectorsUsed { get; set; } = new();
    public string AppIcon { get; set; } = string.Empty;
    public bool Enabled => false;

    [JsonIgnore]
    public string CurrentAppIcon
    {
        get
        {
            if (!string.IsNullOrEmpty(AppIcon))
                return AppIcon;
            return AppDefaults.GetDefaultAppImage();
        }
    }

    public void AddNode(BaseNode node)
    {
        _Nodes.Add(node);
    }

    public void AddRelationship(BasePort source, BasePort target)
    {
        _Links.Add(new BaseLink(source, target));
    }

    public List<BaseNode> GetAllNodes()
    {
        return _Nodes;
    }

    public List<BaseLink> GetAllLinks()
    {
        return _Links;
    }

    public dOSCDataDTO GetDTO(bool Enabled = false, bool AutomationEnabled = false)
    {
        var node = new dOSCDataDTO
        {
            AppGuid = AppGuid,
            AppName = AppName,
            AppVersion = AppVersion,
            AppDescription = AppDescription,
            AppAuthor = AppAuthor,
            Created = Created,
            Modified = Modified,
            AppIcon = AppIcon,
            Enabled = Enabled,
            AutomationEnabled = AutomationEnabled
        };

        var nodes = _Nodes.Select(x => x.GetDTO()).ToList();
        var links = _Links.Select(x => x.GetDTO()).ToList();

        node.Nodes.AddRange(nodes);
        node.Links.AddRange(links);
        return node;
    }

    public void Sync()
    {
        var SaveData = Diagram.ExtractData();
        UpdateData(SaveData.Nodes, SaveData.Links);
    }

    private void UpdateData(List<BaseNode> nodes, List<BaseLink> links)
    {
        _Nodes.Clear();
        _Links.Clear();
        nodes.ForEach(_Nodes.Add);
        links.ForEach(_Links.Add);
    }
}
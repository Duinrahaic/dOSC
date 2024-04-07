using dOSCEngine.Engine.Links;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Services;
using System.ComponentModel.DataAnnotations;

namespace dOSCEngine.Services
{
    public class dOSCDataDTO
    {
        public List<BaseNodeDTO> Nodes { get; set; } = new();
        public List<BaseLinkDTO> Links { get; set; } = new();
        public Guid AppGuid { get; set; } = Guid.NewGuid();
        [MinLength(4)]
        [MaxLength(255)]
        public string AppName { get; set; } = string.Empty;
        public int AppVersion { get; set; } = 1;
        [MinLength(4)]
        [MaxLength(255)]
        public string AppDescription { get; set; } = string.Empty;
        [MinLength(4)]
        [MaxLength(255)]
        public string AppAuthor { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        public string AppIcon { get; set; } = string.Empty; 
        public bool Enabled { get; set; } = false;
        public bool AutomationEnabled { get; set; } = false;

        public dOSCDataDTO(dOSCData wiresheet, bool enabled = false, bool automationEnabled = false)
        {
            AppGuid = wiresheet.AppGuid;
            AppName = wiresheet.AppName;
            AppVersion = wiresheet.AppVersion;
            AppDescription = wiresheet.AppDescription;
            AppAuthor = wiresheet.AppAuthor;
            Created = wiresheet.Created;
            Modified = wiresheet.Modified;
            AppIcon = wiresheet.AppIcon;
            Enabled = enabled;
            AutomationEnabled = automationEnabled;
            Nodes.AddRange(wiresheet._Nodes.Where(x => x != null).Select(x => x.GetDTO()).ToList());
            Links.AddRange(wiresheet._Links.Where(x => x != null).Select(x => x.GetDTO()).ToList());
        }

        public dOSCDataDTO() { }
    }
}

using System.ComponentModel.DataAnnotations;

namespace dOSCEngine.Services
{
    public partial class dOSCWiresheet
    {
        public Guid AppGuid { get; set; } = Guid.NewGuid();
        [Required]
        public string AppName { get; set; } = string.Empty;
        public int AppVersion { get; set; } = 1;
        [Required]
        public string AppDescription { get; set; } = string.Empty;
        public string AppAuthor { get; set; } = string.Empty;
        public string PrefabLocation { get; set; } = string.Empty;  
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        public List<string> ConnectorsUsed { get; set; } = new();
    }
}

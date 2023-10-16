using System.ComponentModel.DataAnnotations;

namespace dOSC.Services
{
    public partial class dOSCWiresheet
    {
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
        public List<string> ConnectorsUsed { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dOSC.Shared.Models.Wiresheet;

public class dOSCDataDTO
{
    public List<BaseNodeDTO> Nodes { get; set; } = new();
    public List<BaseLinkDTO> Links { get; set; } = new();
    public Guid AppGuid { get; set; } = Guid.NewGuid();

    [MinLength(4)] [MaxLength(255)] public string AppName { get; set; } = string.Empty;

    public int AppVersion { get; set; } = 1;

    [MinLength(4)] [MaxLength(255)] public string AppDescription { get; set; } = string.Empty;

    [MinLength(4)] [MaxLength(255)] public string AppAuthor { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; } = DateTime.Now;
    public string AppIcon { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public bool AutomationEnabled { get; set; } = false;
}
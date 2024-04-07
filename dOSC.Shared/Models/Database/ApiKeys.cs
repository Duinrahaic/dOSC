using System.Collections.Generic;

namespace dOSC.Shared.Models.Database;

public class ApiKeys
{
    public int ApiId { get; set; }
    public string Alias { get; set; }
    public string Key { get; set; }
    
    public List<Endpoints> Endpoints { get; set; } = new List<Endpoints>();
    public ServiceCredentials ServiceCredentials { get; set; }
}
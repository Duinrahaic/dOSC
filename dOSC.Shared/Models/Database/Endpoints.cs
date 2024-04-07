using dOSC.Shared.Models.Endpoint;

namespace dOSC.Shared.Models.Database;

public class Endpoint
{
    public int EndpointId { get; set; }
    public int ApiId { get; set; }
    public string Address { get; set; } 
    public DataType Type { get; set; }
    public Policy Policy { get; set; } 
    public string? Facet { get; set; }
}
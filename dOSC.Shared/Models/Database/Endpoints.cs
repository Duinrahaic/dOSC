using dOSC.Shared.Models.Endpoint;

namespace dOSC.Shared.Models.Database;

public class Endpoints
{
    public int EndpointId { get; set; }
    public int ApiId { get; set; }
    public string Address { get; set; } 
    public DataType Type { get; set; }
    public Policy Policy { get; set; } 
    public Facet? Facet { get; set; }
}
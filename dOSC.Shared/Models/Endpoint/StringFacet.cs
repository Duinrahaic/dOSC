namespace dOSC.Shared.Models.Endpoint;

public class StringFacet: Facet
{
    public int MinLength { get; set; } = 0;
    public int MaxLength { get; set; } = int.MaxValue;
}
namespace dOSC.Drivers.DB.Models;

public class StringFacet: Facet
{
    public int MinLength { get; set; } = 0;
    public int MaxLength { get; set; } = int.MaxValue;
}
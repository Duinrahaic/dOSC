namespace dOSC.Shared.Models.Database;

public class StringFacet: Facet
{
    public int MinLength { get; set; } = 0;
    public int MaxLength { get; set; } = int.MaxValue;
}
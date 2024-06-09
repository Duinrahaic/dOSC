using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace dOSC.Drivers.DB.Models;

public class FacetConverter : ValueConverter<Facet, string>
{
    // Parameterless constructor
    public FacetConverter() : this(null)
    {
    }

    
    public FacetConverter(ConverterMappingHints mappingHints = null)
        : base(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<Facet>(v),
            mappingHints)
    {
    }
}
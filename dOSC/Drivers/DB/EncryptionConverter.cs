using dOSC.Shared.Utilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dOSCEngine.Services.User.DB;

public class EncryptionConverter : ValueConverter<string, string>
{
    public EncryptionConverter() : this(null)
    {
    }

    
    public EncryptionConverter(ConverterMappingHints mappingHints = null)
        : base(x => EncryptionHelper.Encrypt(x), x => EncryptionHelper.Decrypt(x), mappingHints)
    { }
}
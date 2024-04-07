using dOSC.Shared.Models.Database;
using dOSCEngine.Services.User.Models;
using Microsoft.EntityFrameworkCore;

namespace dOSCEngine.Services;

public partial class dOSCDBService
{
    public static void SaveCredentials(string name, string data, bool enabled)
    {
        var salt = CredentialHelper.GenerateSalt();
        var hashedData = CredentialHelper.HashData(data, salt);

        using (var db = new DBEntities())
        {
            var credentials = new ServiceCredentials
            {
                Name = name,
                Enabled = enabled,
                Data = $"{Convert.ToBase64String(salt)}:{hashedData}"
            };

            db.ServiceCredentials.Add(credentials);
            db.SaveChanges();
        }
    }
    
    public static void UpdateCredentials(string name, string data, bool enabled)
    {
        var salt = CredentialHelper.GenerateSalt();
        var hashedData = CredentialHelper.HashData(data, salt);

        using (var db = new DBEntities())
        {
            var credentials = db.ServiceCredentials.FirstOrDefault(x => x.Name == name);
            if (credentials == null) return;

            credentials.Enabled = enabled;
            credentials.Data = $"{Convert.ToBase64String(salt)}:{hashedData}";

            db.ServiceCredentials.Update(credentials);
            db.SaveChanges();
        }
    }
    
    public static void DeleteCredentials(string name)
    {
        using (var db = new DBEntities())
        {
            var credentials = db.ServiceCredentials.FirstOrDefault(x => x.Name == name);
            if (credentials == null) return;

            db.ServiceCredentials.Remove(credentials);
            db.SaveChanges();
        }
    }
}
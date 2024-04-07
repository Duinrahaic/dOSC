using dOSC.Drivers.DB.Models;

namespace dOSC.Drivers.DB;

public partial class dOSCDBService
{
    public dOSCDBService(DBEntities dbEntities)
    {
        using (var db = new DBEntities())
        {
            db.Database.EnsureCreated();
        }
    }
}
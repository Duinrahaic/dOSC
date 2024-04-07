using dOSCEngine.Services.User.Models;
using Microsoft.EntityFrameworkCore;

namespace dOSCEngine.Services;

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
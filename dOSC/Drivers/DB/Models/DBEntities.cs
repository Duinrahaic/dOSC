using dOSC.Shared.Models.Database;
using dOSC.Shared.Utilities;
using dOSCEngine.Services.User.DB;
using Microsoft.EntityFrameworkCore;


namespace dOSCEngine.Services.User.Models;

public class DBEntities: DbContext
{
    public virtual DbSet<Data> Data { get; set; }
    public virtual DbSet<ServiceCredentials> ServiceCredentials { get; set; }
    public virtual DbSet<ApiKeys> ApiKeys { get; set; }
    public string path { get; }
    
    public DBEntities()
    {
        path = Path.Join(dOSCFileSystem.BaseDataFolder, "dosc.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={path}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKeys>().Property(x => x.Key).HasConversion<EncryptionConverter>();
        modelBuilder.Entity<ServiceCredentials>().Property(x => x.Data).HasConversion<EncryptionConverter>();
    }
}
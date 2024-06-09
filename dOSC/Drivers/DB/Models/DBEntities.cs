using System.IO;
using dOSC.Utilities;
using Microsoft.EntityFrameworkCore;

namespace dOSC.Drivers.DB.Models;

public class DBEntities : DbContext
{
    public DBEntities()
    {
        path = Path.Join(AppFileSystem.BaseDataFolder, "dosc.db");
    }

    public virtual DbSet<Endpoints> Endpoints { get; set; }
    public virtual DbSet<ServiceCredentials> ServiceCredentials { get; set; }
    public virtual DbSet<ApiKeys> ApiKeys { get; set; }
    public string path { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={path}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKeys>().HasKey(a => a.ApiId);
        modelBuilder.Entity<ApiKeys>().Property(x => x.Key).HasConversion<EncryptionConverter>();
        modelBuilder.Entity<ServiceCredentials>().HasKey(a => a.ServiceId);
        modelBuilder.Entity<ServiceCredentials>().Property(x => x.Data).HasConversion<EncryptionConverter>();
        modelBuilder.Entity<Endpoints>().HasKey(a => a.EndpointId);
        modelBuilder.Entity<Endpoints>().Property(x => x.Facet).HasConversion<FacetConverter>();
    }
}
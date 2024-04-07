namespace dOSC.Shared.Models.Database;

public class ServiceCredentials
{
    public int ServiceId { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; } = false;
    public string? Data { get; set; }
}
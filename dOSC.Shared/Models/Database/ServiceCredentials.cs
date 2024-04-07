namespace dOSCEngine.Services.User.Models;

public class ServiceCredentials
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public bool Enabled { get; set; } = false;
    public string? Data { get; set; } 
}
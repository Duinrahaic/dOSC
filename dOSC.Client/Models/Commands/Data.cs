namespace dOSC.Client.Models.Commands;

public abstract class Data
{
    public virtual CommandType Type { get; set; } = CommandType.None;
}
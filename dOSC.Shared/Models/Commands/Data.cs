namespace dOSC.Shared.Models.Commands;

public abstract class Data
{
    public virtual CommandType Type { get; set; } = CommandType.None;
}
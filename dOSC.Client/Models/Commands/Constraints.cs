namespace dOSC.Client.Models.Commands;

public class Constraints
{
    public decimal? Max { get; set; }
    public decimal? Min { get; set; }
    public int Precision { get; set; } = 5;
}
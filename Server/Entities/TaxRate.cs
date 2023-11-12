namespace OnlineStore.Server.Entities;

public class TaxRate
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Amount { get; set; }
}
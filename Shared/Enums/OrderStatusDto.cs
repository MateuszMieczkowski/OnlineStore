using System.ComponentModel;

namespace OnlineStore.Shared.Enums;

public enum OrderStatusDto
{
    [Description("Przyjęte")]
    Created = 1,
    [Description("W realizacji")]
    Processing = 2,
    [Description("Zrealizowane")]
    Completed = 3,
    [Description("Anulowane")]
    Cancelled = 4,
}
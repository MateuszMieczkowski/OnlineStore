using System.ComponentModel;

namespace OnlineStore.Server.Enums;

public enum OrderStatus
{   [Description("Utworzone")]
    Created = 1,
    
    [Description("W trakcie realizacji")]
    Processing = 2,
    
    [Description("Zakończone")]
    Completed = 3,
    
    [Description("Anulowane")]
    Canceled = 4,
}
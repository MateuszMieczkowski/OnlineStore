using System.ComponentModel;

namespace OnlineStore.Shared.Enums;

public enum ProductStatusDto
{
    [Description("Akywny")]
    Active = 1,
    [Description("Ukryty")]
    Hidden = 2,
    [Description("W koszu")]
    Deleted = 3,
    [Description("Trwale usunięty")]
    HardDeleted = 4,
}

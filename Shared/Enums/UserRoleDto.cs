using System.ComponentModel;

namespace OnlineStore.Shared.Enums;

public enum UserRoleDto
{
    [Description("Klient")]
    User = 1,
    [Description("Administrator")]
    Admin = 2
}
using OnlineStore.Shared.Infrastructure;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Accounts;

public class ChangeUserPassword : ICommand
{
    public ChangeUserPassword(int id, string currentPassword, string newPassword, string confirmNewPassword)
    {
        Id = id;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
        ConfirmNewPassword = confirmNewPassword;
    }

    public ChangeUserPassword() { }

    
    [XmlElement(Order = 0)]
    public string ConfirmNewPassword { get; set; }
    
    [XmlElement(Order = 1)]
    public string CurrentPassword { get; set; }
    
    [XmlElement(Order = 2)]
    public int Id { get; set; }

    [XmlElement(Order = 3)]
    public string NewPassword { get; set; }

}
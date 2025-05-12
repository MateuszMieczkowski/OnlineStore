using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Accounts;

public class RegisterAdmin : IRegisterUserCommand
{
    
    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [Compare("Password", ErrorMessage = "Oba hasła powinny być takie same")]
    [XmlElement(Order = 0)]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "E-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Wprowadzono niepoprawny adres e-mail")]
    [XmlElement(Order = 1)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [XmlElement(Order = 2)]
    public string Password { get; set; }
}

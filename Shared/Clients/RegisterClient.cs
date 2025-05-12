using OnlineStore.Shared.Accounts;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Shared.Clients;

public class RegisterClient : IRegisterUserCommand
{
    [Required(ErrorMessage = "E-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Wprowadzono niepoprawny adres e-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [Compare("Password", ErrorMessage = "Oba hasła powinny być takie same")]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "Imię jest wymagane")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [Phone(ErrorMessage = "Wprowadzono niepoprawny numer telefonu")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Deklaracja zapisania na newsletter jest wymagana")]
    public bool IsSubscribedToNewsletter { get; set; }
}

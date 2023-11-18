using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Shared.Accounts;

public class RegisterUser : IRequest
{
    [Required(ErrorMessage = "E-mail jest wymagany")]
    [EmailAddress(ErrorMessage = "Wprowadzono niepoprawny adres e-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [Compare("Password", ErrorMessage = "Oba hasła powinny być takie same")]
    public string ConfirmPassword { get; set; }
}

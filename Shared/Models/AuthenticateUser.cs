using MediatR;
using OnlineStore.Shared.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Shared.Models;

public class AuthenticateUser : IQuery<AuthResponse>
{
    [Required (ErrorMessage = "E-mail jest wymagany")][EmailAddress (ErrorMessage = "Podano nieprawidłowy adres e-mail")] public string Email { get; set; }

    [Required (ErrorMessage = "Hasło jest wymagane")] public string Password { get; set; }
}
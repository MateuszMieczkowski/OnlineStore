using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Shared.Models;

public class AuthenticateUser : IRequest<AuthResponse>
{
    [Required][EmailAddress] public string Email { get; set; }

    [Required] public string Password { get; set; }
}
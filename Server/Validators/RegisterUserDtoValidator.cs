using FluentValidation;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterAdmin>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();
        RuleFor(x => x.Password)
            .MinimumLength(6);

        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);
    }
}
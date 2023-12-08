using FluentValidation;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Features.Accounts.RegisterUser;

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
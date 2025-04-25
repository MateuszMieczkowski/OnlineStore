using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Features.Accounts.Exceptions;
using OnlineStore.Server.Features.Accounts.Strategies;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Features.Accounts.Services;

public interface IAccountService
{
    Task RegisterUser<TRegisterCommand>(
        TRegisterCommand command,
        IUserFactory<TRegisterCommand> factory,
        CancellationToken token = default)
        where TRegisterCommand : IRegisterUserCommand;
    
    Task ChangePassword(User user, string password);

    void AssertHashedPassword(User user, string currentPassword);
}

public class AccountService : IAccountService
{
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly OnlineStoreDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(
        OnlineStoreDbContext context,
        IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }

    public async Task RegisterUser<TRegisterCommand>(
        TRegisterCommand command,
        IUserFactory<TRegisterCommand> factory,
        CancellationToken token = default)
        where TRegisterCommand : IRegisterUserCommand
    {
        await AssertEmail(command.Email);

        var user = factory.Create(command);
        var hashPassword = _passwordHasher.HashPassword(user, command.Password);

        user.UpdatePassword(hashPassword);

        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task ChangePassword(User user, string password)
    {
        var newPasswordHashed = _passwordHasher.HashPassword(user, password);

        user.PasswordHash = newPasswordHashed; 
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public void AssertHashedPassword(User user, string currentPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, currentPassword);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidCurrentPasswordException();
        }
    }

    private async Task AssertEmail(string email)
    {
        var isEmailDuplicated = await _context.Users.AnyAsync(x => x.Email == email);

        if (isEmailDuplicated)
        {
            throw new DuplicateException($"E-mail {email} już istnieje w bazie użytkowników. Spróbuj się zalogować");
        }
    }
}

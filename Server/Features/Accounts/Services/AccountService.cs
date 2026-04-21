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

    Task GeneratePartialPasswords(User user, string fullPassword);

    Task<bool> VerifyPartialPassword(int userId, int partialPasswordId, string userInput);
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

        // Generate partial passwords if the password meets length requirements
        if (command.Password.Length >= 12 && command.Password.Length <= 18)
        {
            await GeneratePartialPasswords(user, command.Password);
        }

        await _context.SaveChangesAsync(token);
    }
    
    public async Task ChangePassword(User user, string password)
    {
        var newPasswordHashed = _passwordHasher.HashPassword(user, password);

        user.PasswordHash = newPasswordHashed; 
        _context.Users.Update(user);

        // Regenerate partial passwords
        if (password.Length >= 12 && password.Length <= 18)
        {
            await GeneratePartialPasswords(user, password);
        }

        await _context.SaveChangesAsync();
    }
    
    public void AssertHashedPassword(User user, string currentPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, currentPassword);
        if (result != PasswordVerificationResult.Success)
        {
            throw new InvalidCredentialsException();
        }
    }

    public async Task GeneratePartialPasswords(User user, string fullPassword)
    {
        const int MinPasswordLength = 12;
        const int MaxPasswordLength = 18;
        const int MinFragmentLength = 6;
        const int MinPartialCount = 10;

        var passwordLength = fullPassword.Length;
        if (passwordLength < MinPasswordLength || passwordLength > MaxPasswordLength)
            throw new ArgumentException($"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");

        int maxFragmentLength = Math.Max(passwordLength / 2, MinFragmentLength);

        // Remove old partial passwords
        var existing = _context.PartialPasswords.Where(p => p.UserId == user.Id);
        _context.PartialPasswords.RemoveRange(existing);

        var random = new Random();
        var partials = new List<PartialPassword>();

        // Generate at least MinPartialCount unique (start, length) combinations
        var generated = new HashSet<(int, int)>();
        int attempts = 0;
        while (partials.Count < MinPartialCount && attempts < 1000)
        {
            attempts++;
            int fragLen = random.Next(MinFragmentLength, maxFragmentLength + 1);
            if (fragLen > passwordLength) fragLen = passwordLength;
            int maxStart = passwordLength - fragLen;
            int start = random.Next(0, maxStart + 1);
            var key = (start, fragLen);
            if (generated.Contains(key)) continue;
            generated.Add(key);

            var fragment = fullPassword.Substring(start, fragLen);
            var hashedFragment = _passwordHasher.HashPassword(user, fragment);

            partials.Add(new PartialPassword
            {
                UserId = user.Id,
                StartPosition = start,
                Length = fragLen,
                Fragment = hashedFragment
            });
        }

        await _context.PartialPasswords.AddRangeAsync(partials);
    }

    public async Task<bool> VerifyPartialPassword(int userId, int partialPasswordId, string userInput)
    {
        var partial = await _context.PartialPasswords
            .FirstOrDefaultAsync(p => p.Id == partialPasswordId && p.UserId == userId);
        if (partial == null) return false;

        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(user, partial.Fragment, userInput);
        return result == PasswordVerificationResult.Success;
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

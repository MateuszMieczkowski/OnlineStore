using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Services;

public interface IAccountService
{
    Task RegisterUser(RegisterUser command, CancellationToken token = default);

    AuthResponse Login(AuthenticateUser dto);
}

public class AccountService : IAccountService
{
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly OnlineStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(
        OnlineStoreDbContext context,
        IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings,
        IMapper mapper)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _mapper = mapper;
    }

    public async Task RegisterUser(RegisterUser command, CancellationToken token = default)
    {
        await AssertEmail(command.Email);
        
        var newUser = new User
        {
            Email = command.Email,
            UserRole = UserRole.User,
        };
        var hashPassword = _passwordHasher.HashPassword(newUser, command.Password);

        newUser.UpdatePassword(hashPassword);
        
        await _context.Users.AddAsync(newUser, token);
        await _context.SaveChangesAsync(token);
    }

    public AuthResponse Login(AuthenticateUser dto)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
        if (user is null) throw new BadRequestException("Niepoprawny adres e-mail lub hasło.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Niepoprawny adres e-mail lub hasło.");

        var authResponse = _mapper.Map<AuthResponse>(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.UserRole.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        authResponse.Token = tokenHandler.WriteToken(token);

        return authResponse;
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

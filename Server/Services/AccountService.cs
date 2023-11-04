﻿using System.IdentityModel.Tokens.Jwt;
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
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Services;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
    AuthResponse Login(LoginDto dto);
}

public class AccountService : IAccountService
{
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly OnlineStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(OnlineStoreDbContext context, IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings, IMapper mapper)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _mapper = mapper;
    }

    public void RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User
        {
            Email = dto.Login,
            UserRole = UserRole.User
        };
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);
        _context.Users.Add(newUser);
        _context.SaveChanges();
    }


    public AuthResponse Login(LoginDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == dto.Login);
        if (user is null) throw new BadRequestException("Invalid username or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Invalid username or password");

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

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        authResponse.Token = tokenHandler.WriteToken(token);

        return authResponse;
    }
}
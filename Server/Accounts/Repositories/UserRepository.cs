using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Accounts.Repositories;

public interface IUserRepository
{
    Task<User> GetById(int id);
    
    Task<Client?> FindClientByEmail(string email);
    
    Task<User?> FindUserByEmail(string email);
}

public class UserRepository : IUserRepository
{
    private readonly OnlineStoreDbContext _dbContext;
    
    public UserRepository(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<User> GetById(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
        {
            throw new NotFoundException($"Nie znaleziono użytkownika o ID {id}");
        }

        return user;
    }

    public async Task<Client?> FindClientByEmail(string email)
    {
        return await _dbContext.Clients.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}

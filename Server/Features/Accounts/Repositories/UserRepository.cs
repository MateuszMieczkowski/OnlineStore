using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Features.Accounts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<Client?> FindClientByEmail(string email);

    Task<User?> FindUserByEmail(string email);
}

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(OnlineStoreDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<Client?> FindClientByEmail(string email)
    {
        return await _dbContext.Set<Client>().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
    }
}
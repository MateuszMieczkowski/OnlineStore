using System.Linq.Expressions;

namespace OnlineStore.Server.Infrastructure;

using Microsoft.EntityFrameworkCore;


public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly DbContext _dbContext;

    protected RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entity;
    }
    
    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var entitiesArr = entities as T[] ?? entities.ToArray();
        _dbContext.Set<T>().AddRange(entitiesArr);

        await SaveChangesAsync(cancellationToken);

        return entitiesArr;
    }
    
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);

        await SaveChangesAsync(cancellationToken);
    }


    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().UpdateRange(entities);

        await SaveChangesAsync(cancellationToken);
    }


    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);

        await SaveChangesAsync(cancellationToken);
    }
    
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);

        await SaveChangesAsync(cancellationToken);
    }
    
    public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
       return await _dbContext.SaveChangesAsync(cancellationToken);
    }


    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }
    
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }
    
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(cancellationToken);
    }
    
    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
    }
    
}
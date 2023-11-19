using Microsoft.EntityFrameworkCore;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Server.Infrastructure;

public interface IResultPaginator
{
    Task<PagedResult<TDto>> GetPagedResult<TEntity, TDto>(
        IQueryable<TEntity> queryBase,
        IPagedQuery<TDto> query,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TDto : class;
}

public class ResultPaginator : IResultPaginator
{
    public async Task<PagedResult<TDto>> GetPagedResult<TEntity, TDto>(
        IQueryable<TEntity> queryBase,
        IPagedQuery<TDto> query,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TDto : class
    {
        var (pageNumber, pageSize) = (query.PageNumber, query.PageSize);
        var dbQuery = queryBase.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var items = await dbQuery.ToListAsync(cancellationToken);

        var totalCount = await queryBase.CountAsync(cancellationToken);
        var totalPages = (totalCount + pageSize - 1) / pageSize;

        return new PagedResult<TDto>(items.Select(mapper).ToList(), pageNumber, pageSize, totalPages, totalCount);
    }
}

using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Server.Features.Accounts.GetUserList;

public class GetUserListQueryHandler : IQueryHandler<Shared.Accounts.GetUserList, PagedResult<UserDto>>
{
    private readonly IResultPaginator _resultPaginator;
    private readonly OnlineStoreDbContext _dbContext;
    
    public GetUserListQueryHandler(IResultPaginator resultPaginator, OnlineStoreDbContext dbContext)
    {
        _resultPaginator = resultPaginator ?? throw new ArgumentNullException(nameof(resultPaginator));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    }

    public async Task<PagedResult<UserDto>> Handle(Shared.Accounts.GetUserList query, CancellationToken cancellationToken)
    {
        var dbQueryBase = _dbContext.Users.AsQueryable();
        var result = await _resultPaginator.GetPagedResult(dbQueryBase, query, x => x.ToDto(), cancellationToken);
        return result;
    }
}

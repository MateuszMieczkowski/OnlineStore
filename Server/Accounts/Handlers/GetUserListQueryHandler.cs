using MediatR;
using OnlineStore.Server.Accounts.Mapping;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Server.Accounts.Handlers;

public class GetUserListQueryHandler : IQueryHandler<GetUserList, PagedResult<UserDto>>
{
    private readonly IResultPaginator _resultPaginator;
    private readonly OnlineStoreDbContext _dbContext;
    
    public GetUserListQueryHandler(IResultPaginator resultPaginator, OnlineStoreDbContext dbContext)
    {
        _resultPaginator = resultPaginator ?? throw new ArgumentNullException(nameof(resultPaginator));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    }

    public async Task<PagedResult<UserDto>> Handle(GetUserList query, CancellationToken cancellationToken)
    {
        var dbQueryBase = _dbContext.Users.Where(x => x.UserRole == UserRole.Admin).AsQueryable();
        var result = await _resultPaginator.GetPagedResult(dbQueryBase, query, x => x.ToDto(), cancellationToken);
        return result;
    }
}

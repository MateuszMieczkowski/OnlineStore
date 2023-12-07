using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Services;
using OnlineStore.Server.Clients.Mappings;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Clients.Handlers;

public class GetOrderAddressCommandHandler : IQueryHandler<GetOrderAddress, OrderAddressDto?>
{
    private readonly ILoggedUserService _loggedUserService;
    private readonly OnlineStoreDbContext _dbContext;

    public GetOrderAddressCommandHandler(ILoggedUserService loggedUserService, OnlineStoreDbContext dbContext)
    {
        _loggedUserService = loggedUserService ?? throw new ArgumentNullException(nameof(loggedUserService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<OrderAddressDto?> Handle(GetOrderAddress query, CancellationToken cancellationToken)
    {
        var userId = _loggedUserService.GetUserId();
        var address = await _dbContext.OrdersAddresses.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);

        var addressDto = address?.ToDto();
        return addressDto;
    }
    
}

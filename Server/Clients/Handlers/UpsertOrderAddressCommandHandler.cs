using OnlineStore.Server.Clients.Mappings;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Clients.Handlers;

public class UpsertOrderAddressCommandHandler : ICommandHandler<UpsertOrderAddress>
{
    private readonly ILoggedUserService _loggedUserService;
    private readonly OnlineStoreDbContext _dbContext;

    public UpsertOrderAddressCommandHandler(ILoggedUserService loggedUserService, OnlineStoreDbContext dbContext)
    {
        _loggedUserService = loggedUserService;
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Handle(UpsertOrderAddress command, CancellationToken cancellationToken)
    {
        var userId = _loggedUserService.GetUserId();
        var address = command.ToEntity(userId);
        var shouldInsert = command.Id == null;
        
        if (shouldInsert)
        {
            _dbContext.OrdersAddresses.Add(address);
        }
        else
        {
            _dbContext.OrdersAddresses.Update(address);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
}

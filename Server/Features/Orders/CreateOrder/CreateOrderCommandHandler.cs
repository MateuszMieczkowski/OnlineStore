using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Features.Orders.UpdateOrderState;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Email;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<Shared.Orders.CreateOrder>
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILoggedUserService _loggedUserService;
    private readonly IEmailService _emailService;
	public CreateOrderCommandHandler(
		OnlineStoreDbContext dbContext,
		ILoggedUserService loggedUserService,
		IEmailService emailService)
	{
		_dbContext = dbContext;
		_loggedUserService = loggedUserService;
		_emailService = emailService;
	}

	public async Task Handle(Shared.Orders.CreateOrder request, CancellationToken cancellationToken)
    {
        var userId = _loggedUserService.GetUserId();
        var client = await _dbContext.Users
            .FirstAsync(x => x.Id == userId, cancellationToken);

        var requestProductIds = request.Items.Select(x => x.ProductId);

        var products = await _dbContext.Products
            .Where(x => requestProductIds.Contains(x.Id)
                && !x.IsDeleted
                && !x.IsHidden)
            .ToListAsync(cancellationToken);
        if (!products.Any())
        {
            throw new NotFoundException("Nie znaleziono produktów");
        }
        var orderAddress = await _dbContext.OrdersAddresses
            .FirstOrDefaultAsync(x => x.Id == request.OrderAddressId, cancellationToken)
            ?? throw new NotFoundException($"Nie znaleziono adresu");

        var orderItems = new List<OrderItem>();
        decimal totalPriceNet = 0;
        decimal totalPriceGross = 0;
        foreach (var product in products)
        {
            var requestItem = request.Items.First(x => x.ProductId == product.Id);
            if(product.Quantity < requestItem.Count)
            {
                throw new BadRequestException($"Brak wystarczającej ilości produktu {product.Name}");
            }
            var orderItem = new OrderItem
            {
                Product = new OrderItemProduct(product),
                Quantity = requestItem.Count,
                PriceNet = product.PriceGross * requestItem.Count,
                PriceGross = product.PriceNet * requestItem.Count,
            };
            orderItems.Add(orderItem);
            
            product.Quantity -= requestItem.Count;
            totalPriceNet += orderItem.PriceNet;
            totalPriceGross += orderItem.PriceGross;
        }

        var order = new Order
        {
            ClientId = userId,
            User = client,
            Address = orderAddress,
            OrderAddressId = orderAddress.Id,
            OrderItems = orderItems,
            TotalGross = totalPriceGross,
            TotalNet = totalPriceNet
        };

        _dbContext.Orders.Add(order);
        var orderContext = new OrderContext(order, new OrderCreatedState(_emailService));
        await orderContext.CreateAsync();
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        request.CreatedId = order.Id;
    }
}

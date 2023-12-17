using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record ProcessOrder(int OrderId) : ICommand;
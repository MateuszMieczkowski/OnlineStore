using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record CompleteOrder(int OrderId) : ICommand;
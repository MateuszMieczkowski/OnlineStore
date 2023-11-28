using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record CancelOrder(int OrderId) : ICommand;
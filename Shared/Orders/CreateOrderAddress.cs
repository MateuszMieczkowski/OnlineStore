using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record CreateOrderAddress(string StreetNumber, string City, string? State, string PostalCode, string Country) : ICommand;
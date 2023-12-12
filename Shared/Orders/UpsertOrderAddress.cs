using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record UpsertOrderAddress(int? Id, string Street, string StreetNumber, string City, string? State, string PostalCode, string Country) : ICommand;
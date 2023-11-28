namespace OnlineStore.Shared.Orders;

public record OrderAddressDto(int Id, string StreetNumber, string City, string? State, string PostalCode, string Country);

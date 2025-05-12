using OnlineStore.Server.Entities;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Clients.Mappings;

public static class OrderAddressMapper
{
    public static OrderAddress ToEntity(this UpsertOrderAddress command, int? userId)
        => new()
        {
            Id = command.Id ?? 0,
            City = command.City,
            Country = command.Country,
            PostalCode = command.PostalCode,
            State = command.State,
            Street = command.Street,
            StreetNumber = command.StreetNumber,
            UserId =  userId
        };
    
    public static OrderAddressDto ToDto(this OrderAddress address)
        => new(
            Id: address.Id,
            City: address.City,
            Country: address.Country,
            PostalCode: address.PostalCode,
            State: address.State,
            StreetNumber: address.StreetNumber,
            Street: address.Street);
}

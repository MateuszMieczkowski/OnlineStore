using OnlineStore.Shared.Infrastructure;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Orders;

public class UpsertOrderAddress : ICommand
{
    public UpsertOrderAddress(int? id, string street, string streetNumber, string city, string? state, string postalCode, string country)
    {
        Id = id;
        Street = street;
        StreetNumber = streetNumber;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }

    public UpsertOrderAddress() { }
    
    [XmlElement(Order = 0)]
    public string City { get; set; }

    [XmlElement(Order = 1)]
    public string Country { get; set; }

    [XmlElement(Order = 2)]
    public int? Id { get; set; }

    [XmlElement(Order = 3)]
    public string PostalCode { get; set; }

    [XmlElement(Order = 4)]
    public string? State { get; set; }

    [XmlElement(Order = 5)]
    public string Street { get; set; }

    [XmlElement(Order = 6)]
    public string StreetNumber { get; set; }
}
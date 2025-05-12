using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Orders;

[DataContract]
[XmlRoot("GetOrderAddressResult")]
public record OrderAddressDto
{
    public OrderAddressDto(int Id, string Street, string StreetNumber, string City, string? State, string PostalCode, string Country)
    {
        this.Id = Id;
        this.Street = Street;
        this.StreetNumber = StreetNumber;
        this.City = City;
        this.State = State;
        this.PostalCode = PostalCode;
        this.Country = Country;
    }

    public OrderAddressDto() { }
    
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Street { get; set; }

    [DataMember]
    public string StreetNumber { get; set; }

    [DataMember]
    public string City { get; set; }

    [DataMember]
    public string? State { get; set; }

    [DataMember]
    public string PostalCode { get; set; }

    [DataMember]
    public string Country { get; set; }
}

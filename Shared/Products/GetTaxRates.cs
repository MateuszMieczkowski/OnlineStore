using OnlineStore.Shared.Infrastructure;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Products;

public record GetTaxRates : IQuery<IEnumerable<TaxRateDto>>;

public class TaxRateDto
{
    public TaxRateDto()
    {
        
    }
    
    public TaxRateDto(int taxRateId, int amount, string description)
    {
        TaxRateId = taxRateId;
        Amount = amount;
        Description = description;
    }
    public override string ToString()
        => Description;

    [DataMember]
    public int TaxRateId { get; set; }

    [DataMember]
    public int Amount { get; set; }

    [DataMember]
    public string Description { get; set; }
    
};

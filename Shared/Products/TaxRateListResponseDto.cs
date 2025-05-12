using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Products;

[DataContract]
[XmlRoot("GetTaxRatesResult")]
public class TaxRateListResponseDto
{
    [DataMember]
    public List<TaxRateDto> TaxRates { get; set; }

    public TaxRateListResponseDto(List<TaxRateDto> taxRates)
    {
        TaxRates = taxRates;
    }

    public TaxRateListResponseDto()
    {
        
    }
}

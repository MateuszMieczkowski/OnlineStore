using System.ComponentModel;

namespace OnlineStore.Shared.Enums;

public enum TaxRateDto
{
    [Description("VAT 0%")] Vat0 = 0,
    [Description("VAT ZW")] VatZw = 1,
    [Description("VAT 5%")] Vat5 = 5,
    [Description("VAT 8%")] Vat8 = 8,
    [Description("VAT 23%")] Vat23 = 23,
}

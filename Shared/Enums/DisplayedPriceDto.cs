using System.ComponentModel;

namespace OnlineStore.Shared.Enums
{
    public enum DisplayedPriceDto
    {
        [Description("Brutto")]
        Gross = 1,
        [Description("Netto")]
        Net = 2
    }
}

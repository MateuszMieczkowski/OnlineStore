namespace OnlineStore.Server.Features.Products.Services;

public interface ITaxService
{
    decimal ApplyTax(decimal priceNet, int taxPercentage);
    decimal RemoveTax(decimal priceGross, int taxPercentage);
}

public class TaxService : ITaxService
{
    public decimal ApplyTax(decimal priceNet, int taxPercentage)
    {
        if (taxPercentage == 0)
        {
            return priceNet;
        }

        var taxAmount = priceNet * (taxPercentage / 100M);
        var priceGross = priceNet + taxAmount;
        
        return priceGross;
    }

    public decimal RemoveTax(decimal priceGross, int taxPercentage)
    {
        var taxAmount = priceGross * (taxPercentage / 100M);
        return priceGross - taxAmount;
    }
}
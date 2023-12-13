namespace OnlineStore.Client.Products.Models;

public class ProductFilterModel
{
    public string? SearchPhrase { get; set; }
    public string? Name { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ShortDescription { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool HiddenOnly { get; set; }
    
    public bool DeletedOnly { get; set; }
    public bool FilterGrossPrice { get; set; }

    public void Clear()
    {
        SearchPhrase = null;
        Name = null;
        ReferenceNumber = null;
        ShortDescription = null;
        MinPrice = null;
        MaxPrice = null;
        HiddenOnly = false;
    }
}
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public class HideProduct : ICommand
{
    public HideProduct(int id)
    {
        Id = id;
    }

    public HideProduct()
    {
        
    }

    public int Id { get; set; }
}
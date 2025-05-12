using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public class RevealProduct : ICommand
{
    public RevealProduct(int id)
    {
        Id = id;
    }

    public RevealProduct()
    {
        
    }

    public int Id { get; set; }
}
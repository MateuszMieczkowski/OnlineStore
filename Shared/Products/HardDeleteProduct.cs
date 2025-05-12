using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public class HardDeleteProduct : ICommand
{
    public HardDeleteProduct(int id)
    {
        Id = id;
    }

    public HardDeleteProduct()
    {
        
    }

    public int Id { get; set; }
}
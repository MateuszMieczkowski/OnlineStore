using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public class SoftDeleteProduct : ICommand
{
    public SoftDeleteProduct(int Id)
    {
        this.Id = Id;
    }

    public SoftDeleteProduct()
    {
        
    }
    
    public int Id { get; set; }
}
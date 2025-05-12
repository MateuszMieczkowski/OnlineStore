using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public class RecoverProduct : ICommand
{
    public RecoverProduct(int Id)
    {
        this.Id = Id;
    }

    public RecoverProduct()
    {
        
    }
    
    public int Id { get; set; }
}
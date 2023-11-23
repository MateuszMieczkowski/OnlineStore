using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record HardDeleteProduct(int Id) : ICommand;
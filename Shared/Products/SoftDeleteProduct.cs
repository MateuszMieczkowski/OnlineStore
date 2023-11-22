using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record SoftDeleteProduct(int Id) : ICommand;
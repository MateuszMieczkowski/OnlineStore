using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record HideProduct(int Id) : ICommand;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record HardDeleteProducts(int[] Ids, bool DeleteAll) : ICommand;

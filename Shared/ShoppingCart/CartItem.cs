namespace OnlineStore.Shared.ShoppingCart;

public record ShoppingCartDto(IReadOnlyCollection<ShoppingCartItemDto> Items);

public record ShoppingCartItemDto(int ProductId, string Name, string ThumbnailUri, int Count);


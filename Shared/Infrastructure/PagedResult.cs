namespace OnlineStore.Shared.Infrastructure;

public record PagedResult<TDto>(IReadOnlyCollection<TDto> Items, int PageNumber, int PageSize, int TotalPages, int TotalItemsCount)
    where TDto : class
{
}

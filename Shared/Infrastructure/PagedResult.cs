using System.Runtime.Serialization;

namespace OnlineStore.Shared.Infrastructure;

[DataContract]
public class PagedResult<TDto>(IReadOnlyCollection<TDto> items, int pageNumber, int pageSize, int totalPages, int totalItemsCount)
    where TDto : class
{
    // [DataMember]
    public IReadOnlyCollection<TDto> Items { get; init; } = items;

    [DataMember]
    public int PageNumber { get; init; } = pageNumber;

    [DataMember]
    public int PageSize { get; init; } = pageSize;

    [DataMember]
    public int TotalPages { get; init; } = totalPages;

    [DataMember]
    public int TotalItemsCount { get; init; } = totalItemsCount;
}

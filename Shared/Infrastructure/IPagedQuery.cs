namespace OnlineStore.Shared.Infrastructure;

public interface IPagedQuery<TDto> : IQuery<PagedResult<TDto>> where TDto : class
{
    public int PageNumber { get; }
    
    public int PageSize { get; }
}


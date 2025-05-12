using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public record GetUserList : IPagedQuery<UserDto>
{
    public GetUserList(int PageNumber, int PageSize)
    {
        this.PageNumber = PageNumber;
        this.PageSize = PageSize;
    }

    public GetUserList() { }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}
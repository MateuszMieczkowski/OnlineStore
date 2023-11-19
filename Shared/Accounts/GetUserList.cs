using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public record GetUserList(int PageNumber, int PageSize) : IPagedQuery<UserDto>;
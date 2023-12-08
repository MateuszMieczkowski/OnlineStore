using OnlineStore.Server.Entities;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Features.Accounts.GetUserList;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(user.Id, user.Email);
    }
}

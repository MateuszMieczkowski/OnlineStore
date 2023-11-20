using OnlineStore.Server.Entities;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Mapping;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(user.Id, user.Email);
    }
}

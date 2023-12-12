using OnlineStore.Server.Entities;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Enums;

namespace OnlineStore.Server.Features.Accounts.GetUserList;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(user.Id, user.Email, (UserRoleDto)user.UserRole);
    }
}

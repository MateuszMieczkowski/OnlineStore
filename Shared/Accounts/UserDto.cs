using OnlineStore.Shared.Enums;

namespace OnlineStore.Shared.Accounts;

public record UserDto(int Id, string Email, UserRoleDto Role);

using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<AuthResponse> LoginAsync(AuthenticateUser authenticateUser);
    Task RegisterAsync(RegisterAdmin register);
    
    Task<PagedResult<UserDto>> GetUserList(int pageNumber, int pageSize);

    Task ChangeUserPassword(ChangeUserPassword command);
    
    Task ForgotUserPassword(ForgotPassword command);

    Task ResetUserPassword(ResetPassword command);
}
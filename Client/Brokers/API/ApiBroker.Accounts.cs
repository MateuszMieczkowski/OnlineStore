using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string AccountRelativeUrl = "api/account";

    public async Task<AuthResponse> LoginAsync(AuthenticateUser authenticateUser)
    {
        var response = await PostAsync<AuthenticateUser, AuthResponse>(AccountRelativeUrl + "/login", authenticateUser);
        return response;
    }

    public async Task RegisterAsync(RegisterAdmin register)
    {
        await PostAsync(AccountRelativeUrl + "/register", register);
    }

    public async Task<PagedResult<UserDto>> GetUserList(int pageNumber, int pageSize)
    {
        var requestUrl = $"{AccountRelativeUrl}?pageNumber={pageNumber}&pageSize={pageSize}";
        return await GetAsync<PagedResult<UserDto>>(requestUrl);
    }
    
    public async Task ChangeUserPassword(ChangeUserPassword command)
    {
        await PutAsync($"{AccountRelativeUrl}/change-password", command);
    }

    public async Task ForgotUserPassword(ForgotPassword command)
    {
        await PostAsync($"{AccountRelativeUrl}/forgot-password", command);
    }
    
    public async Task ResetUserPassword(ResetPassword command)
    {
        await PostAsync($"{AccountRelativeUrl}/reset-password", command);
    }
}
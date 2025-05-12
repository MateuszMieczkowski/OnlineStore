using MediatR;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.SoapContracts;

namespace OnlineStore.Server.SoapServices;

public class AccountSoapService(IMediator mediator) : IAccountSoapService
{
    public async Task<UserListPagedResponseDto> GetUsers(GetUserList query)
    {
        var result = await mediator.Send(query);
        return new UserListPagedResponseDto(result.Items.ToList(), result.PageNumber, result.PageSize, result.TotalPages, result.TotalItemsCount);
    }

    public Task RegisterUser(RegisterAdmin command)
        => mediator.Send(command);

    public Task<AuthResponse> Login(AuthenticateUser query)
        => mediator.Send(query);

    public Task ChangePassword(ChangeUserPassword command)
        => mediator.Send(command);

    public Task ForgotPassword(ForgotPassword command)
        => mediator.Send(command);

    public Task ResetPassword(ResetPassword command)
        => mediator.Send(command);
}

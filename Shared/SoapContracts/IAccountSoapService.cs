using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using System.ServiceModel;

namespace OnlineStore.Shared.SoapContracts;

[ServiceContract]
public interface IAccountSoapService
{
    [OperationContract]
    Task<UserListPagedResponseDto> GetUsers(GetUserList query);

    [OperationContract]
    Task RegisterUser(RegisterAdmin command);
    
    [OperationContract]
    Task<AuthResponse> Login(AuthenticateUser query);

    [OperationContract]
    Task ChangePassword(ChangeUserPassword command);

    [OperationContract]
    Task ForgotPassword(ForgotPassword command);

    [OperationContract]
    Task ResetPassword(ResetPassword command);
}

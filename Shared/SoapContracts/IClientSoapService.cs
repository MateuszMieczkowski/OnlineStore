using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;
using System.ServiceModel;

namespace OnlineStore.Shared.SoapContracts;

[ServiceContract]
public interface IClientSoapService
{
    [OperationContract]
    Task RegisterUser(RegisterClient command);

    [OperationContract]
    Task ChangePassword(ChangeUserPreferences command);

    [OperationContract]
    Task<OrderAddressDto?> GetOrderAddress();

    [OperationContract]
    Task UpsertOrderAddress(UpsertOrderAddress command);
}

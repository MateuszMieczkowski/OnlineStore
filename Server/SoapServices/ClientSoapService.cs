using MediatR;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;
using OnlineStore.Shared.SoapContracts;

namespace OnlineStore.Server.SoapServices;

public class ClientSoapService(IMediator mediator) : IClientSoapService
{
    public Task RegisterUser(RegisterClient command)
        => mediator.Send(command);

    public Task ChangePassword(ChangeUserPreferences command)
        => mediator.Send(command);

    public Task<OrderAddressDto?> GetOrderAddress()
        => mediator.Send(new GetOrderAddress());

    public Task UpsertOrderAddress(UpsertOrderAddress command)
        => mediator.Send(command);
}

using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;
using System.ServiceModel;

namespace OnlineStore.Shared.SoapContracts;

[ServiceContract]
public interface IOrderSoapService
{
    [OperationContract]
    Task<int> CreateOrder(CreateOrder command);

    [OperationContract]
    Task<GetAllOrdersResponseDto> GetOrders(GetOrders query);

    [OperationContract]
    Task<OrderDto> GetOrder(GetOrder query);

    [OperationContract]
    Task CompleteOrder(CompleteOrder command);

    [OperationContract]
    Task CancelOrder(CancelOrder command);
    
    [OperationContract]
    Task ProcessOrder(ProcessOrder command);
}

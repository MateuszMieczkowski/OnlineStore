using OnlineStore.Client.Models.Orders;
using OnlineStore.Client.Services;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.SoapServices;

public class OrderSoapService(ISoapClient soapClient) : IOrderService
{
    private const string Endpoint = "/soap/order";
    private const string ServiceNamespace = "http://tempuri.org/IOrderSoapService";

    public async Task<int> CreateOrder(CartModel cartModel)
    {
        var orderItems = cartModel.Items.Select(x => new CreateOrderItem(x.ProductId, x.Count)).ToList();
        var command = new CreateOrder(cartModel.AddressId, orderItems);
        return await soapClient.SendCommand(Endpoint, ServiceNamespace, "CreateOrder", command, isCreateCommand: true);
    }

    public async Task<PagedResult<OrderListItemDto>> GetOrders(int pageNumber, int pageSize, int? clientId)
    {
        var query = new GetOrders(pageNumber, pageSize, null, clientId);
        
        var result = await soapClient.SendQuery<GetOrders, GetOrdersResponseDto>(Endpoint, ServiceNamespace, "GetOrders", query);
        return new PagedResult<OrderListItemDto>(result.Items, pageNumber, pageSize, result.TotalPages, result.TotalItemsCount);
    }

    public async Task<OrderDto> GetOrder(int id)
        => await soapClient.SendQuery<GetOrder, OrderDto>(Endpoint, ServiceNamespace, nameof(GetOrder), new GetOrder(id));

    public async Task UpdateOrderStatus(int id, OrderStatusDto status)
    {
        switch (status)
        {
            case OrderStatusDto.Completed:
                await soapClient.SendCommand(Endpoint, ServiceNamespace, nameof(CompleteOrder), new CompleteOrder(id));
                break;
            case OrderStatusDto.Cancelled:
                await soapClient.SendCommand(Endpoint, ServiceNamespace, nameof(CancelOrder), new CancelOrder(id));
                break;

            case OrderStatusDto.Processing:
                await soapClient.SendCommand(Endpoint, ServiceNamespace, nameof(ProcessOrder), new ProcessOrder(id));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }
}

using System.Text;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Features.Orders.UpdateOrderState;

namespace OnlineStore.Server.Emails.EmailDefinitions;

public class OrderCreatedSummaryEmail : EmailDefinition
{
    private readonly Order _order;
    private readonly OrderContext _orderContext;

    public OrderCreatedSummaryEmail(Order order, OrderContext orderContext, string recipientEmail, string? recipientName, string? senderEmail)
        : base(recipientEmail, recipientName, senderEmail)
    {
        _order = order;
        _orderContext = orderContext;
    }

    public override string Subject => "Zamówienie zostało złożone";
    public override string TemplateName => "OrderCreatedSummary";

    public override ICollection<EmailReplacement> GetReplacements()
    {
        var replacements = new List<EmailReplacement>
        {
            new("{{OrderId}}", _order.Id.ToString()),
            new("{{OrderCreatedDate}}", _order.CreatedDate.ToShortDateString()),
            new("{{ClientName}}", $"{_orderContext.Client.FullName}"),
            new("{{ClientEmail}}", _orderContext.Client.Email),
            new("{{OrderAddressStreet}}", _orderContext.OrderAddress.Street),
            new("{{OrderAddressStreetNumber}}", _orderContext.OrderAddress.StreetNumber),
            new("{{OrderAddressPostalCode}}", _orderContext.OrderAddress.PostalCode),
            new("{{OrderAddressCity}}", _orderContext.OrderAddress.City),
            new("{{OrderAddressCountry}}", _orderContext.OrderAddress.Country),
            new("{{OrderItemsRows}}", GetOrderItemsHtmlRows()),
            new("{{OrderTotalGross}}", _order.TotalGross + "zł"),
            new("{{OrderTotalNet}}", _order.TotalNet + "zł")
        };

        return replacements;
    }

    private string GetOrderItemsHtmlRows()
    {
        var rowsBuilder = new StringBuilder();
        foreach (var orderItem in _order.OrderItems)
        {
            var html = GetOrderItemHtml();
            html = html.Replace("{{OrderItemProductName}}", orderItem.Product.Name);
            html = html.Replace("{{OrderItemQuantity}}", orderItem.Quantity.ToString());
            html = html.Replace("{{OrderItemPriceNet}}", orderItem.PriceNet + "zł");
            html = html.Replace("{{OrderItemPriceGross}}", orderItem.PriceGross + "zł");
            rowsBuilder.Append(html);
        }

        return rowsBuilder.ToString();
    }

    private static string GetOrderItemHtml()
    {
        return @"<div class=""order-item"">
                    <p><strong>Nazwa produktu:</strong> {{OrderItemProductName}}</p>
                    <p><strong>Ilość:</strong> {{OrderItemQuantity}}</p>
                    <p><strong>Cena (Netto):</strong> {{OrderItemPriceNet}}</p>
                    <p><strong>Cena (Brutto):</strong> {{OrderItemPriceGross}}</p>
                    <hr>
                </div>";
    }
}
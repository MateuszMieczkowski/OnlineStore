using System.Text;
using OnlineStore.Server.Entities;

namespace OnlineStore.Server.Emails.EmailDefinitions;

public class OrderCreatedSummaryEmail : EmailDefinition
{
    private readonly Order _order;

    public OrderCreatedSummaryEmail(Order order, string recipientEmail, string? recipientName, string? senderEmail)
        : base(recipientEmail, recipientName, senderEmail)
    {
        _order = order;
    }

    public override string Subject => "Zamówienie zostało złożone";
    public override string TemplateName => "OrderCreatedSummary";

    public override ICollection<EmailReplacement> GetReplacements()
    {
        var replacements = new List<EmailReplacement>
        {
            new("{{OrderId}}", _order.Id.ToString()),
            new("{{OrderCreatedDate}}", _order.CreatedDate.ToShortDateString()),
            new("{{ClientName}}", $"{_order.Client.FullName}"),
            new("{{ClientEmail}}", _order.Client.Email),
            new("{{OrderAddressStreet}}", _order.Address.Street),
            new("{{OrderAddressStreetNumber}}", _order.Address.StreetNumber),
            new("{{OrderAddressPostalCode}}", _order.Address.PostalCode),
            new("{{OrderAddressCity}}", _order.Address.City),
            new("{{OrderAddressCountry}}", _order.Address.Country),
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
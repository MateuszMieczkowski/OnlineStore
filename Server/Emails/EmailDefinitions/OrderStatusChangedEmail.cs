using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Emails.EmailDefinitions;

public class OrderStatusChangedEmail : EmailDefinition
{
    private readonly Order _order;
    private readonly OrderAddress _orderAddress;

    public OrderStatusChangedEmail(Order order, OrderAddress orderAddress, string recipientEmail, string? recipientName, string? senderEmail) :
        base(recipientEmail, recipientName, senderEmail)
    {
        _order = order;
        _orderAddress = orderAddress;
    }

    public override string Subject => "Zmiana statusu zamówienia";
    public override string TemplateName => "OrderStatusChanged";

    public override ICollection<EmailReplacement> GetReplacements()
    {
        var replacements = new List<EmailReplacement>
        {
            new("{{OrderId}}", _order.Id.ToString()),
            new("{{OrderCreatedDate}}", _order.CreatedDate.ToShortDateString()),
            new("{{OrderModifiedDate}}", _order.ModifiedDate.ToShortDateString()),
            new("{{OrderStatus}}", EnumHelper.GetDescription(_order.Status)),
            new("{{OrderAddressStreet}}", _orderAddress.Street),
            new("{{OrderAddressStreetNumber}}", _orderAddress.StreetNumber),
            new("{{OrderAddressPostalCode}}", _orderAddress.PostalCode),
            new("{{OrderAddressCity}}", _orderAddress.City),
            new("{{OrderAddressCountry}}", _orderAddress.Country),
            new("{{OrderTotalGross}}", _order.TotalGross + "zł"),
            new("{{OrderTotalNet}}", _order.TotalNet + "zł")
        };
        return replacements;
    }
}
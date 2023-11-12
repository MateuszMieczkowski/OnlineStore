using OnlineStore.Server.Entities;

namespace OnlineStore.Server.Emails.EmailDefinitions;

public class OrderStatusChangedEmail : EmailDefinition
{
    private readonly Order _order;

    public OrderStatusChangedEmail(Order order, string recipientEmail, string? recipientName, string? senderEmail) :
        base(recipientEmail, recipientName, senderEmail)
    {
        _order = order;
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
            new("{{OrderStatus}}", _order.Status.ToString()),
            new("{{OrderAddressStreet}}", _order.Address.Street),
            new("{{OrderAddressStreetNumber}}", _order.Address.StreetNumber),
            new("{{OrderAddressPostalCode}}", _order.Address.PostalCode),
            new("{{OrderAddressCity}}", _order.Address.City),
            new("{{OrderAddressCountry}}", _order.Address.Country),
            new("{{OrderTotalGross}}", _order.TotalGross + "zł"),
            new("{{OrderTotalNet}}", _order.TotalNet + "zł")
        };
        return replacements;
    }
}
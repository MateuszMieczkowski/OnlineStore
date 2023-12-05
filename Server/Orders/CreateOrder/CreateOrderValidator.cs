using FluentValidation;

namespace OnlineStore.Server.Orders.CreateOrder;

public class CreateOrderValidator : AbstractValidator<Shared.Orders.CreateOrder>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderAddressId)
            .NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty()
            .ForEach(x => x.SetValidator(new CreateOrderItemValidator()));
    }
}

public class CreateOrderItemValidator : AbstractValidator<Shared.Orders.CreateOrder.CreateOrderItem>
{
    public CreateOrderItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Count)
            .GreaterThan(0);
    }
}
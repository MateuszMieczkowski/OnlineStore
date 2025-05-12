using FluentValidation;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<Shared.Products.UpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.ShortDescription)
            .NotEmpty()
            .MaximumLength(500);
        
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Description)
            .MaximumLength(2000);
        
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.PriceNet)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.TaxRateId)
            .NotEmpty();
        
        RuleForEach(x => x.ProductFiles)
            .NotEmpty()
            .SetValidator(new UpdateProductFileValidator());
        
        // RuleFor(x => x.ProductFiles)
        //     .Must(x => x.Count(y => y.ProductFileType == ProductFileTypeDto.Thumbnail) == 1)
        //     .WithMessage("Product must have one and only thumbnail.");
    }
}

public class UpdateProductFileValidator : AbstractValidator<UpdateProductFileDto>
{
    public UpdateProductFileValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        When(
            x => x.Id == null,
            () =>
            {
                RuleFor(x => x.FileBase64).NotEmpty();
            });
        
        RuleFor(x => x.FileBase64)
            .NotEmpty()
            .When(x => x.Id == null);
        
        RuleFor(x => x.ProductFileType)
            .IsInEnum();

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
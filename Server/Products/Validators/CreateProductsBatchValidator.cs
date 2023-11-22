using FluentValidation;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Validators;

public class CreateProductsBatchValidator : AbstractValidator<CreateProductsBatch>
{
    public CreateProductsBatchValidator()
    {
        RuleForEach(x => x.Products)
            .SetValidator(new CreateProductDtoValidator());
    }
}

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
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
        
        RuleFor(x => x.PriceGross)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.TaxRateId)
            .NotEmpty();
        
        RuleForEach(x => x.ProductFiles)
            .NotEmpty()
            .SetValidator(new CreateProductFileValidator());

        RuleFor(x => x.ProductFiles)
            .Must(x => x.Count(y => y.ProductFileType == ProductFileTypeDto.Thumbnail) == 1)
            .WithMessage("Product must have one and only thumbnail.");
    }
}

public class CreateProductFileValidator : AbstractValidator<CreateProductFile>
{
    public CreateProductFileValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.FileBase64)
            .NotEmpty();
        
        RuleFor(x => x.ProductFileType)
            .IsInEnum();

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
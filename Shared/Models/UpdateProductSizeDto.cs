using OnlineStore.Shared.Models.Validators;

namespace OnlineStore.Shared.Models;

public class UpdateProductSizeDto
{
    public Guid? SizeId { get; set; }
    public string? Size { get; set; }

    [MinInt(MinValue = 1, ValueName = "Quantity")]
    public int Quantity { get; set; }
}
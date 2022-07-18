namespace SneakersBase.Shared.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReferenceNumber { get; set; }
        public int Quantity { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsHidden { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public List<ProductSizeDto> AvailableSizes { get; set; }
    }
}

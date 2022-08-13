namespace SneakersBase.Shared.Models
{
    public class ProductSizeDto
    {
        public int Id { get; set; }
        public Guid? SizeId { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}

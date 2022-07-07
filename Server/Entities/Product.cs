using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReferenceNumber { get; set; }
        public string? ThumbnailPath { get; set; }
        public virtual List<ProductSize> AvailableSizes { get; set; }
    }
}

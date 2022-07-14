using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerBase.Shared.Dtos
{
    public class ProductSizeDto
    {
        public int Id { get; set; }
        public int SizeId { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Server.Entities
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Guid? SizeId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Size? Size { get; set; }
    }
}

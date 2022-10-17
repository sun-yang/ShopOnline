using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models
{
    public class OrderDetailsDTO
    {
        public DateTime OrderDate { get; set; }
        public decimal OrderPrice { get; set; }
        public List<OrderDetailsProductDTO> Products { get; set; }
    }
}

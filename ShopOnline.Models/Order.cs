using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderPrice { get; set; }        
        public List<OrderItem> OrderItems { get; set; }
    }
}

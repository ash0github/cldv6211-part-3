using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloCraft.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public ProductInformation Product { get; set; }
        [Required]
        public int Quantity { get; set; }

        public CartItem()
        {
            
        }
    }
}

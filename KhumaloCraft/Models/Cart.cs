using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloCraft.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        [Required]
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public bool OrderConfirmed { get; set; }
        public DateTime? OrderDate { get; set; }
        [Required]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public IdentityUser User { get; set; }
    }
}

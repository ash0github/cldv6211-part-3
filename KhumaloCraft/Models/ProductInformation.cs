using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloCraft.Models
{
    public class ProductInformation
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        public string ProductCategory { get; set; }
        [Required]
        public Boolean ProductAvailability { get; set; }
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public IdentityUser User { get; set; }

        public ProductInformation()
        {

        }
    }
}

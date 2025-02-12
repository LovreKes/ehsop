using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!; // ✅ Ovdje osiguravamo da ne bude null

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}

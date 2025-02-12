using System.ComponentModel.DataAnnotations;

namespace eshop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 99999.99, ErrorMessage = "Cijena mora biti veća od 0!")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}

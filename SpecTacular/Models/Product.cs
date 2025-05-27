using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecTacular.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Column(TypeName = "decimal(18,2)")] // Specifies precision and scale
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } // Example: Eyeglasses, Sunglasses, Screen Glasses

        public DateTime DateAdded { get; set; } = DateTime.Now; // default value
    }
}

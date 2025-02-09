using System.ComponentModel.DataAnnotations;

namespace Products.API.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public required string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = "";

        [Required]
        public double Amount { get; set; }
    }
}

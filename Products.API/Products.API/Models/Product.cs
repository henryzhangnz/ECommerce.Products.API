using System.ComponentModel.DataAnnotations;

namespace Products.API.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Domain.Entities
{
    public class Product : Base
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        public string Category { get; set; } = string.Empty;

        public int Stock { get; set; } = 0;
        public bool IsAvailable { get; set; } = true;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "The name of the product must be provided")]
        [MaxLength(50, ErrorMessage = "The size of the product name must not exceed 50 characters")]
        public string name { get; set; }

        [Required(ErrorMessage = "The description of the product must be provided")]
        [MaxLength(50, ErrorMessage = "The size of the product description must not exceed 50 characters")]
        public string description { get; set; }

        [Required(ErrorMessage = "The price of the product must be provided")]
        public decimal price { get; set; }
        public string? imageUrl { get; set; }

        [Required(ErrorMessage = "The businessID of the product must be provided")]
        public Guid? businessID { get; set; }

        public bool isActive { get; set; }
    }
}

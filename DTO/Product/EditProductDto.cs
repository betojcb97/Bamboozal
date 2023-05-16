using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class EditProductDto
    {
        [MaxLength(50, ErrorMessage = "The size of the product name must not exceed 50 characters")]
        public string? name { get; set; }
        [MaxLength(50, ErrorMessage = "The size of the product description must not exceed 50 characters")]
        public string? description { get; set; }
        public decimal price { get; set; }
        public decimal cost { get; set; }
        public decimal tax { get; set; }
        public decimal discount { get; set; }
        public string? imageUrl { get; set; }
        public Guid? businessID { get; set; }
        public bool isActive { get; set; }
    }
}

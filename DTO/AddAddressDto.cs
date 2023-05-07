using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddAddressDto
    {
        [Required(ErrorMessage = "The street of the business must be provided")]
        [MaxLength(70, ErrorMessage = "The size of the street must not exceed 70 characters")]
        [MinLength(5, ErrorMessage = "The size of the street must be at least 5 characters")]
        public string street { get; set; }

        [Required(ErrorMessage = "The number of the business must be provided")]
        [MaxLength(10, ErrorMessage = "The size of the number must not exceed 10 characters")]
        [MinLength(1, ErrorMessage = "The size of the number must be at least 1 character")]
        public string number { get; set; }

        [Required(ErrorMessage = "The city of the business must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the city must not exceed 20 characters")]
        [MinLength(2, ErrorMessage = "The size of the city must be at least 2 characters")]
        public string city { get; set; }

        [Required(ErrorMessage = "The country of the business must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the country must not exceed 20 characters")]
        [MinLength(2, ErrorMessage = "The size of the country must be at least 2 characters")]
        public string country { get; set; }

        [Required(ErrorMessage = "The postalCode of the business must be provided")]
        [MaxLength(8, ErrorMessage = "The size of the postalCode must not exceed 8 characters")]
        [MinLength(8, ErrorMessage = "The size of the postalCode must be at least 8 characters")]
        public string postalCode { get; set; }

        public Guid? businessId { get; set; }
        public Guid? userId { get; set; }

    }
}

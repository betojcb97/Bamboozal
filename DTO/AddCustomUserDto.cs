using System.ComponentModel.DataAnnotations;
using static Bamboo.Models.User;

namespace Bamboo.DTO
{
    public class AddCustomUserDto
    {

        [Required(ErrorMessage = "The userName must be provided")]
        [MaxLength(50, ErrorMessage = "The size of the userName must not exceed 50 characters")]
        public string userName { get; set; }

        [Required(ErrorMessage = "The userEmail must be provided")]
        [MaxLength(50, ErrorMessage = "The size of the userEmail must not exceed 50 characters")]
        public string userEmail { get; set; }

        [Required(ErrorMessage = "The userPassword must be provided")]
        [MaxLength(50, ErrorMessage = "The size of the userPassword must not exceed 50 characters")]
        public string userPassword { get; set; }
        [Required]
        [Compare("userPassword")]
        public string rePassword { get; set; }

        [Required(ErrorMessage = "The userFirstName must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the userFirstName must not exceed 20 characters")]
        public string userFirstName { get; set; }

        [Required(ErrorMessage = "The userLastName must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the userLastName must not exceed 20 characters")]
        public string userLastName { get; set; }

        public Guid? addressID { get; set; }

        public bool isActive { get; set; }

        public Guid? ownerOfBusinessID { get; set; }

        public DateTime? dateOfBirth { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using static Bamboo.Models.User;

namespace Bamboo.DTO
{
    public class EditCustomUserDto
    {
        public Guid userID { get; set; }

        [MaxLength(50, ErrorMessage = "The size of the userName must not exceed 50 characters")]
        public string? userName { get; set; }

        [MaxLength(50, ErrorMessage = "The size of the userEmail must not exceed 50 characters")]
        public string? userEmail { get; set; }

        [MaxLength(20, ErrorMessage = "The size of the userFirstName must not exceed 20 characters")]
        public string? userFirstName { get; set; }

        [MaxLength(20, ErrorMessage = "The size of the userLastName must not exceed 20 characters")]
        public string? userLastName { get; set; }

        public Guid? addressID { get; set; }

        public bool isActive { get; set; }

        public Guid? ownerOfBusinessID { get; set; }

        public DateTime? dateOfBirth { get; set; }

        public string? role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Bamboo.Models
{
    public class User : IdentityUser
    {
  
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

        public DateTime? dateOfRegister { get; set; }

        public User() : base()
        {
            dateOfRegister = DateTime.Now;
        }
    }
}

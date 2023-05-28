using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bamboo.Models
{
    public class CustomUser
    {
        [Required]
        [Key]
        public Guid userID { get; set; }

        [Required(ErrorMessage = "The userName must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the userName must not exceed 20 characters")]
        public string userName { get; set; }

        [Required(ErrorMessage = "The userFirstName must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the userFirstName must not exceed 20 characters")]
        public string userFirstName { get; set; }

        [Required(ErrorMessage = "The userLastName must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the userLastName must not exceed 20 characters")]
        public string userLastName { get; set; }

        [Required(ErrorMessage = "The userEmail must be provided")]
        public string userEmail { get; set; }

        [Required(ErrorMessage = "The userPassword must be provided")]
        public string userPassword { get; set; }

        public Guid? addressID { get; set; }

        public virtual Address? address { get; set; }

        public bool isActive { get; set; }

        public Guid? ownerOfBusinessID { get; set; }

        public virtual Business? business { get; set; }

        public DateTime? dateOfBirth { get; set; }

        public DateTime? dateOfRegister { get; set; }

        public string? token { get; set; }

        public DateTime? tokenExpirationDate { get; set; }

        public string? emailTokenConfirmation { get; set; }

        public string? role { get; set; }

        public CustomUser() : base()
        {
            userID = Guid.NewGuid();
            dateOfRegister = DateTime.Now;
            if (ownerOfBusinessID.HasValue) { role = "BusinessOwner"; }
            else { role = "Consumer"; }
        }
    }
}

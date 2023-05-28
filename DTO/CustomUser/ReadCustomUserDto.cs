using Bamboo.Models;
using System.ComponentModel.DataAnnotations;
using static Bamboo.Models.CustomUser;

namespace Bamboo.DTO
{
    public class ReadCustomUserDto
    {
        public Guid userID { get; set; }
        public string userFirstName { get; set; }
        public string userLastName { get; set; }
        public Guid? addressID { get; set; }
        public virtual ReadAddressDto? addressDto { get; set; }
        public bool isActive { get; set; }
        public Guid? ownerOfBusinessID { get; set; }
        public virtual ReadBusinessDto? businessDto { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? dateOfRegister { get; set; }

        public string? role { get; set; }
    }
}

using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadUserDto
    {
        public string userFirstName { get; set; }
        public string userLastName { get; set; }
        public Guid? addressID { get; set; }
        public virtual ReadAddressDto? addressDto { get; set; }
        public bool isActive { get; set; }
        public Guid? ownerOfBusinessID { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public DateTime? dateOfRegister { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class LoginCustomUserDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string userPassword { get; set; }
    }
}

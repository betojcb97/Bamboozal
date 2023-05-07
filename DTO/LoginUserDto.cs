using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class LoginUserDto
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string userPassword { get; set; }
    }
}

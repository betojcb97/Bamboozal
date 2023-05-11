using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class LoginUserDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string userPassword { get; set; }
    }
}

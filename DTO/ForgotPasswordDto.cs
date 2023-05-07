using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}

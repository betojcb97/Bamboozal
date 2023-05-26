using System.ComponentModel.DataAnnotations;

namespace Bamboo.Models
{
    public class Log
    {
        [Key]
        [Required]
        public Guid logID { get; set; }

        public string log { get; set; }

        public DateTime? dateOfCreation { get; set; }

        public Log()
        {
            logID = Guid.NewGuid();
            dateOfCreation = DateTime.Now;
        }
    }
}

using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class EditCartDto
    {

        public Guid? userID { get; set; }

        public List<Guid>? productsIds { get; set; }

    }
}

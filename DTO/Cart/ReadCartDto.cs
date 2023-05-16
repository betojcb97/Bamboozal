using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadCartDto
    {
        public Guid cartID { get; set; }

        public Guid? userID { get; set; }

        public List<Guid>? productsIds { get; set; }

        public List<ReadProductDto>? productsDto { get; set; }

        public DateTime? dateOfCreation { get; set; }

    }
}

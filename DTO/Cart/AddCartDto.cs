using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddCartDto
    {
        public Guid? userID { get; set; }

        public List<Dictionary<Guid, int>> productsIdsAndQuantities { get; set; }
    }
}

using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddCartDto
    {
        public Dictionary<string, int> productsIdsAndQuantities { get; set; }
    }
}

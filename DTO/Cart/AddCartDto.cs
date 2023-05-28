using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddCartDto
    {
        public List<Dictionary<string, int>> productsIdsAndQuantities { get; set; }
    }
}

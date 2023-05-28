using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadCartDto
    {
        public Guid cartID { get; set; }

        public Guid? userID { get; set; }

        public decimal total { get; set; }

        public decimal cost { get; set; }

        public decimal tax { get; set; }

        public decimal subtotal { get; set; }

        public decimal discount { get; set; }

        public Dictionary<string, int> productsIdsAndQuantities { get; set; }

        public DateTime? dateOfCreation { get; set; }

    }
}

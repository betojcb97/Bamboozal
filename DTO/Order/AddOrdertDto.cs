using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddOrderDto
    {
        public Guid OrderID { get; set; }

        public decimal total { get; set; }

        public decimal cost { get; set; }

        public decimal tax { get; set; }

        public decimal subtotal { get; set; }

        public decimal discount { get; set; }

        public Guid? userID { get; set; }

        public Guid? businessID { get; set; }

        public List<Guid?>? productsIds { get; set; }

        public Guid? deliveryAddressID { get; set; }
    }
}

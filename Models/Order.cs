using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bamboo.Models
{
    public class Order
    {
        [Key]
        public Guid orderID { get; set; }

        public decimal total { get; set; }

        public decimal cost { get; set; }

        public decimal tax { get; set; }

        public decimal subtotal { get; set; }

        public decimal discount { get; set; }

        [Required(ErrorMessage = "The userID of the Order must be provided")]
        public Guid? userID { get; set; }

        public virtual CustomUser user { get; set; }

        [Required(ErrorMessage = "The businessID of the Order must be provided")]
        public Guid? businessID { get; set; }

        public virtual Business business { get; set; }

        [NotMapped]
        public virtual List<Guid> productsIds { get; set; }

        public virtual ICollection<Product> products { get; set; }

        public Guid? deliveryAddressID { get; set; }

        public virtual Address deliveryAddress { get; set; }

        public Order()
        {
            orderID = Guid.NewGuid();
            total = subtotal - discount;
        }
    }
}

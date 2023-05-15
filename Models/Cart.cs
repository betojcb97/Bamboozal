using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bamboo.Models
{
    public class Cart
    {
        [Key]
        public Guid cartID { get; set; }

        public Guid? userID { get; set; }

        [NotMapped]
        public virtual List<Guid> productsIds { get; set; }

        public virtual ICollection<Product> products { get; set; }

        public DateTime? dateOfCreation { get; set; }

        public Cart()
        {
            cartID = Guid.NewGuid();
            dateOfCreation = DateTime.Now;
        }
    }
}

using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadOrderDto
    {
        public Guid? OrderID { get; set; }

        public decimal? total { get; set; }

        public decimal? cost { get; set; }

        public decimal? tax { get; set; }

        public decimal? subtotal { get; set; }

        public decimal? discount { get; set; }

        public Guid? userID { get; set; }

        public virtual ReadCustomUserDto userDto { get; set; }

        public Guid? businessID { get; set; }

        public virtual ReadBusinessDto businessDto { get; set; }

        public List<Guid?>? productsIds { get; set; }

        public virtual List<ReadProductDto> productsDto { get; set; }

        public Guid? deliveryAddressID { get; set; }
    }
}

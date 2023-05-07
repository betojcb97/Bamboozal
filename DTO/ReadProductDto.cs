using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadProductDto
    {
        public Guid productID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }
        public Guid businessID { get; set; }
        public virtual ReadBusinessDto? businessDto { get; set; }
        public bool isActive { get; set; }
    }
}

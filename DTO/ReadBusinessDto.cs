using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadBusinessDto
    {
        public Guid businessID { get; set; }
        public string name { get; set; }

        public string description { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public ReadAddressDto? addressDto { get; set; }

        public bool isActive { get; set; }

        public string? logoUrl { get; set; }
    }
}

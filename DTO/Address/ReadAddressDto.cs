using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadAddressDto
    {
        public Guid addressID { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
    }
}

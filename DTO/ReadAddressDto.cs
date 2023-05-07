using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class ReadAddressDto
    {
        public string street { get; set; }
        public string number { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public ReadBusinessDto? businessDto { get; set; }
        public ReadUserDto? userDto { get; set; }
    }
}

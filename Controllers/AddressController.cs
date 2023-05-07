using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {

        private BambooContext db;
        private IMapper _mapper;
        public AddressController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost("AddAddress")]
        public IActionResult AddAddress([FromBody] AddAddressDto addressDto)
        {
            Address address = _mapper.Map<Address>(addressDto);
            db.Addresses.Add(address);
            db.SaveChanges();
            return CreatedAtAction(nameof(address), address);
        }

        [HttpPost("EditAddress")]
        public IActionResult EditAddress([FromBody] EditAddressDto addressDto)
        {
            Address dbAddress = db.Addresses.Where(a => a.addressID.Equals(addressDto.addressId)).FirstOrDefault();
            if (dbAddress == null) return NotFound();
            Address addressNewInfo = _mapper.Map<Address>(addressDto);

            dbAddress.street = addressNewInfo.street ?? addressNewInfo.street;
            dbAddress.number = addressNewInfo.number ?? addressNewInfo.number;
            dbAddress.country = addressNewInfo.country ?? addressNewInfo.country;
            dbAddress.city = addressNewInfo.city ?? addressNewInfo.city;
            dbAddress.postalCode = addressNewInfo.postalCode ?? addressNewInfo.postalCode;

            db.Entry(dbAddress).State = EntityState.Modified;
            db.SaveChanges();
            return CreatedAtAction(nameof(dbAddress), dbAddress);
        }

        [HttpGet("GetAddresses")]
        public IActionResult GetAddresses()
        {
            return Ok(db.Addresses);
        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(Guid id)
        {
            var address = db.Addresses.FirstOrDefault(a => a.addressID.Equals(id));
            if (address == null) { return NotFound(); }
            return Ok(address);
        }
    }
}

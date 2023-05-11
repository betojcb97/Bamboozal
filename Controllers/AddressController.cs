using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : Controller
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

        [HttpPost("RemoveAddress/{addressID}")]
        public IActionResult RemoveAddress(Guid addressID)
        {
            Address dbAddress = db.Addresses.Where(a => a.addressID.Equals(addressID)).FirstOrDefault();
            if (dbAddress == null) { return Ok(); }
            db.Addresses.Remove(dbAddress);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("EditAddress/{addressID}")]
        public IActionResult EditAddress(Guid addressID, [FromBody] EditAddressDto addressDto)
        {
            Address dbAddress = db.Addresses.Where(a => a.addressID.Equals(addressID)).FirstOrDefault();
            if (dbAddress == null) return NotFound();
            Address addressNewInfo = _mapper.Map<Address>(addressDto);

            PropertyInfo[] properties = addressNewInfo.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(addressNewInfo) != null && property.Name != "addressID")
                {
                    property.SetValue(dbAddress, property.GetValue(addressNewInfo));
                }
            }

            db.Entry(dbAddress).State = EntityState.Modified;
            db.SaveChanges();
            return CreatedAtAction(nameof(dbAddress), dbAddress);
        }

        [HttpGet("ListAddresses")]
        public IActionResult ListAddresses()
        {
            List<ReadAddressDto> readAddressDtos = _mapper.Map<List<ReadAddressDto>>(db.Addresses.ToList());
            return Json(readAddressDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(Guid id)
        {
            var address = db.Addresses.FirstOrDefault(a => a.addressID.Equals(id));
            if (address == null) { return NotFound(); }
            return Ok(address);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

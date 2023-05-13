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

        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        public AddressController(BambooContext context, IMapper _mapper, TokenValidator _tokenValidator)
        {
            db = context;
            mapper = _mapper;
            tokenValidator = _tokenValidator;
        }

        [HttpPost("AddAddress")]
        public IActionResult AddAddress([FromBody] AddAddressDto addressDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Address address = mapper.Map<Address>(addressDto);
                db.Addresses.Add(address);
                db.SaveChanges();
                return CreatedAtAction(nameof(address), address);
            }
            else { return Unauthorized(); }
        }

        [HttpPost("RemoveAddress/{addressID}")]
        public IActionResult RemoveAddress(Guid addressID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Address dbAddress = db.Addresses.Where(a => a.addressID.Equals(addressID)).FirstOrDefault();
                if (dbAddress == null) { return Ok(); }
                db.Addresses.Remove(dbAddress);
                db.SaveChanges();
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditAddress/{addressID}")]
        public IActionResult EditAddress(Guid addressID, [FromBody] EditAddressDto addressDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Address dbAddress = db.Addresses.Where(a => a.addressID.Equals(addressID)).FirstOrDefault();
                if (dbAddress == null) return NotFound();
                Address addressNewInfo = mapper.Map<Address>(addressDto);

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
            else { return Unauthorized(); }
        }

        [HttpGet("ListAddresses")]
        public IActionResult ListAddresses()
        {
            List<ReadAddressDto> readAddressDtos = mapper.Map<List<ReadAddressDto>>(db.Addresses.ToList());
            return Json(readAddressDtos);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

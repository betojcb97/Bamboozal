using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Net;
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
        private IHttpContextAccessor httpContextAccessor;
        private LogService logManager;
        public AddressController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, LogService logManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
            this.logManager = logManager;
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
                logManager.AddLog($"Address: {address.street} id=({address.addressID}) added successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
                return CreatedAtAction(nameof(address), address);
            }
            else { return Unauthorized(); }
        }

        [HttpPost("RemoveAddress/{addressID}")]
        public IActionResult RemoveAddress(string addressID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Address dbAddress = db.Addresses.Where(a => a.addressID.ToString().Equals(addressID)).FirstOrDefault();
                if (dbAddress == null) { return Ok(); }
                db.Addresses.Remove(dbAddress);
                db.SaveChanges();
                logManager.AddLog($"Address: {dbAddress.street} id=({dbAddress.addressID}) removed successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditAddress/{addressID}")]
        public IActionResult EditAddress(string addressID, [FromBody] EditAddressDto addressDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Address dbAddress = db.Addresses.Where(a => a.addressID.ToString().Equals(addressID)).FirstOrDefault();
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
                logManager.AddLog($"Address: {dbAddress.street} id=({dbAddress.addressID}) edited successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
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

        [HttpGet("ListAddressInfo/{addressID}")]
        public IActionResult ListAddressInfo(string addressID)
        {
            Address dbAddress = db.Addresses.Where(p => p.addressID.ToString().Equals(addressID)).FirstOrDefault();
            if (dbAddress == null) return NotFound();
            ReadAddressDto dbAddressDto = mapper.Map<ReadAddressDto>(dbAddress);
            return Json(dbAddress);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

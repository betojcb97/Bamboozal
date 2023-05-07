using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {

        private BambooContext db;
        private IMapper _mapper;
        public BusinessController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddBusiness([FromBody] AddBusinessDto businessDto)
        {
            Business business = _mapper.Map<Business>(businessDto);
            Business exists = db.Businesses.Where(b => b.name == business.name).FirstOrDefault();
            if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
            db.Businesses.Add(business);
            db.SaveChanges();
            Console.WriteLine(business);
            return CreatedAtAction(nameof(business), business);
        }

        [HttpGet]
        public IActionResult GetBusinesses()
        {
            return Ok(db.Businesses);
        }
        [HttpGet("{id}")]
        public IActionResult GetBusinessById(Guid id)
        {
            var business = db.Businesses.FirstOrDefault(b => b.businessID.Equals(id));
            if (business == null) { return NotFound(); }
            return Ok(business);
        }
    }
}

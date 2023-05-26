using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;
using System.Linq.Dynamic.Core;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : Controller
    {

        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        public LogController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("ListLogs")]
        public IActionResult ListLogs([FromQuery] string? orderBy = "dateOfCreation", [FromQuery] string? ascdesc = "descending")
        {
            string ordering = orderBy + " " + ascdesc;
            List<Log> logs = db.Logs
            .AsQueryable()
            .OrderBy(ordering)
            .ToList();
            return Json(logs);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

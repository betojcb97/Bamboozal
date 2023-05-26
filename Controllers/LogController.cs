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

        [HttpPost("AddLog")]
        public IActionResult AddLog(string logInfo)
        {
            Log log = new Log();
            log.log = logInfo;
            db.Logs.Add(log);
            db.SaveChanges();
            return CreatedAtAction(nameof(log), log);
        }

        [HttpGet("ListLogs")]
        public IActionResult ListLogs()
        {
            List<Log> logs = db.Logs.ToList();
            return Json(logs);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "ActiveUser")]
        public IActionResult Get()
        {
            return Ok("Access granted");
        }
    }
}

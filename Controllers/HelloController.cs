using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SOA.Controllers
{
    [ApiController]
    [Route("")]
    public class HelloController : ControllerBase
    {
        [Authorize]
        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok("Hello World");
        }
    }
}

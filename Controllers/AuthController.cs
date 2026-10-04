using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SOA.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [Authorize]
        [HttpGet("auth")]
        public IActionResult Auth()
        {
            return Ok(new
            {
                message = "Token hợp lệ",
                userName = User.Identity?.Name
            });
        }
    }
}

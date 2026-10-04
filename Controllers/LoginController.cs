using SOA.Data;
using SOA.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace SOA.Controllers
{
    [ApiController]
    [Route("")]
    public class LoginController : ControllerBase {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public LoginController(
            AppDbContext context,
            JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.UserName == request.UserName &&
                    x.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Sai username hoặc password"
                });
            }

            var token = _jwtService.GenerateToken(
                user.UserName
            );

            user.Token = token;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đăng nhập thành công",
                userName = user.UserName,
                token = token
            });
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = "";

        public string Password { get; set; } = "";
    }

}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult GetPublic()
        {
            return Ok(new { message = "This is a public endpoint", timestamp = DateTime.UtcNow });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult GetProtected()
        {
            return Ok(new { 
                message = "This is a protected endpoint - JWT RS256 is working!", 
                user = User.Identity?.Name,
                timestamp = DateTime.UtcNow 
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { 
                message = "This is an admin-only endpoint", 
                user = User.Identity?.Name,
                timestamp = DateTime.UtcNow 
            });
        }

        [HttpGet("exception")]
        public IActionResult TestException()
        {
            throw new InvalidOperationException("This is a test exception to verify global exception handler");
        }
    }
}
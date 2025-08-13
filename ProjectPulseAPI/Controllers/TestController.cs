using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
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

        [HttpGet("debug-user/{email}")]
        public async Task<IActionResult> DebugUser(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    return Ok(new { 
                        found = false, 
                        message = "User not found",
                        searchedEmail = email
                    });
                }

                return Ok(new { 
                    found = true,
                    user = new {
                        user.Id,
                        user.Email,
                        user.FirstName,
                        user.LastName,
                        user.IsActive,
                        user.IsDeleted,
                        HasPassword = !string.IsNullOrEmpty(user.PasswordHash),
                        PasswordHashPrefix = user.PasswordHash?.Substring(0, Math.Min(10, user.PasswordHash.Length))
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    found = false, 
                    error = ex.Message,
                    searchedEmail = email
                });
            }
        }
    }
}
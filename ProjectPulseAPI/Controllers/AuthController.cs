using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPulseAPI.Application.Interfaces;
using System.Security.Claims;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _authService.RefreshTokenAsync(token);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _authService.LogoutAsync(token);
            
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetCurrentUserAsync(token);
            
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                role = user.Role.ToString(),
                isActive = user.IsActive
            });
        }

        [HttpPost("validate")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isValid = await _authService.ValidateTokenAsync(token);
            
            return Ok(new { valid = isValid });
        }
    }
}
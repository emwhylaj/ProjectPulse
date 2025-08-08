using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Welcome to ProjectPulse API",
                version = "1.0.0",
                documentation = "/swagger",
                timestamp = DateTime.UtcNow,
                endpoints = new
                {
                    auth = "/api/auth",
                    users = "/api/users", 
                    projects = "/api/projects",
                    tasks = "/api/tasks",
                    notifications = "/api/notifications",
                    activities = "/api/activities",
                    test = "/api/test"
                }
            });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                uptime = DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime).ToString(),
                version = "1.0.0"
            });
        }
    }
}
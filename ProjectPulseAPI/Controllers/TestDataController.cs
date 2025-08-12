using Microsoft.AspNetCore.Mvc;
using ProjectPulseAPI.Application.Services;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDataController : ControllerBase
    {
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = InMemoryDataService.GetUsers().Take(10).Select(u => new {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.IsActive,
                u.CreatedAt
            });
            return Ok(users);
        }

        [HttpGet("projects")]
        public IActionResult GetProjects()
        {
            var projects = InMemoryDataService.GetProjects().Take(10).Select(p => new {
                p.Id,
                p.Name,
                p.Description,
                p.Status,
                p.Budget,
                p.StartDate,
                p.EndDate,
                p.ProjectManagerId
            });
            return Ok(projects);
        }

        [HttpGet("tasks")]
        public IActionResult GetTasks()
        {
            var tasks = InMemoryDataService.GetTasks().Take(10).Select(t => new {
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.Priority,
                t.DueDate,
                t.ProjectId,
                t.AssignedToId,
                t.Progress
            });
            return Ok(tasks);
        }

        [HttpGet("notifications")]
        public IActionResult GetNotifications()
        {
            var notifications = InMemoryDataService.GetNotifications().Take(10).Select(n => new {
                n.Id,
                n.Title,
                n.Message,
                n.Type,
                n.IsRead,
                n.UserId,
                n.CreatedAt
            });
            return Ok(notifications);
        }

        [HttpGet("project-members")]
        public IActionResult GetProjectMembers()
        {
            var members = InMemoryDataService.GetProjectMembers().Take(10).Select(pm => new {
                pm.Id,
                pm.ProjectId,
                pm.UserId,
                pm.Role,
                pm.IsActive,
                pm.JoinedAt
            });
            return Ok(members);
        }

        [HttpGet("task-comments")]
        public IActionResult GetTaskComments()
        {
            var comments = InMemoryDataService.GetTaskComments().Take(10).Select(tc => new {
                tc.Id,
                tc.TaskId,
                tc.UserId,
                tc.Content,
                tc.CreatedAt,
                tc.ParentCommentId
            });
            return Ok(comments);
        }

        [HttpGet("project-activities")]
        public IActionResult GetProjectActivities()
        {
            var activities = InMemoryDataService.GetProjectActivities().Take(10).Select(pa => new {
                pa.Id,
                pa.ProjectId,
                pa.UserId,
                pa.ActivityType,
                pa.Description,
                pa.CreatedAt
            });
            return Ok(activities);
        }

        [HttpGet("seed-data")]
        public IActionResult SeedData()
        {
            InMemoryDataService.SeedData();
            return Ok(new { 
                message = "Data reseeded successfully",
                userCount = InMemoryDataService.GetUsers().Count(),
                projectCount = InMemoryDataService.GetProjects().Count(),
                taskCount = InMemoryDataService.GetTasks().Count(),
                notificationCount = InMemoryDataService.GetNotifications().Count(),
                memberCount = InMemoryDataService.GetProjectMembers().Count(),
                commentCount = InMemoryDataService.GetTaskComments().Count(),
                activityCount = InMemoryDataService.GetProjectActivities().Count()
            });
        }
    }
}
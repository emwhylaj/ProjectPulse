using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Shared.Enums;
using System.Security.Claims;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetTaskWithDetails(int id)
        {
            var task = await _taskService.GetTaskWithDetailsAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserTasks(int userId, [FromQuery] TaskStatus? status = null)
        {
            // Check authorization - users can only view their own tasks unless they're admin/manager
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != userId && !IsAdminOrManager(currentUserRole))
                return Forbid();

            var tasks = await _taskService.GetUserTasksAsync(userId, status);
            return Ok(tasks);
        }

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks([FromQuery] TaskStatus? status = null)
        {
            var currentUserId = GetCurrentUserId();
            var tasks = await _taskService.GetUserTasksAsync(currentUserId, status);
            return Ok(tasks);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectTasks(int projectId)
        {
            var tasks = await _taskService.GetProjectTasksAsync(projectId);
            return Ok(tasks);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueTasks()
        {
            var tasks = await _taskService.GetOverdueTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTasksByStatus(TaskStatus status)
        {
            var tasks = await _taskService.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }

        [HttpGet("{parentTaskId}/subtasks")]
        public async Task<IActionResult> GetSubTasks(int parentTaskId)
        {
            var tasks = await _taskService.GetSubTasksAsync(parentTaskId);
            return Ok(tasks);
        }

        [HttpGet("status-counts")]
        public async Task<IActionResult> GetTaskStatusCounts([FromQuery] int? projectId = null)
        {
            var counts = await _taskService.GetTaskStatusCountsAsync(projectId);
            return Ok(counts);
        }

        [HttpGet("due-soon")]
        public async Task<IActionResult> GetTasksDueSoon([FromQuery] int days = 3)
        {
            var tasks = await _taskService.GetTasksDueSoonAsync(days);
            return Ok(tasks);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTasks([FromQuery] string searchTerm, [FromQuery] int? projectId = null, [FromQuery] int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term cannot be empty");

            var tasks = await _taskService.SearchTasksAsync(searchTerm, projectId, userId);
            return Ok(tasks);
        }

        [HttpGet("{taskId}/comments")]
        public async Task<IActionResult> GetTaskComments(int taskId)
        {
            var comments = await _taskService.GetTaskCommentsAsync(taskId);
            return Ok(comments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();
            var task = await _taskService.CreateTaskAsync(taskDto, currentUserId);

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user can update this task
            var canUpdate = await CanUserModifyTask(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var task = await _taskService.UpdateTaskAsync(id, taskDto, currentUserId);
            
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var canUpdate = await CanUserModifyTask(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _taskService.DeleteTaskAsync(id, currentUserId);
            
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] AssignTaskDto assignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var canUpdate = await CanUserModifyTask(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _taskService.AssignTaskAsync(id, assignDto.UserId, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "Task assigned successfully" });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto statusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Users can update status of tasks assigned to them
            var canUpdate = await CanUserModifyTask(id, allowAssigneeUpdate: true);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _taskService.UpdateTaskStatusAsync(id, statusDto.Status, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "Task status updated successfully" });
        }

        [HttpPatch("{id}/progress")]
        public async Task<IActionResult> UpdateTaskProgress(int id, [FromBody] UpdateTaskProgressDto progressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Users can update progress of tasks assigned to them
            var canUpdate = await CanUserModifyTask(id, allowAssigneeUpdate: true);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _taskService.UpdateTaskProgressAsync(id, progressDto.Progress, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "Task progress updated successfully" });
        }

        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddTaskComment(int taskId, [FromBody] AddCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();
            var comment = await _taskService.AddTaskCommentAsync(taskId, commentDto.Content, currentUserId);

            return Ok(comment);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        private bool IsAdminOrManager(string role)
        {
            return role == "Admin" || role == "ProjectManager";
        }

        private async Task<bool> CanUserModifyTask(int taskId, bool allowAssigneeUpdate = false)
        {
            var currentUserRole = GetCurrentUserRole();
            var currentUserId = GetCurrentUserId();

            // Admin can modify any task
            if (currentUserRole == "Admin")
                return true;

            // Get the task to check ownership
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return false;

            // Project managers can modify tasks in their projects
            if (currentUserRole == "ProjectManager")
            {
                // You might need to add a method to check if user manages this project
                return true; // Simplified for now
            }

            // Users can update tasks assigned to them (if allowed)
            if (allowAssigneeUpdate && task.AssignedToId == currentUserId)
                return true;

            return false;
        }
    }

    public class AssignTaskDto
    {
        public int UserId { get; set; }
    }

    public class UpdateTaskStatusDto
    {
        public TaskStatus Status { get; set; }
    }

    public class UpdateTaskProgressDto
    {
        public int Progress { get; set; }
    }

    public class AddCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }
}
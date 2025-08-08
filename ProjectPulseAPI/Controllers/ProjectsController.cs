using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Shared.Enums;
using System.Security.Claims;

namespace ProjectPulseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProjectWithDetails(int id)
        {
            var project = await _projectService.GetProjectWithDetailsAsync(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProjects(int userId)
        {
            // Check authorization - users can only view their own projects unless they're admin/manager
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != userId && !IsAdminOrManager(currentUserRole))
                return Forbid();

            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpGet("my-projects")]
        public async Task<IActionResult> GetMyProjects()
        {
            var currentUserId = GetCurrentUserId();
            var projects = await _projectService.GetUserProjectsAsync(currentUserId);
            return Ok(projects);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetProjectsByStatus(ProjectStatus status)
        {
            var projects = await _projectService.GetProjectsByStatusAsync(status);
            return Ok(projects);
        }

        [HttpGet("manager/{managerId}")]
        public async Task<IActionResult> GetProjectsByManager(int managerId)
        {
            var projects = await _projectService.GetProjectsByManagerAsync(managerId);
            return Ok(projects);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueProjects()
        {
            var projects = await _projectService.GetOverdueProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("status-counts")]
        public async Task<IActionResult> GetProjectStatusCounts()
        {
            var counts = await _projectService.GetProjectStatusCountsAsync();
            return Ok(counts);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects([FromQuery] string searchTerm, [FromQuery] int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term cannot be empty");

            var projects = await _projectService.SearchProjectsAsync(searchTerm, userId);
            return Ok(projects);
        }

        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetProjectMembers(int id)
        {
            var members = await _projectService.GetProjectMembersAsync(id);
            return Ok(members);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();
            var project = await _projectService.CreateProjectAsync(projectDto, currentUserId);

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user can update this project
            var canUpdate = await CanUserModifyProject(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var project = await _projectService.UpdateProjectAsync(id, projectDto, currentUserId);
            
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var canUpdate = await CanUserModifyProject(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _projectService.DeleteProjectAsync(id, currentUserId);
            
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/members")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> AddProjectMember(int id, [FromBody] AddProjectMemberDto memberDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var canUpdate = await CanUserModifyProject(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _projectService.AddProjectMemberAsync(id, memberDto.UserId, memberDto.Role, currentUserId);
            
            if (!success)
                return BadRequest("User is already a member of this project");

            return Ok(new { message = "Member added successfully" });
        }

        [HttpDelete("{id}/members/{userId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> RemoveProjectMember(int id, int userId)
        {
            var canUpdate = await CanUserModifyProject(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _projectService.RemoveProjectMemberAsync(id, userId, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "Member removed successfully" });
        }

        [HttpPut("{id}/members/{userId}/role")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> UpdateProjectMemberRole(int id, int userId, [FromBody] UpdateMemberRoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var canUpdate = await CanUserModifyProject(id);
            if (!canUpdate)
                return Forbid();

            var currentUserId = GetCurrentUserId();
            var success = await _projectService.UpdateProjectMemberRoleAsync(id, userId, roleDto.Role, currentUserId);
            
            if (!success)
                return NotFound();

            return Ok(new { message = "Member role updated successfully" });
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

        private async Task<bool> CanUserModifyProject(int projectId)
        {
            var currentUserRole = GetCurrentUserRole();
            var currentUserId = GetCurrentUserId();

            // Admin can modify any project
            if (currentUserRole == "Admin")
                return true;

            // Project managers can modify projects they manage
            if (currentUserRole == "ProjectManager")
            {
                var project = await _projectService.GetProjectByIdAsync(projectId);
                return project?.ProjectManagerId == currentUserId;
            }

            return false;
        }
    }

    public class AddProjectMemberDto
    {
        public int UserId { get; set; }
        public ProjectMemberRole Role { get; set; }
    }

    public class UpdateMemberRoleDto
    {
        public ProjectMemberRole Role { get; set; }
    }
}
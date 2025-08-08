using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _unitOfWork.ProjectRepository.GetAllAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _unitOfWork.ProjectRepository.GetByIdAsync(id);
        }

        public async Task<Project?> GetProjectWithDetailsAsync(int id)
        {
            return await _unitOfWork.ProjectRepository.GetProjectWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(int userId)
        {
            return await _unitOfWork.ProjectRepository.GetUserProjectsAsync(userId);
        }

        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
        {
            return await _unitOfWork.ProjectRepository.GetProjectsByStatusAsync(status);
        }

        public async Task<IEnumerable<Project>> GetProjectsByManagerAsync(int managerId)
        {
            return await _unitOfWork.ProjectRepository.GetProjectsByManagerAsync(managerId);
        }

        public async Task<IEnumerable<Project>> GetOverdueProjectsAsync()
        {
            return await _unitOfWork.ProjectRepository.GetOverdueProjectsAsync();
        }

        public async Task<Dictionary<ProjectStatus, int>> GetProjectStatusCountsAsync()
        {
            return await _unitOfWork.ProjectRepository.GetProjectStatusCountsAsync();
        }

        public async Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm, int? userId = null)
        {
            return await _unitOfWork.ProjectRepository.SearchProjectsAsync(searchTerm, userId);
        }

        public async Task<Project> CreateProjectAsync(CreateProjectDto projectDto, int createdBy)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Budget = projectDto.Budget,
                Priority = projectDto.Priority,
                Color = projectDto.Color,
                ProjectManagerId = projectDto.ProjectManagerId,
                Status = ProjectStatus.Planning,
                CreatedBy = createdBy.ToString(),
                UpdatedBy = createdBy.ToString()
            };

            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            // Add project manager as a member
            await AddProjectMemberAsync(project.Id, projectDto.ProjectManagerId, ProjectMemberRole.ProjectManager, createdBy);

            return project;
        }

        public async Task<Project?> UpdateProjectAsync(int id, UpdateProjectDto projectDto, int updatedBy)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null) return null;

            if (!string.IsNullOrEmpty(projectDto.Name))
                project.Name = projectDto.Name;
            if (!string.IsNullOrEmpty(projectDto.Description))
                project.Description = projectDto.Description;
            if (projectDto.StartDate.HasValue)
                project.StartDate = projectDto.StartDate.Value;
            if (projectDto.EndDate.HasValue)
                project.EndDate = projectDto.EndDate.Value;
            if (projectDto.Status.HasValue)
                project.Status = projectDto.Status.Value;
            if (projectDto.Budget.HasValue)
                project.Budget = projectDto.Budget.Value;
            if (projectDto.ActualCost.HasValue)
                project.ActualCost = projectDto.ActualCost.Value;
            if (projectDto.Priority.HasValue)
                project.Priority = projectDto.Priority.Value;
            if (!string.IsNullOrEmpty(projectDto.Color))
                project.Color = projectDto.Color;
            if (projectDto.ProjectManagerId.HasValue && projectDto.ProjectManagerId != project.ProjectManagerId)
            {
                project.ProjectManagerId = projectDto.ProjectManagerId.Value;
                // Update project manager membership
                await UpdateProjectMemberRoleAsync(project.Id, projectDto.ProjectManagerId.Value, ProjectMemberRole.ProjectManager, updatedBy);
            }

            project.UpdatedBy = updatedBy.ToString();

            await _unitOfWork.ProjectRepository.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id, int deletedBy)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null) return false;

            project.IsDeleted = true;
            project.DeletedBy = deletedBy.ToString();
            project.UpdatedBy = deletedBy.ToString();

            await _unitOfWork.ProjectRepository.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddProjectMemberAsync(int projectId, int userId, ProjectMemberRole role, int addedBy)
        {
            // Check if user is already a member
            var existingMember = await _unitOfWork.ProjectMemberRepository.GetProjectMemberAsync(projectId, userId);
            if (existingMember != null && existingMember.IsActive)
                return false;

            var projectMember = new ProjectMember
            {
                ProjectId = projectId,
                UserId = userId,
                Role = role,
                JoinedAt = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = addedBy.ToString(),
                UpdatedBy = addedBy.ToString()
            };

            await _unitOfWork.ProjectMemberRepository.AddAsync(projectMember);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveProjectMemberAsync(int projectId, int userId, int removedBy)
        {
            var projectMember = await _unitOfWork.ProjectMemberRepository.GetProjectMemberAsync(projectId, userId);
            if (projectMember == null || !projectMember.IsActive)
                return false;

            projectMember.IsActive = false;
            projectMember.UpdatedBy = removedBy.ToString();

            await _unitOfWork.ProjectMemberRepository.UpdateAsync(projectMember);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProjectMemberRoleAsync(int projectId, int userId, ProjectMemberRole newRole, int updatedBy)
        {
            var projectMember = await _unitOfWork.ProjectMemberRepository.GetProjectMemberAsync(projectId, userId);
            if (projectMember == null)
            {
                // Add as new member if doesn't exist
                return await AddProjectMemberAsync(projectId, userId, newRole, updatedBy);
            }

            projectMember.Role = newRole;
            projectMember.IsActive = true;
            projectMember.UpdatedBy = updatedBy.ToString();

            await _unitOfWork.ProjectMemberRepository.UpdateAsync(projectMember);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(int projectId)
        {
            return await _unitOfWork.ProjectMemberRepository.GetActiveProjectMembersAsync(projectId);
        }
    }
}
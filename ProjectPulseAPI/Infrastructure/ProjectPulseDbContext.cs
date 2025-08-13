using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Shared.Enums;
using ProjectPulseAPI.Shared.Helpers;
using TaskStatus = ProjectPulseAPI.Shared.Enums.TaskStatus;

namespace ProjectPulseAPI.Infrastructure
{
    public class ProjectPulseDbContext : DbContext
    {
        public ProjectPulseDbContext(DbContextOptions<ProjectPulseDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProjectActivity> ProjectActivities { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entities
            ConfigureUserEntity(modelBuilder);
            ConfigureProjectEntity(modelBuilder);
            ConfigureTaskEntity(modelBuilder);
            ConfigureProjectMemberEntity(modelBuilder);
            ConfigureTaskCommentEntity(modelBuilder);
            ConfigureNotificationEntity(modelBuilder);
            ConfigureProjectActivityEntity(modelBuilder);
            ConfigureFileEntities(modelBuilder);

            // Configure indexes
            ConfigureIndexes(modelBuilder);

            // Seed data
            SeedData(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);

                // Global query filter for soft delete (handles nullable bool)
                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureProjectEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Budget).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Color).HasMaxLength(7); // #FFFFFF format

                entity.HasOne(e => e.ProjectManager)
                      .WithMany(u => u.ManagedProjects)
                      .HasForeignKey(e => e.ProjectManagerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureTaskEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Tags).HasMaxLength(500);
                entity.Property(e => e.Progress).HasColumnType("decimal(5,2)");

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AssignedTo)
                      .WithMany(u => u.AssignedTasks)
                      .HasForeignKey(e => e.AssignedToId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ParentTask)
                      .WithMany(t => t.SubTasks)
                      .HasForeignKey(e => e.ParentTaskId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureProjectMemberEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProjectId, e.UserId }).IsUnique();

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Members)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.ProjectMemberships)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureTaskCommentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskComment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);

                entity.HasOne(e => e.Task)
                      .WithMany(t => t.Comments)
                      .HasForeignKey(e => e.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.TaskComments)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ParentComment)
                      .WithMany(c => c.Replies)
                      .HasForeignKey(e => e.ParentCommentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureNotificationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ActionUrl).HasMaxLength(500);
                entity.Property(e => e.RelatedEntityType).HasMaxLength(100);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureProjectActivityEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectActivity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.EntityType).HasMaxLength(100);
                entity.Property(e => e.OldValues).HasColumnType("nvarchar(max)");
                entity.Property(e => e.NewValues).HasColumnType("nvarchar(max)");

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Activities)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => e.IsDeleted != true);
            });
        }

        private void ConfigureFileEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectFile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.MimeType).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Files)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UploadedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.UploadedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TaskFile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.MimeType).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Task)
                      .WithMany(t => t.Files)
                      .HasForeignKey(e => e.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UploadedByUser)
                      .WithMany()
                      .HasForeignKey(e => e.UploadedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // User indexes
            modelBuilder.Entity<User>()
                .HasIndex(e => new { e.Email, e.IsDeleted });

            // Project indexes
            modelBuilder.Entity<Project>()
                .HasIndex(e => new { e.Status, e.IsDeleted });
            modelBuilder.Entity<Project>()
                .HasIndex(e => new { e.ProjectManagerId, e.IsDeleted });

            // Task indexes
            modelBuilder.Entity<UserTask>()
                .HasIndex(e => new { e.ProjectId, e.Status, e.IsDeleted });
            modelBuilder.Entity<UserTask>()
                .HasIndex(e => new { e.AssignedToId, e.Status, e.IsDeleted });
            modelBuilder.Entity<UserTask>()
                .HasIndex(e => new { e.DueDate, e.Status, e.IsDeleted });

            // Notification indexes
            modelBuilder.Entity<Notification>()
                .HasIndex(e => new { e.UserId, e.IsRead, e.IsDeleted });

            // Activity indexes
            modelBuilder.Entity<ProjectActivity>()
                .HasIndex(e => new { e.ProjectId, e.CreatedAt, e.IsDeleted });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@projectpulse.com",
                    PasswordHash = Helpers.HashPassword("Admin123!"),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Project",
                    LastName = "Manager",
                    Email = "manager@projectpulse.com",
                    PasswordHash = Helpers.HashPassword("Manager123!"),
                    Role = UserRole.ProjectManager,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Team",
                    LastName = "Member",
                    Email = "member@projectpulse.com",
                    PasswordHash = Helpers.HashPassword("Member123!"),
                    Role = UserRole.TeamMember,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                }
            );

            // Seed sample project
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "ProjectPulse Development",
                    Description = "Development of the ProjectPulse project management system",
                    StartDate = DateTime.UtcNow.Date,
                    EndDate = DateTime.UtcNow.AddMonths(3).Date,
                    Status = ProjectStatus.InProgress,
                    Priority = ProjectPriority.High,
                    Budget = 50000,
                    ActualCost = 15000,
                    ProjectManagerId = 2,
                    Color = "#3B82F6",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                }
            );

            // Seed project members
            modelBuilder.Entity<ProjectMember>().HasData(
                new ProjectMember
                {
                    Id = 1,
                    ProjectId = 1,
                    UserId = 2,
                    Role = ProjectMemberRole.ProjectManager,
                    JoinedAt = DateTime.UtcNow,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                },
                new ProjectMember
                {
                    Id = 2,
                    ProjectId = 1,
                    UserId = 3,
                    Role = ProjectMemberRole.Developer,
                    JoinedAt = DateTime.UtcNow,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                }
            );

            // Seed sample tasks
            modelBuilder.Entity<UserTask>().HasData(
                new UserTask
                {
                    Id = 1,
                    Title = "Setup Project Structure",
                    Description = "Create the initial project structure and configure the development environment",
                    Priority = TaskPriority.High,
                    Status = Shared.Enums.TaskStatus.Completed,
                    DueDate = DateTime.UtcNow.AddDays(-5),
                    EstimatedHours = 8,
                    ActualHours = 6,
                    Progress = 100,
                    ProjectId = 1,
                    AssignedToId = 3,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                },
                new UserTask
                {
                    Id = 2,
                    Title = "Implement User Authentication",
                    Description = "Develop JWT-based authentication system with role-based access control",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(3),
                    EstimatedHours = 16,
                    ActualHours = 8,
                    Progress = 60,
                    ProjectId = 1,
                    AssignedToId = 3,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                },
                new UserTask
                {
                    Id = 3,
                    Title = "Design Database Schema",
                    Description = "Create comprehensive database schema for project management system",
                    Priority = TaskPriority.Medium,
                    Status = TaskStatus.Review,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    EstimatedHours = 12,
                    ActualHours = 10,
                    Progress = 90,
                    ProjectId = 1,
                    AssignedToId = 2,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
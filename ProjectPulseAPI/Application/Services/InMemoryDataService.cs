using ProjectPulseAPI.Core.Entities;
using ProjectPulseAPI.Shared.Enums;

namespace ProjectPulseAPI.Application.Services
{
    public class InMemoryDataService
    {
        private static readonly List<User> _users = new();
        private static readonly List<Project> _projects = new();
        private static readonly List<UserTask> _tasks = new();
        private static readonly List<ProjectMember> _projectMembers = new();
        private static readonly List<Notification> _notifications = new();
        private static readonly List<TaskComment> _taskComments = new();
        private static readonly List<ProjectActivity> _projectActivities = new();

        private static int _userIdCounter = 1;
        private static int _projectIdCounter = 1;
        private static int _taskIdCounter = 1;
        private static int _projectMemberIdCounter = 1;
        private static int _notificationIdCounter = 1;
        private static int _commentIdCounter = 1;
        private static int _activityIdCounter = 1;

        static InMemoryDataService()
        {
            SeedData();
        }

        public static void SeedData()
        {
            SeedUsers();
            SeedProjects();
            SeedProjectMembers();
            SeedTasks();
            SeedNotifications();
            SeedTaskComments();
            SeedProjectActivities();
        }

        private static void SeedUsers()
        {
            _users.Clear();
            var users = new[]
            {
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567890",
                    Role = UserRole.Admin,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/john-doe.jpg",
                    LastLoginAt = DateTime.UtcNow.AddMinutes(-30),
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567891",
                    Role = UserRole.ProjectManager,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/jane-smith.jpg",
                    LastLoginAt = DateTime.UtcNow.AddHours(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Email = "mike.johnson@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567892",
                    Role = UserRole.TeamMember,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/mike-johnson.jpg",
                    LastLoginAt = DateTime.UtcNow.AddMinutes(-15),
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Sarah",
                    LastName = "Williams",
                    Email = "sarah.williams@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567893",
                    Role = UserRole.TeamMember,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/sarah-williams.jpg",
                    LastLoginAt = DateTime.UtcNow.AddHours(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "David",
                    LastName = "Brown",
                    Email = "david.brown@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567894",
                    Role = UserRole.ProjectManager,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/david-brown.jpg",
                    LastLoginAt = DateTime.UtcNow.AddMinutes(-45),
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Emily",
                    LastName = "Davis",
                    Email = "emily.davis@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567895",
                    Role = UserRole.TeamMember,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/emily-davis.jpg",
                    LastLoginAt = DateTime.UtcNow.AddHours(-3),
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Robert",
                    LastName = "Miller",
                    Email = "robert.miller@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567896",
                    Role = UserRole.TeamMember,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/robert-miller.jpg",
                    LastLoginAt = DateTime.UtcNow.AddMinutes(-90),
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Lisa",
                    LastName = "Wilson",
                    Email = "lisa.wilson@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567897",
                    Role = UserRole.Viewer,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/lisa-wilson.jpg",
                    LastLoginAt = DateTime.UtcNow.AddHours(-4),
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Tom",
                    LastName = "Anderson",
                    Email = "tom.anderson@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567898",
                    Role = UserRole.TeamMember,
                    IsActive = false,
                    ProfileImageUrl = "https://example.com/images/tom-anderson.jpg",
                    LastLoginAt = DateTime.UtcNow.AddDays(-5),
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = _userIdCounter++,
                    FirstName = "Anna",
                    LastName = "Taylor",
                    Email = "anna.taylor@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    PhoneNumber = "+1234567899",
                    Role = UserRole.ProjectManager,
                    IsActive = true,
                    ProfileImageUrl = "https://example.com/images/anna-taylor.jpg",
                    LastLoginAt = DateTime.UtcNow.AddMinutes(-10),
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    CreatedBy = "System"
                }
            };

            _users.AddRange(users);
        }

        private static void SeedProjects()
        {
            _projects.Clear();
            var projects = new[]
            {
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "E-Commerce Platform Redesign",
                    Description = "Complete redesign of the company's e-commerce platform with modern UI/UX and improved performance",
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(60),
                    Status = ProjectStatus.InProgress,
                    Budget = 150000m,
                    ActualCost = 45000m,
                    Color = "#3498db",
                    ProjectManagerId = 2,
                    Priority = ProjectPriority.High,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Mobile App Development",
                    Description = "Development of iOS and Android mobile applications for customer engagement",
                    StartDate = DateTime.UtcNow.AddDays(-20),
                    EndDate = DateTime.UtcNow.AddDays(80),
                    Status = ProjectStatus.InProgress,
                    Budget = 200000m,
                    ActualCost = 32000m,
                    Color = "#e74c3c",
                    ProjectManagerId = 5,
                    Priority = ProjectPriority.Critical,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Database Migration",
                    Description = "Migration of legacy database to modern cloud-based solution",
                    StartDate = DateTime.UtcNow.AddDays(-45),
                    EndDate = DateTime.UtcNow.AddDays(15),
                    Status = ProjectStatus.InProgress,
                    Budget = 75000m,
                    ActualCost = 68000m,
                    Color = "#2ecc71",
                    ProjectManagerId = 10,
                    Priority = ProjectPriority.High,
                    CreatedAt = DateTime.UtcNow.AddDays(-45),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Customer Support Portal",
                    Description = "Development of self-service customer support portal",
                    StartDate = DateTime.UtcNow.AddDays(-10),
                    EndDate = DateTime.UtcNow.AddDays(50),
                    Status = ProjectStatus.Planning,
                    Budget = 90000m,
                    ActualCost = 5000m,
                    Color = "#f39c12",
                    ProjectManagerId = 2,
                    Priority = ProjectPriority.Medium,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "API Integration",
                    Description = "Integration with third-party payment and shipping APIs",
                    StartDate = DateTime.UtcNow.AddDays(-60),
                    EndDate = DateTime.UtcNow.AddDays(-10),
                    Status = ProjectStatus.Completed,
                    Budget = 50000m,
                    ActualCost = 48000m,
                    Color = "#9b59b6",
                    ProjectManagerId = 5,
                    Priority = ProjectPriority.Medium,
                    CreatedAt = DateTime.UtcNow.AddDays(-60),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Security Audit",
                    Description = "Comprehensive security audit and vulnerability assessment",
                    StartDate = DateTime.UtcNow.AddDays(-15),
                    EndDate = DateTime.UtcNow.AddDays(30),
                    Status = ProjectStatus.OnHold,
                    Budget = 40000m,
                    ActualCost = 12000m,
                    Color = "#e67e22",
                    ProjectManagerId = 10,
                    Priority = ProjectPriority.Critical,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Performance Optimization",
                    Description = "Website and application performance optimization initiative",
                    StartDate = DateTime.UtcNow.AddDays(-5),
                    EndDate = DateTime.UtcNow.AddDays(40),
                    Status = ProjectStatus.InProgress,
                    Budget = 65000m,
                    ActualCost = 8000m,
                    Color = "#1abc9c",
                    ProjectManagerId = 2,
                    Priority = ProjectPriority.Medium,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Analytics Dashboard",
                    Description = "Business intelligence dashboard for executive reporting",
                    StartDate = DateTime.UtcNow.AddDays(10),
                    EndDate = DateTime.UtcNow.AddDays(70),
                    Status = ProjectStatus.Planning,
                    Budget = 120000m,
                    ActualCost = 0m,
                    Color = "#34495e",
                    ProjectManagerId = 5,
                    Priority = ProjectPriority.Low,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "DevOps Pipeline",
                    Description = "Implementation of CI/CD pipeline and infrastructure automation",
                    StartDate = DateTime.UtcNow.AddDays(-25),
                    EndDate = DateTime.UtcNow.AddDays(20),
                    Status = ProjectStatus.InProgress,
                    Budget = 80000m,
                    ActualCost = 55000m,
                    Color = "#95a5a6",
                    ProjectManagerId = 10,
                    Priority = ProjectPriority.High,
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    CreatedBy = "1"
                },
                new Project
                {
                    Id = _projectIdCounter++,
                    Name = "Training Platform",
                    Description = "Internal employee training and onboarding platform",
                    StartDate = DateTime.UtcNow.AddDays(-90),
                    EndDate = DateTime.UtcNow.AddDays(-30),
                    Status = ProjectStatus.Cancelled,
                    Budget = 100000m,
                    ActualCost = 25000m,
                    Color = "#7f8c8d",
                    ProjectManagerId = 2,
                    Priority = ProjectPriority.Low,
                    CreatedAt = DateTime.UtcNow.AddDays(-90),
                    CreatedBy = "1"
                }
            };

            _projects.AddRange(projects);
        }

        private static void SeedProjectMembers()
        {
            _projectMembers.Clear();
            var projectMembers = new[]
            {
                // E-Commerce Platform Redesign (Project 1)
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 1, UserId = 2, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-30), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-30), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 1, UserId = 3, Role = ProjectMemberRole.Developer, JoinedAt = DateTime.UtcNow.AddDays(-28), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-28), CreatedBy = "2" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 1, UserId = 4, Role = ProjectMemberRole.Designer, JoinedAt = DateTime.UtcNow.AddDays(-28), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-28), CreatedBy = "2" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 1, UserId = 6, Role = ProjectMemberRole.Tester, JoinedAt = DateTime.UtcNow.AddDays(-25), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-25), CreatedBy = "2" },

                // Mobile App Development (Project 2)
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 2, UserId = 5, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-20), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-20), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 2, UserId = 7, Role = ProjectMemberRole.Developer, JoinedAt = DateTime.UtcNow.AddDays(-18), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-18), CreatedBy = "5" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 2, UserId = 4, Role = ProjectMemberRole.Designer, JoinedAt = DateTime.UtcNow.AddDays(-18), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-18), CreatedBy = "5" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 2, UserId = 6, Role = ProjectMemberRole.Tester, JoinedAt = DateTime.UtcNow.AddDays(-15), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-15), CreatedBy = "5" },

                // Database Migration (Project 3)
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 3, UserId = 10, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-45), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-45), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 3, UserId = 3, Role = ProjectMemberRole.Developer, JoinedAt = DateTime.UtcNow.AddDays(-43), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-43), CreatedBy = "10" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 3, UserId = 7, Role = ProjectMemberRole.Developer, JoinedAt = DateTime.UtcNow.AddDays(-43), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-43), CreatedBy = "10" },

                // Customer Support Portal (Project 4)
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 4, UserId = 2, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-10), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-10), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 4, UserId = 6, Role = ProjectMemberRole.Developer, JoinedAt = DateTime.UtcNow.AddDays(-8), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-8), CreatedBy = "2" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 4, UserId = 8, Role = ProjectMemberRole.Observer, JoinedAt = DateTime.UtcNow.AddDays(-8), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-8), CreatedBy = "2" },

                // Additional members for other projects
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 5, UserId = 5, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-60), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-60), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 6, UserId = 10, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-15), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-15), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 7, UserId = 2, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-5), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-5), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 8, UserId = 5, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-2), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-2), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 9, UserId = 10, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-25), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-25), CreatedBy = "1" },
                new ProjectMember { Id = _projectMemberIdCounter++, ProjectId = 10, UserId = 2, Role = ProjectMemberRole.ProjectManager, JoinedAt = DateTime.UtcNow.AddDays(-90), IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-90), CreatedBy = "1" }
            };

            _projectMembers.AddRange(projectMembers);
        }

        private static void SeedTasks()
        {
            _tasks.Clear();
            var tasks = new[]
            {
                // E-Commerce Platform Redesign Tasks
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Design Homepage Mockups",
                    Description = "Create wireframes and mockups for the new homepage design",
                    Priority = TaskPriority.High,
                    Status = Shared.Enums.TaskStatus.Completed,
                    DueDate = DateTime.UtcNow.AddDays(-5),
                    EstimatedHours = 16,
                    ActualHours = 14,
                    ProjectId = 1,
                    Tags = "design,homepage,ui",
                    AssignedToId = 4,
                    Progress = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    CreatedBy = "2"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Implement User Authentication",
                    Description = "Develop secure user authentication system with JWT tokens",
                    Priority = TaskPriority.Critical,
                    Status = Shared.Enums.TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(10),
                    EstimatedHours = 32,
                    ActualHours = 18,
                    ProjectId = 1,
                    Tags = "backend,security,authentication",
                    AssignedToId = 3,
                    Progress = 60,
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    CreatedBy = "2"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Setup Product Catalog",
                    Description = "Create product catalog structure and database schema",
                    Priority = TaskPriority.Medium,
                    Status = Shared.Enums.TaskStatus.Review,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    EstimatedHours = 24,
                    ActualHours = 22,
                    ProjectId = 1,
                    Tags = "database,catalog,products",
                    AssignedToId = 3,
                    Progress = 85,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "2"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Payment Gateway Integration",
                    Description = "Integrate Stripe payment gateway for secure transactions",
                    Priority = TaskPriority.High,
                    Status = Shared.Enums.TaskStatus.ToDo,
                    DueDate = DateTime.UtcNow.AddDays(15),
                    EstimatedHours = 20,
                    ActualHours = 0,
                    ProjectId = 1,
                    Tags = "payment,integration,stripe",
                    AssignedToId = 6,
                    Progress = 0,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    CreatedBy = "2"
                },

                // Mobile App Development Tasks
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "iOS App Architecture",
                    Description = "Design and implement iOS app architecture using SwiftUI",
                    Priority = TaskPriority.Critical,
                    Status = Shared.Enums.TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(20),
                    EstimatedHours = 40,
                    ActualHours = 25,
                    ProjectId = 2,
                    Tags = "ios,swift,architecture",
                    AssignedToId = 7,
                    Progress = 65,
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    CreatedBy = "5"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Android App Development",
                    Description = "Develop Android application using Kotlin and Jetpack Compose",
                    Priority = TaskPriority.High,
                    Status = Shared.Enums.TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(25),
                    EstimatedHours = 45,
                    ActualHours = 20,
                    ProjectId = 2,
                    Tags = "android,kotlin,jetpack",
                    AssignedToId = 7,
                    Progress = 45,
                    CreatedAt = DateTime.UtcNow.AddDays(-16),
                    CreatedBy = "5"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Push Notifications Setup",
                    Description = "Implement push notification system for both iOS and Android",
                    Priority = TaskPriority.Medium,
                    Status = Shared.Enums.TaskStatus.Blocked,
                    DueDate = DateTime.UtcNow.AddDays(30),
                    EstimatedHours = 16,
                    ActualHours = 3,
                    ProjectId = 2,
                    Tags = "notifications,firebase,push",
                    AssignedToId = 6,
                    Progress = 20,
                    CreatedAt = DateTime.UtcNow.AddDays(-14),
                    CreatedBy = "5"
                },

                // Database Migration Tasks
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Data Migration Strategy",
                    Description = "Plan and document database migration strategy",
                    Priority = TaskPriority.Critical,
                    Status = Shared.Enums.TaskStatus.Completed,
                    DueDate = DateTime.UtcNow.AddDays(-20),
                    EstimatedHours = 12,
                    ActualHours = 15,
                    ProjectId = 3,
                    Tags = "migration,planning,documentation",
                    AssignedToId = 10,
                    Progress = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-40),
                    CreatedBy = "10"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Schema Conversion",
                    Description = "Convert legacy database schema to new cloud format",
                    Priority = TaskPriority.High,
                    Status = Shared.Enums.TaskStatus.InProgress,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    EstimatedHours = 30,
                    ActualHours = 28,
                    ProjectId = 3,
                    Tags = "schema,conversion,cloud",
                    AssignedToId = 3,
                    Progress = 90,
                    CreatedAt = DateTime.UtcNow.AddDays(-35),
                    CreatedBy = "10"
                },
                new UserTask
                {
                    Id = _taskIdCounter++,
                    Title = "Performance Testing",
                    Description = "Test performance of migrated database systems",
                    Priority = TaskPriority.Medium,
                    Status = Shared.Enums.TaskStatus.OnHold,
                    DueDate = DateTime.UtcNow.AddDays(8),
                    EstimatedHours = 20,
                    ActualHours = 0,
                    ProjectId = 3,
                    Tags = "testing,performance,migration",
                    AssignedToId = 7,
                    Progress = 0,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    CreatedBy = "10"
                }
            };

            _tasks.AddRange(tasks);
        }

        private static void SeedNotifications()
        {
            _notifications.Clear();
            var notifications = new[]
            {
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 3,
                    Title = "New Task Assigned",
                    Message = "You have been assigned to task: Implement User Authentication",
                    Type = NotificationType.TaskAssigned,
                    IsRead = false,
                    ActionUrl = "/tasks/2",
                    RelatedEntityId = 2,
                    RelatedEntityType = "Task",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    CreatedBy = "2"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 4,
                    Title = "Task Completed",
                    Message = "Your task 'Design Homepage Mockups' has been marked as completed",
                    Type = NotificationType.TaskCompleted,
                    IsRead = true,
                    ReadAt = DateTime.UtcNow.AddHours(-1),
                    ActionUrl = "/tasks/1",
                    RelatedEntityId = 1,
                    RelatedEntityType = "Task",
                    CreatedAt = DateTime.UtcNow.AddHours(-3),
                    CreatedBy = "2"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 7,
                    Title = "Project Updated",
                    Message = "Mobile App Development project has been updated",
                    Type = NotificationType.ProjectUpdated,
                    IsRead = false,
                    ActionUrl = "/projects/2",
                    RelatedEntityId = 2,
                    RelatedEntityType = "Project",
                    CreatedAt = DateTime.UtcNow.AddHours(-4),
                    CreatedBy = "5"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 6,
                    Title = "Task Due Soon",
                    Message = "Task 'Push Notifications Setup' is due in 2 days",
                    Type = NotificationType.TaskDueSoon,
                    IsRead = false,
                    ActionUrl = "/tasks/7",
                    RelatedEntityId = 7,
                    RelatedEntityType = "Task",
                    CreatedAt = DateTime.UtcNow.AddHours(-6),
                    CreatedBy = "System"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 2,
                    Title = "New Comment",
                    Message = "Mike Johnson commented on task: Setup Product Catalog",
                    Type = NotificationType.CommentAdded,
                    IsRead = true,
                    ReadAt = DateTime.UtcNow.AddMinutes(-30),
                    ActionUrl = "/tasks/3",
                    RelatedEntityId = 3,
                    RelatedEntityType = "Task",
                    CreatedAt = DateTime.UtcNow.AddHours(-8),
                    CreatedBy = "3"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 5,
                    Title = "Project Member Added",
                    Message = "Emily Davis has been added to Mobile App Development project",
                    Type = NotificationType.ProjectMemberAdded,
                    IsRead = false,
                    ActionUrl = "/projects/2",
                    RelatedEntityId = 2,
                    RelatedEntityType = "Project",
                    CreatedAt = DateTime.UtcNow.AddHours(-10),
                    CreatedBy = "1"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 10,
                    Title = "Status Changed",
                    Message = "Task 'Performance Testing' status changed to On Hold",
                    Type = NotificationType.StatusChanged,
                    IsRead = false,
                    ActionUrl = "/tasks/10",
                    RelatedEntityId = 10,
                    RelatedEntityType = "Task",
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    CreatedBy = "10"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 3,
                    Title = "Deadline Reminder",
                    Message = "Task 'Schema Conversion' is due tomorrow",
                    Type = NotificationType.DeadlineReminder,
                    IsRead = false,
                    ActionUrl = "/tasks/9",
                    RelatedEntityId = 9,
                    RelatedEntityType = "Task",
                    CreatedAt = DateTime.UtcNow.AddHours(-14),
                    CreatedBy = "System"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 4,
                    Title = "File Uploaded",
                    Message = "New design file uploaded to E-Commerce Platform Redesign project",
                    Type = NotificationType.FileUploaded,
                    IsRead = true,
                    ReadAt = DateTime.UtcNow.AddHours(-10),
                    ActionUrl = "/projects/1/files",
                    RelatedEntityId = 1,
                    RelatedEntityType = "Project",
                    CreatedAt = DateTime.UtcNow.AddHours(-16),
                    CreatedBy = "4"
                },
                new Notification
                {
                    Id = _notificationIdCounter++,
                    UserId = 8,
                    Title = "Project Invitation",
                    Message = "You have been invited to join Customer Support Portal project",
                    Type = NotificationType.ProjectInvitation,
                    IsRead = false,
                    ActionUrl = "/projects/4",
                    RelatedEntityId = 4,
                    RelatedEntityType = "Project",
                    CreatedAt = DateTime.UtcNow.AddHours(-18),
                    CreatedBy = "2"
                }
            };

            _notifications.AddRange(notifications);
        }

        private static void SeedTaskComments()
        {
            _taskComments.Clear();
            var comments = new[]
            {
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 2,
                    UserId = 2,
                    Content = "Great progress on the authentication system! Make sure to include 2FA support.",
                    CreatedAt = DateTime.UtcNow.AddHours(-6),
                    CreatedBy = "2"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 2,
                    UserId = 3,
                    Content = "I'm working on integrating OAuth providers as well. Should be ready for review by tomorrow.",
                    ParentCommentId = 1,
                    CreatedAt = DateTime.UtcNow.AddHours(-5),
                    CreatedBy = "3"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 3,
                    UserId = 3,
                    Content = "The catalog structure is almost complete. Need to add indexing for better performance.",
                    CreatedAt = DateTime.UtcNow.AddHours(-8),
                    CreatedBy = "3"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 5,
                    UserId = 5,
                    Content = "The iOS architecture is coming along well. Swift UI is making the development much faster.",
                    CreatedAt = DateTime.UtcNow.AddHours(-10),
                    CreatedBy = "5"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 5,
                    UserId = 7,
                    Content = "Agreed! The declarative syntax is much cleaner. I've completed the navigation structure.",
                    CreatedAt = DateTime.UtcNow.AddHours(-9),
                    CreatedBy = "7"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 7,
                    UserId = 6,
                    Content = "I'm blocked on this task. The Firebase configuration seems to be conflicting with the existing setup.",
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    CreatedBy = "6"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 7,
                    UserId = 5,
                    Content = "Let me help you with that. I'll review the configuration and get back to you.",
                    ParentCommentId = 6,
                    CreatedAt = DateTime.UtcNow.AddHours(-11),
                    CreatedBy = "5"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 9,
                    UserId = 10,
                    Content = "The schema conversion is 90% complete. Just need to handle the legacy datetime formats.",
                    CreatedAt = DateTime.UtcNow.AddHours(-14),
                    CreatedBy = "10"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 9,
                    UserId = 3,
                    Content = "I've created a utility function to handle the datetime conversion. It's ready for testing.",
                    CreatedAt = DateTime.UtcNow.AddHours(-13),
                    CreatedBy = "3"
                },
                new TaskComment
                {
                    Id = _commentIdCounter++,
                    TaskId = 1,
                    UserId = 2,
                    Content = "Excellent work on the homepage mockups! The design perfectly captures our vision.",
                    CreatedAt = DateTime.UtcNow.AddHours(-16),
                    CreatedBy = "2"
                }
            };

            _taskComments.AddRange(comments);
        }

        private static void SeedProjectActivities()
        {
            _projectActivities.Clear();
            var activities = new[]
            {
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 1,
                    UserId = 2,
                    ActivityType = ActivityType.ProjectCreated,
                    Description = "E-Commerce Platform Redesign project was created",
                    EntityType = "Project",
                    EntityId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    CreatedBy = "2"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 1,
                    UserId = 2,
                    ActivityType = ActivityType.MemberAdded,
                    Description = "Mike Johnson was added to the project as Developer",
                    EntityType = "ProjectMember",
                    EntityId = 3,
                    CreatedAt = DateTime.UtcNow.AddDays(-28),
                    CreatedBy = "2"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 1,
                    UserId = 2,
                    ActivityType = ActivityType.TaskCreated,
                    Description = "Task 'Design Homepage Mockups' was created",
                    EntityType = "Task",
                    EntityId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    CreatedBy = "2"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 1,
                    UserId = 4,
                    ActivityType = ActivityType.TaskCompleted,
                    Description = "Task 'Design Homepage Mockups' was completed",
                    EntityType = "Task",
                    EntityId = 1,
                    OldValues = "{\"Status\": \"InProgress\", \"Progress\": 85}",
                    NewValues = "{\"Status\": \"Completed\", \"Progress\": 100}",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    CreatedBy = "4"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 2,
                    UserId = 5,
                    ActivityType = ActivityType.ProjectCreated,
                    Description = "Mobile App Development project was created",
                    EntityType = "Project",
                    EntityId = 2,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    CreatedBy = "5"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 2,
                    UserId = 5,
                    ActivityType = ActivityType.TaskAssigned,
                    Description = "Task 'iOS App Architecture' was assigned to Robert Miller",
                    EntityType = "Task",
                    EntityId = 5,
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    CreatedBy = "5"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 3,
                    UserId = 10,
                    ActivityType = ActivityType.ProjectUpdated,
                    Description = "Database Migration project budget was updated",
                    EntityType = "Project",
                    EntityId = 3,
                    OldValues = "{\"Budget\": 70000, \"ActualCost\": 50000}",
                    NewValues = "{\"Budget\": 75000, \"ActualCost\": 68000}",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "10"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 1,
                    UserId = 3,
                    ActivityType = ActivityType.StatusChanged,
                    Description = "Task 'Setup Product Catalog' status changed to Review",
                    EntityType = "Task",
                    EntityId = 3,
                    OldValues = "{\"Status\": \"InProgress\"}",
                    NewValues = "{\"Status\": \"Review\"}",
                    CreatedAt = DateTime.UtcNow.AddHours(-4),
                    CreatedBy = "3"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 2,
                    UserId = 6,
                    ActivityType = ActivityType.CommentAdded,
                    Description = "Comment added to task 'Push Notifications Setup'",
                    EntityType = "TaskComment",
                    EntityId = 6,
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    CreatedBy = "6"
                },
                new ProjectActivity
                {
                    Id = _activityIdCounter++,
                    ProjectId = 1,
                    UserId = 4,
                    ActivityType = ActivityType.FileUploaded,
                    Description = "Design mockup files uploaded to project",
                    EntityType = "ProjectFile",
                    EntityId = 1,
                    CreatedAt = DateTime.UtcNow.AddHours(-16),
                    CreatedBy = "4"
                }
            };

            _projectActivities.AddRange(activities);
        }

        // Public methods to access data
        public static IEnumerable<User> GetUsers() => _users.AsReadOnly();
        public static IEnumerable<Project> GetProjects() => _projects.AsReadOnly();
        public static IEnumerable<UserTask> GetTasks() => _tasks.AsReadOnly();
        public static IEnumerable<ProjectMember> GetProjectMembers() => _projectMembers.AsReadOnly();
        public static IEnumerable<Notification> GetNotifications() => _notifications.AsReadOnly();
        public static IEnumerable<TaskComment> GetTaskComments() => _taskComments.AsReadOnly();
        public static IEnumerable<ProjectActivity> GetProjectActivities() => _projectActivities.AsReadOnly();

        // CRUD operations for Users
        public static User? GetUser(int id) => _users.FirstOrDefault(u => u.Id == id);
        public static User AddUser(User user)
        {
            user.Id = _userIdCounter++;
            user.CreatedAt = DateTime.UtcNow;
            _users.Add(user);
            return user;
        }
        public static User? UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                var index = _users.IndexOf(existingUser);
                _users[index] = user;
                return user;
            }
            return null;
        }
        public static bool DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
                return true;
            }
            return false;
        }

        // CRUD operations for Projects
        public static Project? GetProject(int id) => _projects.FirstOrDefault(p => p.Id == id);
        public static Project AddProject(Project project)
        {
            project.Id = _projectIdCounter++;
            project.CreatedAt = DateTime.UtcNow;
            _projects.Add(project);
            return project;
        }
        public static Project? UpdateProject(Project project)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject != null)
            {
                var index = _projects.IndexOf(existingProject);
                _projects[index] = project;
                return project;
            }
            return null;
        }
        public static bool DeleteProject(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                _projects.Remove(project);
                return true;
            }
            return false;
        }

        // CRUD operations for Tasks
        public static UserTask? GetTask(int id) => _tasks.FirstOrDefault(t => t.Id == id);
        public static UserTask AddTask(UserTask task)
        {
            task.Id = _taskIdCounter++;
            task.CreatedAt = DateTime.UtcNow;
            _tasks.Add(task);
            return task;
        }
        public static UserTask? UpdateTask(UserTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                var index = _tasks.IndexOf(existingTask);
                _tasks[index] = task;
                return task;
            }
            return null;
        }
        public static bool DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            return false;
        }

        // CRUD operations for Notifications
        public static Notification? GetNotification(int id) => _notifications.FirstOrDefault(n => n.Id == id);
        public static Notification AddNotification(Notification notification)
        {
            notification.Id = _notificationIdCounter++;
            notification.CreatedAt = DateTime.UtcNow;
            _notifications.Add(notification);
            return notification;
        }
        public static Notification? UpdateNotification(Notification notification)
        {
            var existingNotification = _notifications.FirstOrDefault(n => n.Id == notification.Id);
            if (existingNotification != null)
            {
                var index = _notifications.IndexOf(existingNotification);
                _notifications[index] = notification;
                return notification;
            }
            return null;
        }
        public static bool DeleteNotification(int id)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == id);
            if (notification != null)
            {
                _notifications.Remove(notification);
                return true;
            }
            return false;
        }

        // CRUD operations for Task Comments
        public static TaskComment? GetTaskComment(int id) => _taskComments.FirstOrDefault(c => c.Id == id);
        public static TaskComment AddTaskComment(TaskComment comment)
        {
            comment.Id = _commentIdCounter++;
            comment.CreatedAt = DateTime.UtcNow;
            _taskComments.Add(comment);
            return comment;
        }
        public static TaskComment? UpdateTaskComment(TaskComment comment)
        {
            var existingComment = _taskComments.FirstOrDefault(c => c.Id == comment.Id);
            if (existingComment != null)
            {
                var index = _taskComments.IndexOf(existingComment);
                _taskComments[index] = comment;
                return comment;
            }
            return null;
        }
        public static bool DeleteTaskComment(int id)
        {
            var comment = _taskComments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _taskComments.Remove(comment);
                return true;
            }
            return false;
        }

        // CRUD operations for Project Activities
        public static ProjectActivity? GetProjectActivity(int id) => _projectActivities.FirstOrDefault(a => a.Id == id);
        public static ProjectActivity AddProjectActivity(ProjectActivity activity)
        {
            activity.Id = _activityIdCounter++;
            activity.CreatedAt = DateTime.UtcNow;
            _projectActivities.Add(activity);
            return activity;
        }

        // CRUD operations for Project Members
        public static ProjectMember? GetProjectMember(int id) => _projectMembers.FirstOrDefault(m => m.Id == id);
        public static ProjectMember AddProjectMember(ProjectMember member)
        {
            member.Id = _projectMemberIdCounter++;
            member.CreatedAt = DateTime.UtcNow;
            _projectMembers.Add(member);
            return member;
        }
        public static ProjectMember? UpdateProjectMember(ProjectMember member)
        {
            var existingMember = _projectMembers.FirstOrDefault(m => m.Id == member.Id);
            if (existingMember != null)
            {
                var index = _projectMembers.IndexOf(existingMember);
                _projectMembers[index] = member;
                return member;
            }
            return null;
        }
        public static bool DeleteProjectMember(int id)
        {
            var member = _projectMembers.FirstOrDefault(m => m.Id == id);
            if (member != null)
            {
                _projectMembers.Remove(member);
                return true;
            }
            return false;
        }
    }
}
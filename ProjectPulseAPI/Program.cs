using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectPulseAPI.Application.Interfaces;
using ProjectPulseAPI.Application.Services;
using ProjectPulseAPI.Core.Persistence.Repository;
using ProjectPulseAPI.Core.Persistence.UnitOfWork;
using ProjectPulseAPI.Infrastructure;
using ProjectPulseAPI.Middleware;
using ProjectPulseAPI.Shared.Services;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework
builder.Services.AddDbContext<ProjectPulseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Unit of Work pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Repository pattern
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IProjectActivityRepository, ProjectActivityRepository>();

// Add JWT Token Service
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

// Add Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProjectActivityService, ProjectActivityService>();

// Add JWT Authentication with RS256
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    
    // Configure the token validation parameters to be set dynamically
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "ProjectPulseAPI",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "ProjectPulseAPI",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RequireExpirationTime = true,
        RequireSignedTokens = true,
        
        // Set a dummy key that will be replaced in the event handler
        IssuerSigningKey = new SymmetricSecurityKey(new byte[32])
    };
    
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            // Get the JWT service and set the correct RSA key for validation
            var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtTokenService>();
            var rsaKey = jwtService.GetRSAKey();
            context.Properties.Items["rsa_key"] = Convert.ToBase64String(rsaKey.ExportRSAPublicKey());
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            // Set the correct RSA key before token validation
            var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtTokenService>();
            var rsaKey = jwtService.GetRSAKey();
            context.Options.TokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(rsaKey);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Unauthorized\",\"message\":\"Invalid or missing JWT token\"}");
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Forbidden\",\"message\":\"Access denied\"}");
        }
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProjectPulse API",
        Version = "v1",
        Description = "A comprehensive project management system API"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure Swagger (available in all environments for API testing)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectPulse API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    c.DocumentTitle = "ProjectPulse API Documentation";
    c.DefaultModelsExpandDepth(-1); // Hide models section by default
});

app.UseHttpsRedirection();

// Add Global Exception Handler after Swagger but before other middleware
app.UseGlobalExceptionHandler();

// Enable CORS
app.UseCors("AllowAll");

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProjectPulseDbContext>();
    try
    {
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.Run();
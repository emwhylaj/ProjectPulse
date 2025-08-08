using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ProjectPulseAPI.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Don't handle exceptions for swagger endpoints
                if (context.Request.Path.StartsWithSegments("/swagger") || 
                    context.Request.Path.StartsWithSegments("/_framework") ||
                    context.Request.Path.StartsWithSegments("/_vs"))
                {
                    throw; // Re-throw to let default handling occur
                }

                _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;
            var errorResponse = new ErrorResponse();

            switch (exception)
            {
                case ArgumentException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorType = "BadRequest";
                    break;

                case KeyNotFoundException:
                case FileNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = exception.Message;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorType = "NotFound";
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Message = "Unauthorized access";
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorType = "Unauthorized";
                    break;

                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = exception.Message;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorType = "InvalidOperation";
                    break;

                case TimeoutException:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    errorResponse.Message = "Request timeout";
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorType = "Timeout";
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An internal server error occurred";
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorType = "InternalServerError";
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, options);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string ErrorType { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Path { get; set; }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
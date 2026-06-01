using System.Net;
using System.Text.Json;
<<<<<<< HEAD
using System.Collections.Generic;
using TeleCare.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TeleCare.Exceptions
{
    // Global exception middleware that catches all unhandled exceptions and returns standardized error responses.
    // This eliminates the need for local try-catch blocks in controllers.
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
=======
 
namespace TeleCare.Exceptions
{
    /// <summary>
    /// Custom exception for bad requests (400)
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
 
    /// <summary>
    /// Custom exception for not found errors (404)
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
 
    /// <summary>
    /// Custom exception for unauthorized errors (401)
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
 
    /// <summary>
    /// Custom exception for forbidden access errors (403)
    /// </summary>
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
 
    /// <summary>
    /// Custom exception for conflict errors (409) e.g. duplicate entries
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
 
    /// <summary>
    /// Global exception handling middleware
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
 
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
>>>>>>> 1c322d2759f0ac9764e2db63dbdaa7c2553105a2
        }
 
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
<<<<<<< HEAD
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found");
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ApiConstants.NotFound);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument provided");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ApiConstants.InvalidRequest);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ApiConstants.InvalidRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ApiConstants.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Status = (int)statusCode,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    // Standard error response format for all API errors.
    public class ErrorResponse
    {
        public int Status { get; set; }
        public required string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
=======
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
 
        private Task HandleException(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = ex.Message;
 
            if (ex is NotFoundException)
                statusCode = HttpStatusCode.NotFound;
            else if (ex is BadRequestException)
                statusCode = HttpStatusCode.BadRequest;
            else if (ex is UnauthorizedException)
                statusCode = HttpStatusCode.Unauthorized;
            else if (ex is ForbiddenException)
                statusCode = HttpStatusCode.Forbidden;
            else if (ex is ConflictException)
                statusCode = HttpStatusCode.Conflict;
 
            var result = JsonSerializer.Serialize(new
            {
                status = (int)statusCode,
                message
            });
 
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
 
            return context.Response.WriteAsync(result);
        }
    }
}
 
 
>>>>>>> 1c322d2759f0ac9764e2db63dbdaa7c2553105a2

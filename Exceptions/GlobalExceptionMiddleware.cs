using System.Net;
using System.Text.Json;
 
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
        }
 
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
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
 
 

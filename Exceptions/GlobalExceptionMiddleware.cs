using System.Net;
using System.Text.Json;
using TeleCare.Constants;

namespace TeleCare.Exceptions
{
    
    // Global exception middleware that catches all unhandled exceptions and returns standardized error responses.
    // This eliminates the need for local try-catch blocks in controllers.
   
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found");
                await HandleException(context, HttpStatusCode.NotFound, ApiConstants.NotFound);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument provided");
                await HandleException(context, HttpStatusCode.BadRequest, ApiConstants.InvalidRequest);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation");
                await HandleException(context, HttpStatusCode.BadRequest, ApiConstants.InvalidRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleException(context, HttpStatusCode.InternalServerError, ApiConstants.InternalServerError);
            }
        }

        private static async Task HandleException(HttpContext context, HttpStatusCode statusCode, string message)
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
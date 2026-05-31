using System.Net;
using System.Text.Json;

namespace TeleCare.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public GlobalExceptionMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal server error";

            // ✅ Handle based on exception type
            if (exception is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode,
                message
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
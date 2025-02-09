namespace Products.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ArgumentNullException => new { Message = "Invalid input.", Details = exception.Message },
                ArgumentOutOfRangeException => new { Message = "Invalid input.", Details = exception.Message },
                ArgumentException => new { Message = "Invalid input.", Details = exception.Message },
                KeyNotFoundException => new { Message = "Resource not found.", Details = exception.Message },
                InvalidOperationException => new { Message = "Invalid operation.", Details = exception.Message },
                _ => new { Message = "An unexpected error occurred.", Details = exception.Message }
            };

            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}

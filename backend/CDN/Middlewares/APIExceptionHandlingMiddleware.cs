using CDN.Exceptions;
using Newtonsoft.Json;

namespace CDN.Middlewares
{
    public class APIExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public APIExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<APIExceptionHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseAPIException ex)
            {
                string message = ex.Message;
                context.Response.StatusCode = 400;
                if (ex is UnauthorizedException)
                {
                    context.Response.StatusCode = 401;
                }
                if (ex is ResourceNotFoundException)
                {
                    context.Response.StatusCode = 404;
                }
                _logger.LogWarning($"Encountered API error, message: {message}");
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message }));
            }
        }
    }
}

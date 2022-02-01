namespace Zropbox.Middlewares
{
    public class HeaderMiddleware
    {

        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("zropbox_access_token"))
            {
                context.Request.Headers["Authorization"] = "Bearer " + context.Request.Cookies["zropbox_access_token"];
            }
            await _next(context);
        }
    }
}

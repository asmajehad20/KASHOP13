namespace KASHOP13.PL.Middleware
{
    public static class CustomeMiddlewareExtentions
    {
        public static IApplicationBuilder UseCustomeMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomeMiddleware>();
        }
    }
    public class CustomeMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("processing request");
            await _next(context);
            Console.WriteLine("processing response");
        }
    }
}

namespace User.Api.Middlewares
{
    public class RequestUserMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? id = context?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value;

            if (string.IsNullOrEmpty(id))
                id = context?.User?.Claims?.FirstOrDefault(x => x.Type == "userid")?.Value;

            context.Items["UserId"] = id;

            await _next(context);
        }
    }
}

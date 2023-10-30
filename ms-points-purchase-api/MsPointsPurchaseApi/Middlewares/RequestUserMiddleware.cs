namespace MsPointsPurchaseApi.Middlewares
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
            context.Items["UserId"] = context?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value;            

            await _next(context);
        }
    }
}

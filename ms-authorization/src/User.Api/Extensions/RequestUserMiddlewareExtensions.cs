using User.Api.Middlewares;

namespace User.Api.Extensions
{
    public static class RequestUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestUserInfo(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestUserMiddleware>();
        }
    }
}

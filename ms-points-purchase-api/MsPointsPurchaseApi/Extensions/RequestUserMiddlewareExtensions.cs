using MsPointsPurchaseApi.Middlewares;

namespace MsPointsPurchaseApi
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

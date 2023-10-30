﻿using PlatformConfiguration.Api.Middlewares;

namespace PlatformConfiguration
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

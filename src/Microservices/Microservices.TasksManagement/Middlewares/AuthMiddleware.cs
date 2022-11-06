using Authentication;
using Common.Utilities;

namespace Microservices.TasksManagement.Middlewares
{
    public static class AuthMiddleware
    {
        public static IApplicationBuilder UseAppAunthentication(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value != "/api/login")
                {
                    var jwtAuthenticationSettings = app.ApplicationServices.GetService<JwtAuthenticationSettings>();
                    var allcookies = context.Request.Cookies;
                    var accesstokenEncrypted = allcookies["access_token"];
                    if (!String.IsNullOrEmpty(accesstokenEncrypted))
                    {
                        var accesstoken = AppUtilities.DecryptString(accesstokenEncrypted, jwtAuthenticationSettings.Key);
                        context.Request.Headers["Authorization"] = "Bearer " + accesstoken;
                    }
                }
                await next(context);
            });
            return app;
        }

    }
}

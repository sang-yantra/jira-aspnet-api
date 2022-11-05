using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Authentication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            var jwtAuthSettings = configuration.GetSection("Aunthetication").Get<JwtAuthenticationSettings>();
            services.AddTransient((_) => jwtAuthSettings);


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "APP_AUTHENTICATION_SCHEME";
                options.DefaultChallengeScheme = "APP_AUTHENTICATION_SCHEME";
            })
            .AddPolicyScheme("APP_AUTHENTICATION_SCHEME", "APP_AUTHENTICATION_SCHEME", options =>
            {
                options.ForwardDefaultSelector = (context) =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer"))
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAuthSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAuthSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthSettings.Key))

                };
            });
            return services;
        }
    }
}

using Chats.Configuration;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Chats
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChatsServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            var pusherconfig = config.GetSection("PusherChat").Get<PusherConfiguration>();
            services.AddTransient((_) => pusherconfig);

            return services;
        }
    }
}
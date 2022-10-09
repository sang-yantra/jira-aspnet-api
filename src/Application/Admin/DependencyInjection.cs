using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Admin
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

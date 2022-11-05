using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Tasks
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTasksServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

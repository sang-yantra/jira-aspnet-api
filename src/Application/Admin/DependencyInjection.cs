using Admin.DAL.Implementations;
using Admin.DAL.Interfaces;
using Admin.Services.Implementations;
using Admin.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Reflection;
using FluentValidation;

namespace Admin
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            /// services DI
            services.AddScoped<ITeamService, TeamService>();


            /// DAL DI
            services.AddScoped<ITeamRepo, TeamRepo>();


            return services;
        }
    }
}

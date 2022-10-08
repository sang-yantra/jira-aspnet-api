﻿using Admin.Common.Interfaces;
using Microservice.Admin.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Jira.Supabase
{
    public static class DependencyInjecton
    {
        public static IServiceCollection AddJiraInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            /// register main app db context
            services.AddDbContext<JiraDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("JiraSupabaseDb"));
            });


            services.AddScoped<IJiraDbContext>(provider => provider.GetService<JiraDbContext>());

            return services;
        }
    }
}

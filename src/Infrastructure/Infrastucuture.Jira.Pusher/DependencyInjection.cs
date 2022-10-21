using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucuture.Jira.Pusher
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPusherServices(this IServiceCollection services, IConfiguration config)
        {


            return services;
        }
    }
}

using Microsoft.Extensions.Configuration;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucuture.Jira.Pusher
{
    public class AppSocketContext: PusherServer.Pusher
    {
        private AppSocketContextOptions options { get; set; }
        public AppSocketContext(AppSocketContextOptions options) 
            : base(appId: options.appId,
                  appKey: options.appKey,
                  appSecret: options.appSecret,
                  options: options.PusherOptions
                  )
        {

        }

    }
}

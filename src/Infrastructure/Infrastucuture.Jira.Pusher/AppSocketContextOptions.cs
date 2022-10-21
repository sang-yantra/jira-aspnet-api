using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucuture.Jira.Pusher
{
    public class AppSocketContextOptions
    {
        public string appId { get; set; }
        public string appKey { get; set; }
        public string appSecret { get; set; }
        public PusherServer.PusherOptions PusherOptions { get; set; }
    }
}

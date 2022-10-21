using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chats.Configuration
{
    public class PusherConfiguration
    {
        public string ApplicationId { get; set; }
        public string Cluster { get; set; }
        public string Secret { get; set; }
        public string Key { get; set; }
    }
}

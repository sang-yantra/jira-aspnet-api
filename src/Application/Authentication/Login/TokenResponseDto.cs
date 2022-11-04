using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Login
{
    public class TokenResponseDto
    {
        public string access_token { get; set; }
        public string refresh_token { get; set;}
        public string token_type { get; set;}      
        public double expires_in { get; set; }
    }
}

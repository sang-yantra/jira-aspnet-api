using Jira.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Login
{
    /// <summary>
    /// Claims for jwt token
    /// </summary>
    public class JwtClaim
    { 

        /// <summary>
        /// JWT token Guid
        /// </summary>
        public string Jti { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// username property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// issued at unix time
        /// </summary>
        public string Iat { get; set; }

        /// <summary>
        /// not before at unix time
        /// </summary>
        public string Nbf { get; set; }

        /// <summary>
        /// expired unix time
        /// </summary>
        public string Exp { get; set; }

        /// <summary>
        /// App version
        /// </summary>
        public string Version { get; set; }

    }
}

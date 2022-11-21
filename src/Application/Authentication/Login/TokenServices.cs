using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Login
{
    public static class TokenServices
    {
        /// <summary>
        /// Create token
        /// </summary>
        /// <param name="authclaims"></param>
        /// <param name="expiresAt"></param>
        /// <param name="jwtAuthenticationSettings"></param>
        /// <returns></returns>
        public static string CreateToken(List<Claim> authclaims, DateTime expiresAt, JwtAuthenticationSettings jwtAuthenticationSettings)
        {
            var authSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtAuthenticationSettings.Key));

            var token = new JwtSecurityToken(
                issuer: jwtAuthenticationSettings.Issuer,
                audience: jwtAuthenticationSettings.Audience,
                claims: authclaims,
                expires: expiresAt,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    authSigningKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        /// <summary>
        /// Create claims
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static List<Claim> CreateClaims(JwtClaim claim)
        {
            List<Claim> claims = new List<Claim>();
            var properties = claim.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(claim).ToString();
                Claim jwtclaim = new Claim(prop.Name, value);
                claims.Add(jwtclaim);
            }
            return claims;
        }

    }
}

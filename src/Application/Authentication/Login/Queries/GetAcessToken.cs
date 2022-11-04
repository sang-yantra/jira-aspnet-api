using Common.Interfaces;
using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Queries
{
    public class GetAccessToken : IRequest<TokenResponseDto>
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class GetAccessTokenHandler : IRequestHandler<GetAccessToken, TokenResponseDto>
    {
        private readonly IJiraDbContext _context;
        private readonly JwtAuthenticationSettings _jwtAuthSettinngs;

        public GetAccessTokenHandler(IJiraDbContext context, JwtAuthenticationSettings jwtAuthSettings)
        {
            _context = context;
            _jwtAuthSettinngs = jwtAuthSettings;
        }
        public async Task<TokenResponseDto> Handle(GetAccessToken request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(x => x.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken);

            /// handle error scenarios
            if (user == null)
            {
                throw new DataNotFoundException("User not found");
            }

            if (user.Username != request.Username || user.Password != request.Password)
            {
                List<ValidationFailure> failures = new List<ValidationFailure>();
                ValidationFailure usernamefailure = new ValidationFailure(nameof(request.Username), "Invalid username");
                ValidationFailure passwordFailure = new ValidationFailure(nameof(request.Password), "Invalid password");
                failures.Add(usernamefailure);
                failures.Add(passwordFailure);

                throw new Common.Exceptions.ValidationException(failures);
            }

            DateTime tokenIssuedAt = DateTime.UtcNow;
            long tokenIssuedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            /// @research
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(JwtRegisteredClaimNames.Iat, tokenIssuedAtUnix.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = GenerateToken(authClaims);
            var refreshToken = GenerateRefreshToken();

            AddJwtCookie();

            var tokenResponse = new TokenResponseDto()
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                refresh_token = refreshToken,
                token_type = "Bearer",
                expires_in = 60 * 60

            };
            return tokenResponse;
        }

        /// <summary>
        /// Method to generate access token
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtAuthSettinngs.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtAuthSettinngs.Issuer,
                audience: _jwtAuthSettinngs.Audience,
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    authSigningKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        /// <summary>
        /// Method to generate refresh token
        /// </summary>
        /// <returns></returns>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Add jwt cookie to the response
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void AddJwtCookie()
        {
            throw new NotImplementedException();
        }
    }


}

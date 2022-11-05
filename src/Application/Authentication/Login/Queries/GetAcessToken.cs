﻿using Common.Exceptions;
using Common.Interfaces;
using Common.Utilities;
using FluentValidation.Results;
using Jira.Domain.Entities.ProjectManagement;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
        private readonly HttpContext _httpContext;
        private readonly JwtAuthenticationSettings _jwtAuthSettinngs;
        private readonly DateTime _accessTokenExpiresAt;
        private readonly long _accessTokenExpiresAtUnix;
        private readonly DateTime _refreshTokenExpiresAt;
        private readonly long _refereshTokenExpiresAtUnix;

        public GetAccessTokenHandler(IJiraDbContext context, IHttpContextAccessor httpContext,
            JwtAuthenticationSettings jwtAuthSettings)
        {
            _context = context;
            _httpContext = httpContext.HttpContext;
            _jwtAuthSettinngs = jwtAuthSettings;
            _accessTokenExpiresAt = DateTime.UtcNow.AddHours(1);
            _refreshTokenExpiresAt = DateTime.UtcNow.AddDays(1);
            _accessTokenExpiresAtUnix = ((DateTimeOffset)_accessTokenExpiresAt).ToUnixTimeMilliseconds();
            _refereshTokenExpiresAtUnix = ((DateTimeOffset)_refreshTokenExpiresAt).ToUnixTimeMilliseconds();

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
                throw new Exception("Invalid credentials");

                List<ValidationFailure> failures = new List<ValidationFailure>();
                ValidationFailure usernamefailure = new ValidationFailure(nameof(request.Username), "Invalid username");
                ValidationFailure passwordFailure = new ValidationFailure(nameof(request.Password), "Invalid password");
                failures.Add(usernamefailure);
                failures.Add(passwordFailure);

                throw new Common.Exceptions.ValidationException(failures);
            }

            /// Get tokens deom db
            TokenResponseDto tokenResponseDto = new TokenResponseDto();
            var userToken = await _context.UserTokens.Where(x => x.UserId == user.Id)
                    .FirstOrDefaultAsync(cancellationToken);

            if (userToken == null)
            {
                tokenResponseDto = await CreateAndSaveTokenAsync(user.Id, user.Username, cancellationToken);
            }
            else
            {
                if (IsValidToken(userToken.AccessTokenExpired) && IsValidToken(userToken.RefreshTokenExpired))
                {
                    tokenResponseDto.access_token = userToken.AccessToken;
                    tokenResponseDto.refresh_token = userToken.RefreshToken;
                    AddJwtCookie(user.Username, tokenResponseDto.access_token, tokenResponseDto.refresh_token);
                    return tokenResponseDto;
                }
                tokenResponseDto = await CreateAndUpdateTokenAsync(userToken, user.Username, cancellationToken);
            }
            return tokenResponseDto;
        }

        /// <summary>
        /// Create and save access tokens and refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TokenResponseDto> CreateAndSaveTokenAsync(Guid userId, string username, CancellationToken cancellationToken)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Iat, _accessTokenExpiresAt.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            JwtSecurityToken accessJwtToken = GenerateToken(authClaims, _accessTokenExpiresAt);
            JwtSecurityToken refreshJwtToken = GenerateToken(authClaims, _refreshTokenExpiresAt);
            string accessToken = new JwtSecurityTokenHandler().WriteToken(accessJwtToken);
            string refreshToken = new JwtSecurityTokenHandler().WriteToken(refreshJwtToken);
            await SaveUserToken(userId, accessToken, _accessTokenExpiresAtUnix,
                                refreshToken, _refereshTokenExpiresAtUnix,
                                cancellationToken
                                );
            AddJwtCookie(username, accessToken, refreshToken);

            var tokenResponse = new TokenResponseDto()
            {
                access_token = accessToken,
                refresh_token = refreshToken,
            };
            return tokenResponse;
        }

        /// <summary>
        /// Create and update tokens
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="username"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TokenResponseDto> CreateAndUpdateTokenAsync(UserToken userToken, string username, CancellationToken cancellationToken)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Iat, _accessTokenExpiresAt.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken accessJwtToken = GenerateToken(authClaims, _accessTokenExpiresAt);
            JwtSecurityToken refreshJwtToken = GenerateToken(authClaims, _refreshTokenExpiresAt);
            string accessToken = new JwtSecurityTokenHandler().WriteToken(accessJwtToken);
            string refreshToken = new JwtSecurityTokenHandler().WriteToken(refreshJwtToken);
            await UpdateUserToken(userToken, accessToken, _accessTokenExpiresAtUnix,
                                refreshToken, _refereshTokenExpiresAtUnix,
                                cancellationToken
                                );
            AddJwtCookie(username, accessToken, refreshToken);

            var tokenResponse = new TokenResponseDto()
            {
                access_token = accessToken,
                refresh_token = refreshToken,
            };
            return tokenResponse;
        }

        /// <summary>
        /// Method to generate access token
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private JwtSecurityToken GenerateToken(List<Claim> authClaims, DateTime expiresAt)
        {
            var authSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtAuthSettinngs.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtAuthSettinngs.Issuer,
                audience: _jwtAuthSettinngs.Audience,
                claims: authClaims,
                expires: expiresAt,
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
        private void AddJwtCookie(string username, string accesstoken, string refreshtoken)
        {
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, username));

            var encryptAccessToken = AppUtilities.EncryptString(accesstoken, _jwtAuthSettinngs.Key);
            var encryptRefreshToken = AppUtilities.EncryptString(refreshtoken, _jwtAuthSettinngs.Key);

            var principal = new ClaimsPrincipal(identity);
            _httpContext.Response.Cookies.Append("access_token", encryptAccessToken,
                new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            _httpContext.Response.Cookies.Append("refresh_oken", encryptRefreshToken,
                new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

        }

        /// <summary>
        /// Check token is valid
        /// </summary>
        /// <param name="expiresAtUnix"></param>
        /// <returns></returns>
        private bool IsValidToken(long expiresAtUnix)
        {
            DateTimeOffset expiresAtOffset = DateTimeOffset.FromUnixTimeMilliseconds(expiresAtUnix);
            DateTime expiresAtDateTime = expiresAtOffset.DateTime;
            int comparison = DateTime.Compare(expiresAtDateTime, DateTime.UtcNow);
            return comparison > 0;
        }

        /// <summary>
        /// Save tokens in database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessTokenExpiresAtUnix"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refreshTokenExpiresAtUnix"></param>
        private async Task SaveUserToken(Guid userId,
            string accessToken, long accessTokenExpiresAtUnix,
            string refreshToken, long refreshTokenExpiresAtUnix,
            CancellationToken cancellationToken
            )
        {
            UserToken userToken = new UserToken()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccessToken = accessToken,
                AccessTokenExpired = accessTokenExpiresAtUnix,
                RefreshToken = refreshToken,
                RefreshTokenExpired = refreshTokenExpiresAtUnix,
            };

            await _context.UserTokens.AddAsync(userToken);
            await _context.SaveChangesAsync(cancellationToken);

        }

        /// <summary>
        /// Update user token in database
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessTokenExpiresAtUnix"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refereshTokenExpiresAtUnix"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task UpdateUserToken(UserToken userToken,
         string accessToken, long accessTokenExpiresAtUnix,
         string refreshToken, long refereshTokenExpiresAtUnix, CancellationToken cancellationToken)
        {
            userToken.AccessToken = accessToken;
            userToken.AccessTokenExpired = accessTokenExpiresAtUnix;
            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpired = refereshTokenExpiresAtUnix;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }


}

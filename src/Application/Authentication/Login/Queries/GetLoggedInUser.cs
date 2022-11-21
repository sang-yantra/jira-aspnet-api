using Common.Exceptions;
using Common.Interfaces;
using Common.Utilities;
using FluentValidation.Results;
using Jira.Domain;
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
    public class GetLoggedInUser : IRequest<LoggedInUserDto>
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class GetLoggedInUserHandler : IRequestHandler<GetLoggedInUser, LoggedInUserDto>
    {
        private readonly IJiraDbContext _context;
        private readonly HttpContext _httpContext;
        private readonly JwtAuthenticationSettings _jwtAuthSettinngs;
        private readonly DateTime _accessTokenExpiresAt;
        private readonly long _accessTokenExpiresAtUnix;
        private readonly DateTime _refreshTokenExpiresAt;
        private readonly long _refereshTokenExpiresAtUnix;
        private readonly string APP_VERSION;

        public GetLoggedInUserHandler(IJiraDbContext context, IHttpContextAccessor httpContext,
            JwtAuthenticationSettings jwtAuthSettings, AppVersion appVersion)
        {
            _context = context;
            _httpContext = httpContext.HttpContext;
            _jwtAuthSettinngs = jwtAuthSettings;
            _accessTokenExpiresAt = DateTime.UtcNow.AddHours(1);
            _refreshTokenExpiresAt = DateTime.UtcNow.AddDays(1);
            _accessTokenExpiresAtUnix = ((DateTimeOffset)_accessTokenExpiresAt).ToUnixTimeMilliseconds();
            _refereshTokenExpiresAtUnix = ((DateTimeOffset)_refreshTokenExpiresAt).ToUnixTimeMilliseconds();
            APP_VERSION = appVersion.Version;
        }
        public async Task<LoggedInUserDto> Handle(GetLoggedInUser request, CancellationToken cancellationToken)
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
            var userToken = await _context.UserTokens.Where(x => x.UserId == user.Id)
                    .FirstOrDefaultAsync(cancellationToken);

            if (userToken == null)
            {
                await CreateAndSaveTokenAsync(user, user.Username, cancellationToken);
            }
            else
            {
                if (IsValidToken(userToken.AccessTokenExpired) && IsValidToken(userToken.RefreshTokenExpired))
                {
                    AddJwtCookie(user.Username, userToken.AccessToken, userToken.RefreshToken);
                }
                else
                {
                    await CreateAndUpdateTokenAsync(user, userToken, user.Username, cancellationToken);
                }
            }
            return new LoggedInUserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Firstname = user.Firstname,
                Middlename = user.Middlename,
                Lastname = user.Lastname,
                Alias = user.Alias,
                Email = user.Email,
                Avatar = user.AvatarPath
            };
        }

        /// <summary>
        /// Create and save access tokens and refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CreateAndSaveTokenAsync(User user, string username, CancellationToken cancellationToken)
        {
            var issuedAtUnixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds().ToString();
            JwtClaim accessTokenJwtClaim = new JwtClaim()
            {
                Jti = Guid.NewGuid().ToString(),
                UserId = user.Id.ToString(),
                Name = user.Username,
                Email = user.Email,
                Iat = issuedAtUnixTime,
                Nbf = issuedAtUnixTime,
                Exp = _accessTokenExpiresAtUnix.ToString(),
                Version = APP_VERSION
            };

            JwtClaim refreshTokenJwtClaim = new JwtClaim()
            {
                Jti = Guid.NewGuid().ToString(),
                UserId = user.Id.ToString(),
                Name = user.Username,
                Email = user.Email,
                Iat = issuedAtUnixTime,
                Nbf = issuedAtUnixTime,
                Exp = _refereshTokenExpiresAtUnix.ToString(),
                Version = APP_VERSION
            };

            var accessTokenClaims = TokenServices.CreateClaims(accessTokenJwtClaim);
            var refreshTokenClaims = TokenServices.CreateClaims(refreshTokenJwtClaim);
            string accessToken = TokenServices.CreateToken(accessTokenClaims, _accessTokenExpiresAt, _jwtAuthSettinngs);
            string refreshToken = TokenServices.CreateToken(refreshTokenClaims, _refreshTokenExpiresAt, _jwtAuthSettinngs);
            await SaveUserToken(user.Id, accessToken, _accessTokenExpiresAtUnix,
                                refreshToken, _refereshTokenExpiresAtUnix,
                                cancellationToken
                                );
            AddJwtCookie(username, accessToken, refreshToken);

        }

        /// <summary>
        /// Create and update tokens
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="username"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CreateAndUpdateTokenAsync(User user,UserToken userToken, string username, CancellationToken cancellationToken)
        {
            var issuedAtUnixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds().ToString();
            JwtClaim accessTokenJwtClaim = new JwtClaim()
            {
                Jti = Guid.NewGuid().ToString(),
                UserId = user.Id.ToString(),
                Name = user.Username,
                Email = user.Email,
                Iat = issuedAtUnixTime,
                Nbf = issuedAtUnixTime,
                Exp = _accessTokenExpiresAtUnix.ToString(),
                Version = APP_VERSION
            };

            JwtClaim refreshTokenJwtClaim = new JwtClaim()
            {
                Jti = Guid.NewGuid().ToString(),
                UserId = user.Id.ToString(),
                Name = user.Username,
                Email = user.Email,
                Iat = issuedAtUnixTime,
                Nbf = issuedAtUnixTime,
                Exp = _refereshTokenExpiresAtUnix.ToString(),
                Version = APP_VERSION
            };

            var accessTokenClaims = TokenServices.CreateClaims(accessTokenJwtClaim);
            var refreshTokenClaims = TokenServices.CreateClaims(refreshTokenJwtClaim);
            string accessToken = TokenServices.CreateToken(accessTokenClaims, _accessTokenExpiresAt, _jwtAuthSettinngs);
            string refreshToken = TokenServices.CreateToken(refreshTokenClaims, _refreshTokenExpiresAt, _jwtAuthSettinngs);
            await UpdateUserToken(userToken, accessToken, _accessTokenExpiresAtUnix,
                                refreshToken, _refereshTokenExpiresAtUnix,
                                cancellationToken
                                );
            AddJwtCookie(username, accessToken, refreshToken);
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
                new CookieOptions() { 
                    HttpOnly=false, 
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddHours(1) 
                });

            _httpContext.Response.Cookies.Append("refresh_token", encryptRefreshToken,
                new CookieOptions()
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddHours(1)
                });

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

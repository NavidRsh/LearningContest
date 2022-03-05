using LearningContest.Application.Contracts.Infrastructure;
using LearningContest.Application.Extension;
using LearningContest.Domain.Common.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Infrastructure.Auth
{
    public class JwtService : Application.Contracts.Infrastructure.IJwtService
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        
        public JwtService(IJwtTokenHandler jwtTokenHandler, IOptions<JwtIssuerOptions> jwtOptions, ILogger logger)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentException(nameof(JwtIssuerOptions));
            ThrowIfInvalidOptions(_jwtOptions);
            
            _logger = logger;
        }
        public async Task<AccessTokenDto> GenerateEncodedToken(int userId, string userName, long? customerId, string optionalIssuer = "", string optionalAudience = "", string optionalSecretKey = "")
        {
            var identity = GenerateClaimsIdentity(userId.ToString(), userName);
            var tokenExpiration = TimeSpan.FromMinutes(int.Parse(_jwtOptions.TokenExpireMins));

            var test = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Domain.Enums.User.ClaimType.UserIdClaim, userId.ToString())
            });

            var claims = new List<Claim>()
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 //identity.FindFirst(Infrastructure.Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Auth.Constants.Strings.JwtClaimIdentifiers.Id),
                 new Claim(Domain.Enums.User.ClaimType.CustomerIdClaim, customerId != null ? customerId.ToString() : ""),
                 new Claim("IsActive", "true")
             };

            SymmetricSecurityKey optionalSigningKey = null;
            if (!string.IsNullOrEmpty(optionalSecretKey))
            {
                optionalSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(optionalSecretKey));
            }
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                string.IsNullOrEmpty(optionalIssuer) ? _jwtOptions.Issuer : optionalIssuer,
                string.IsNullOrEmpty(optionalAudience) ? _jwtOptions.Audience : optionalAudience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.IssuedAt.Add(tokenExpiration),
                optionalSigningKey == null ? _jwtOptions.SigningCredentials : new Microsoft.IdentityModel.Tokens.SigningCredentials(optionalSigningKey, SecurityAlgorithms.HmacSha256));
            return new AccessTokenDto(_jwtTokenHandler.WriteToken(jwt), (int)tokenExpiration.TotalSeconds);
        }

        public string GenerateRefreshToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private static ClaimsIdentity GenerateClaimsIdentity(string id, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Domain.Enums.User.ClaimType.UserIdClaim, id)
            });
        }
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            //if (options.ValidFor <= TimeSpan.Zero)
            //{
            //    throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            //}

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

    }
}

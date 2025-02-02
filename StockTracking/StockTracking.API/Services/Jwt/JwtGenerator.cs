using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockTracking.API.Models.Domain;
using StockTracking.API.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockTracking.API.Services.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user)
        {
            try
            {
                // 1) Create Claims
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                };

                // 2) Create Key
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                var creds = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                );

                // 3) Token Expiration
                var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);

                // 4) Create the Token
                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while generating the token: {ex.Message}", ex);
            }
        }

        public IEnumerable<Claim>? ValidateToken(string token)
        {
            // 1) Initialize Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2) Get Key
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            try
            {
                // 3) Validate with Parameters
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                return principal.Claims;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

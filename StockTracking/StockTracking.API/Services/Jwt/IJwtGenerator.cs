using StockTracking.API.Models.Domain;
using System.Security.Claims;

namespace StockTracking.API.Services.Jwt
{
    /// <summary>
    /// Defines the interface for JWT generation and validation.
    /// </summary>
    public interface IJwtGenerator
    {

        string GenerateToken(User user);

        IEnumerable<Claim>? ValidateToken(string token);
    }
}

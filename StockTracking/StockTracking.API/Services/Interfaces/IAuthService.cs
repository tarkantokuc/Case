using System.Threading.Tasks;
using StockTracking.API.Models.Domain;

namespace StockTracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines the signatures for authentication-related methods.
    /// </summary>
    public interface IAuthService
    {
        Task<(bool Success, string? TokenOrMsg)> RegisterAsync(string email, string password);

        Task<(bool Success, string? TokenOrMsg)> LoginAsync(string email, string password);
    }
}

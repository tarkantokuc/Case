using System.Threading.Tasks;
using StockTracking.API.Models.DTO.Response;

namespace StockTracking.API.Services.Interfaces
{
    /// <summary>
    /// Interface for operations related to sales.
    /// </summary>
    public interface ISaleService
    {
        Task<SaleDto> AddSaleAsync(int productId, int quantity, DateTime date);
    }
}

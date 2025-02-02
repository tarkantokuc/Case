using System;
using System.Threading.Tasks;
using StockTracking.API.Models.DTO.Response;

namespace StockTracking.API.Services.Interfaces
{
    /// <summary>
    /// Interface for operations related to product supplies.
    /// </summary>
    public interface ISupplyService
    {
        Task<SupplyDto> AddSupplyAsync(int productId, int quantity, DateTime date);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockTracking.API.Models.DTO.Response;

namespace StockTracking.API.Services.Interfaces
{
    /// <summary>
    /// Interface for transaction query operations.
    /// </summary>
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync(DateTime? startDate, DateTime? endDate, int? productId = null);
    }
}

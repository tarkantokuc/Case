using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockTracking.API.Data;
using StockTracking.API.Models.DTO.Response;
using StockTracking.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracking.API.Services.Implementations
{
    /// <summary>
    /// Implementation of the transaction query service.
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly StockTrackingDbContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(StockTrackingDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(DateTime? startDate, DateTime? endDate, int? productId = null)
        {
            try
            {
                _logger.LogInformation("Fetching transactions from {StartDate} to {EndDate} for ProductId: {ProductId}", startDate, endDate, productId);

                // Query supplies
                var suppliesQuery = _context.ProductSupplies.AsQueryable();

                // Query sales
                var salesQuery = _context.ProductSales.AsQueryable();

                // Apply date filters if startDate provided
                if (startDate.HasValue)
                {
                    suppliesQuery = suppliesQuery.Where(s => s.Date >= startDate.Value);
                    salesQuery = salesQuery.Where(s => s.Date >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    suppliesQuery = suppliesQuery.Where(s => s.Date <= endDate.Value);
                    salesQuery = salesQuery.Where(s => s.Date <= endDate.Value);
                }

                // Apply productId filters if productId provided
                if (productId.HasValue)
                {
                    suppliesQuery = suppliesQuery.Where(s => s.ProductId == productId.Value);
                    salesQuery = salesQuery.Where(s => s.ProductId == productId.Value);
                }

                // Convert supplies to DTO
                var supplies = await suppliesQuery
                    .Select(s => new TransactionDto
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        TransactionType = "Supply",
                        Quantity = s.Quantity,
                        Date = s.Date
                    })
                    .ToListAsync();

                _logger.LogInformation("Supplies fetched: {Count}", supplies.Count);

                // Convert sales to DTO
                var sales = await salesQuery
                    .Select(s => new TransactionDto
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        TransactionType = "Sale",
                        Quantity = s.Quantity,
                        Date = s.Date
                    })
                    .ToListAsync();

                _logger.LogInformation("Sales fetched: {Count}", sales.Count);

                // Combine supplies and sales results and sort by date
                var transactions = supplies.Concat(sales).OrderBy(t => t.Date).ToList();

                _logger.LogInformation("Total transactions returned: {Count}", transactions.Count);

                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching transactions.");
                throw new Exception($"An error occurred while fetching transactions: {ex.Message}", ex);
            }
        }
    }
}

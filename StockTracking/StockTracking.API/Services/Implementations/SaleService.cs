using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockTracking.API.Data;
using StockTracking.API.Models.Domain;
using StockTracking.API.Models.DTO.Response;
using StockTracking.API.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracking.API.Services.Implementations
{
    /// <summary>
    /// Implementation of the sale service.
    /// </summary>
    public class SaleService : ISaleService
    {
        private readonly StockTrackingDbContext _context;
        private readonly ILogger<SaleService> _logger;

        public SaleService(StockTrackingDbContext context, ILogger<SaleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SaleDto> AddSaleAsync(int productId, int quantity, DateTime date)
        {
            _logger.LogInformation("Adding sale for ProductId: {ProductId}, Quantity: {Quantity}, Date: {Date}", productId, quantity, date);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if the product exists
                var product = await _context.Products
                    .Include(p => p.ProductSupplies)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                    throw new ArgumentException("Product not found.", nameof(productId));
                }

                // Calculate total stock
                int totalRemaining = product.ProductSupplies.Sum(s => s.RemainingQuantity);
                _logger.LogInformation("Total remaining stock for ProductId {ProductId}: {TotalRemaining}", productId, totalRemaining);

                if (quantity > totalRemaining)
                {
                    _logger.LogWarning("Insufficient stock for ProductId {ProductId}. Requested: {Quantity}, Available: {TotalRemaining}", productId, quantity, totalRemaining);
                    throw new InvalidOperationException("Insufficient stock quantity.");
                }

                int remainingToDeduct = quantity;

                // Ordered RemainingQuantity from the oldest supplies
                var supplies = product.ProductSupplies
                    .Where(s => s.RemainingQuantity > 0)
                    .OrderBy(s => s.Date)
                    .ToList();

                foreach (var supply in supplies)
                {
                    if (remainingToDeduct <= 0)
                        break;

                    int deductQuantity = Math.Min(supply.RemainingQuantity, remainingToDeduct);
                    supply.RemainingQuantity -= deductQuantity;
                    remainingToDeduct -= deductQuantity;

                    _logger.LogInformation("Deducted {DeductQuantity} from Supply ID {SupplyId}. RemainingQuantity: {RemainingQuantity}", deductQuantity, supply.Id, supply.RemainingQuantity);
                }

                // Create the sale record
                var sale = new ProductSale
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Date = date
                };

                _context.ProductSales.Add(sale);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Sale added successfully with ID {SaleId}", sale.Id);

                return new SaleDto
                {
                    Id = sale.Id,
                    ProductId = sale.ProductId,
                    Quantity = sale.Quantity,
                    Date = sale.Date
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding sale for ProductId {ProductId}", productId);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

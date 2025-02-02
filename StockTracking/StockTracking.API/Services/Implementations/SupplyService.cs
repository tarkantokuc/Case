using Microsoft.EntityFrameworkCore;
using StockTracking.API.Data;
using StockTracking.API.Models.Domain;
using StockTracking.API.Models.DTO.Response;
using StockTracking.API.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StockTracking.API.Services.Implementations
{
    /// <summary>
    /// Implementation of the product supply service.
    /// </summary>
    public class SupplyService : ISupplyService
    {
        private readonly StockTrackingDbContext _context;

        public SupplyService(StockTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<SupplyDto> AddSupplyAsync(int productId, int quantity, DateTime date)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductSupplies)
                    .FirstOrDefaultAsync(p => p.Id == productId);
                
                // Check if the product exists
                if (product == null)
                {
                    throw new ArgumentException("Product not found.", nameof(productId));
                }

                // Create a new supply record
                var productSupply = new ProductSupply
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Date = date,
                    RemainingQuantity = quantity 
                };

                _context.ProductSupplies.Add(productSupply);
                await _context.SaveChangesAsync();

                // Convert to DTO
                return new SupplyDto
                {
                    Id = productSupply.Id,
                    Quantity = productSupply.Quantity,
                    Date = productSupply.Date,
                    RemainingQuantity = productSupply.RemainingQuantity
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding supply: {ex.Message}", ex);
            }
        }
    }
}

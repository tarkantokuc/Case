using Microsoft.EntityFrameworkCore;
using StockTracking.API.Data;
using StockTracking.API.Models.Domain;
using StockTracking.API.Models.DTO.Response;
using StockTracking.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockTracking.API.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly StockTrackingDbContext _context;

        public ProductService(StockTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                var productDtos = new List<ProductDto>();

                foreach (var product in products)
                {
                    productDtos.Add(new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name
                    });
                }

                return productDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching products: {ex.Message}", ex);
            }
        }

        public async Task<ProductDto> CreateProductAsync(string name)
        {
            try
            {
                var product = new Product
                {
                    Name = name
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the product: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateProductAsync(int id, string name)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return false;

                product.Name = name;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product: {ex.Message}", ex);
            }
        }
    }
}

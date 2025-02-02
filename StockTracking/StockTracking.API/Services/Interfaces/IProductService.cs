using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockTracking.API.Models.DTO.Response;

namespace StockTracking.API.Services.Interfaces
{
    /// <summary>
    /// Interface for product-related operations.
    /// </summary>
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<ProductDto> CreateProductAsync(string name);

        Task<bool> UpdateProductAsync(int id, string name);

    }
}

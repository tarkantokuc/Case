using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.API.Models.Domain
{
    /// <summary>
    /// Represents a product.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<ProductSupply> ProductSupplies { get; set; } = new List<ProductSupply>();
        public ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();
    }
}

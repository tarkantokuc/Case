using System;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.API.Models.Domain
{
    /// <summary>
    /// Represents a product supply operation.
    /// </summary>
    public class ProductSupply
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int RemainingQuantity { get; set; }

        public Product Product { get; set; } = null!;
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.API.Models.DTO.Request
{
    /// <summary>
    /// Request DTO for creating a sale record.
    /// </summary>
    public class CreateSaleRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}

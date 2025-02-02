using System;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.API.Models.DTO.Request
{
    /// <summary>
    /// Request DTO for creating a supply record.
    /// </summary>
    public class CreateSupplyRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}

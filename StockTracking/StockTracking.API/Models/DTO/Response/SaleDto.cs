namespace StockTracking.API.Models.DTO.Response
{
    /// <summary>
    /// Data Transfer Object representing a sale operation.
    /// </summary>
    public class SaleDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}

namespace StockTracking.API.Models.DTO.Response
{
    /// <summary>
    /// Data Transfer Object representing a supply operation.
    /// </summary>
    public class SupplyDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public int RemainingQuantity { get; set; }
    }
}

namespace StockTracking.API.Models.DTO.Response
{
    /// <summary>
    /// Data Transfer Object for a transaction.
    /// </summary>
    public class TransactionDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string TransactionType { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}

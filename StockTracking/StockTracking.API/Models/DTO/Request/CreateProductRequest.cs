namespace StockTracking.API.Models.DTO.Request
{
    /// <summary>
    /// Request DTO for creating a new product.
    /// </summary>
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
    }
}

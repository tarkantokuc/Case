namespace StockTracking.API.Models.DTO.Request
{
    /// <summary>
    /// Request DTO for updating a product.
    /// </summary>
    public class UpdateProductRequest
    {
        public string Name { get; set; } = null!;
    }
}

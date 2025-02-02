namespace StockTracking.API.Models.DTO.Response
{
    /// <summary>
    /// Data Transfer Object for a product.
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

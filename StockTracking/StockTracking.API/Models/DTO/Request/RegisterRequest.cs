namespace StockTracking.API.Models.DTO.Request
{
    /// <summary>
    /// Request DTO for user registration.
    /// </summary>
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace StockTracking.API.Models.Domain
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;
    }
}

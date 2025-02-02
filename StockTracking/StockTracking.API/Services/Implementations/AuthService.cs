using Microsoft.EntityFrameworkCore;
using StockTracking.API.Data;
using StockTracking.API.Models.Domain;
using StockTracking.API.Services.Interfaces;
using StockTracking.API.Services.Jwt;
using System;
using System.Threading.Tasks;

namespace StockTracking.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly StockTrackingDbContext _context;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(StockTrackingDbContext context, IJwtGenerator jwtGenerator)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<(bool Success, string? TokenOrMsg)> RegisterAsync(string email, string password)
        {
            try
            {
                // Check if the user is already registered
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser != null)
                {
                    return (false, "This email is already registered.");
                }

                // Hash the password
                var passwordHash = HashPassword(password);

                // Create a new user
                var user = new User
                {
                    Email = email,
                    PasswordHash = passwordHash 
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Generate a JWT token 
                var token = _jwtGenerator.GenerateToken(user);
                return (true, token);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred during registration: {ex.Message}");
            }
        }

        public async Task<(bool Success, string? TokenOrMsg)> LoginAsync(string email, string password)
        {
            try
            {
                // Find the user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return (false, "User not found.");
                }

                // Verify the password
                var isPasswordValid = VerifyPassword(password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    return (false, "Incorrect email or password.");
                }

                // Generate a JWT token 
                var token = _jwtGenerator.GenerateToken(user);
                return (true, token);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred during login: {ex.Message}");
            }
        }

        // Password hashing using bcrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); 
        }

        // Password verification 
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword); 
        }
    }
}

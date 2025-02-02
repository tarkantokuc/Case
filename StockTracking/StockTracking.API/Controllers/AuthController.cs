using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockTracking.API.Models.DTO.Request;
using StockTracking.API.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StockTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)] // Successful response containing message
        [ProducesResponseType(typeof(string), 400)] // Bad request
        [ProducesResponseType(typeof(object), 500)] // Internal server error response with details
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var (success, tokenOrMsg) = await _authService.RegisterAsync(request.Email, request.Password);
                if (!success)
                    return BadRequest(tokenOrMsg);

                return Ok(new { message = "User has been successfully registered." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration.", detail = ex.Message });
            }
        }

        // POST /api/auth/login
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)] // Successful response containing token
        [ProducesResponseType(typeof(string), 401)] // Unauthorized
        [ProducesResponseType(typeof(object), 500)] // Internal server error response with details
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var (success, tokenOrMsg) = await _authService.LoginAsync(request.Email, request.Password);
                if (!success)
                    return Unauthorized(tokenOrMsg);

                return Ok(new { token = tokenOrMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login.", detail = ex.Message });
            }
        }
    }
}

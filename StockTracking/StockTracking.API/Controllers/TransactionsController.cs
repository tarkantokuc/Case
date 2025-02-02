using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockTracking.API.Models.DTO.Response;
using StockTracking.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        // GET /api/Transaction
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), 200)] // Succesful transactions response
        [ProducesResponseType(400)] // Bad request for invalid model state
        [ProducesResponseType(typeof(object), 500)] // Internal server error with details
        public async Task<IActionResult> GetTransactions(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? productId)
        {
            // Validate that the start date is not after the end date.
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                return BadRequest(new { message = "The start date cannot be after the end date." });

            try
            {
                var transactions = await _transactionService.GetTransactionsAsync(startDate, endDate, productId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching transactions.", detail = ex.Message });
            }
        }
    }
}

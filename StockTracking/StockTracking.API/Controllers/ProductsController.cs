using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockTracking.API.Models.DTO.Request;
using StockTracking.API.Models.DTO.Response;
using StockTracking.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Authorization control
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ISupplyService _supplyService;
        private readonly ISaleService _saleService;

        public ProductsController(IProductService productService, ISupplyService supplyService, ISaleService saleService)
        {
            _productService = productService;
            _supplyService = supplyService;
            _saleService = saleService;
        }

        // GET /api/Products
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)] // Successful response: list of products
        [ProducesResponseType(typeof(object), 500)] // Internal server error with details
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching products.", detail = ex.Message });
            }
        }

        // POST /api/Products
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 201)] // Successful product creation response
        [ProducesResponseType(400)]                      // Bad request for invalid model state
        [ProducesResponseType(typeof(object), 500)]      // Internal server error with details
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newProduct = await _productService.CreateProductAsync(request.Name);
                return CreatedAtAction(nameof(Create), new { id = newProduct.Id }, newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product.", detail = ex.Message });
            }
        }

        // PUT /api/Products/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]  // Successful update message
        [ProducesResponseType(typeof(string), 404)]  // Product not found
        [ProducesResponseType(400)]                   // Bad request for invalid model state
        [ProducesResponseType(typeof(object), 500)]   // Internal server error with details
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _productService.UpdateProductAsync(id, request.Name);
                if (!success)
                    return NotFound("Product not found.");

                return Ok("Product updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product.", detail = ex.Message });
            }
        }

        // POST /api/Products/{productId}/supplies
        [HttpPost("{productId}/supplies")]
        [ProducesResponseType(typeof(SupplyDto), 201)] // Successful response
        [ProducesResponseType(400)] // Bad request for invalid model state
        [ProducesResponseType(typeof(string), 404)] // Product Not found
        [ProducesResponseType(typeof(object), 500)] // Internal server error with details
        public async Task<IActionResult> AddSupply(int productId, [FromBody] CreateSupplyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var supplyDto = await _supplyService.AddSupplyAsync(productId, request.Quantity, request.Date);
                return CreatedAtAction(nameof(AddSupply), new { productId = productId, id = supplyDto.Id }, supplyDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding supply.", detail = ex.Message });
            }
        }

        // POST /api/Products/{productId}/sales
        [HttpPost("{productId}/sales")]
        [ProducesResponseType(typeof(SaleDto), 201)] // Successful response
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(typeof(object), 404)] // Product Not found
        [ProducesResponseType(typeof(object), 409)] // Conflict response
        [ProducesResponseType(typeof(object), 500)] // Internal server error with details
        public async Task<IActionResult> AddSale(int productId, [FromBody] CreateSaleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var saleDto = await _saleService.AddSaleAsync(productId, request.Quantity, request.Date);
                return CreatedAtAction(nameof(AddSale), new { productId = productId, id = saleDto.Id }, saleDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding sale.", detail = ex.Message });
            }
        }
    }
}

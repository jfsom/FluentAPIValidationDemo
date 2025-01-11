using FluentAPIValidationDemo.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FluentAPIValidationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly IValidator<Product> _validator;

        public ProductsController(ECommerceDbContext context, IValidator<Product> validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            // Validate the Product manually using FluentValidation
            var validationResult = _validator.Validate(product);

            // Custom Error Response
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }

            //If the Data is valid, add to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            // Check if the product ID in the URL matches the ID in the body
            if (id != product.ProductId)
            {
                return BadRequest(new { Error = "Product ID in the URL does not match the Product ID in the body." });
            }

            // Validate the Product using FluentValidation
            var validationResult = _validator.Validate(product);

            // Custom Error Response
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }

            // Check if the product exists in the database
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { Error = "Product not found." });
            }

            // Update the existing product with new values
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Stock = product.Stock;

            try
            {
                // Save changes to the database
                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while updating the product.", Details = ex.Message });
            }

            return Ok(existingProduct);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
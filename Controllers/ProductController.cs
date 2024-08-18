using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Interfaces.IServices;
using Product.Domain.Models;

namespace productmanagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetProducts");
                throw;
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product is null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "GetProductById");
                throw;
            }
           
            
        }

        [HttpPost]
        public async Task<ActionResult<Products>> CreateProduct(Products product)
        {
            try
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while CreateProduct");

                return StatusCode(500, new { message = "An error occurred while CreateProduct." });
            }
          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Products product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest();
                }

                 await _productService.UpdateProductAsync(product);
                return Ok(new { message = "Product updated successfully." }); ;

            }
            catch (Exception ex)
            {

                // Log the exception
                _logger.LogError(ex, "Error updating product with ID {ProductId}", id);

                return StatusCode(500, new { message = "An error occurred while updating the product." });
            }       

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok(new { message = "Product deleted successfully." });

            }
            catch (Exception ex)
            {

                // Log the exception
                _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);

                return StatusCode(500, new { message = "An error occurred while deleting the product." });
            }
         
        }
    }
}

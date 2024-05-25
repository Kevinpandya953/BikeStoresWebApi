using Microsoft.AspNetCore.Mvc;
using BikeStoresWebApi.Models;
using BikeStoresWebApi.UOW;
using Microsoft.Extensions.Logging; 
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeStoresWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger; // Inject ILogger

        public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger; // Initialize ILogger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            try
            {
                var productsRepository = _unitOfWork.GetRepository<Products>();
                var products = await productsRepository.GetAllAsync();
                _logger.LogInformation("Retrieved all products.");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            try
            {
                var productsRepository = _unitOfWork.GetRepository<Products>();
                var product = await productsRepository.GetByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Retrieved product with ID {id}.");
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving product with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Products product)
        {
            try
            {
                var productsRepository = _unitOfWork.GetRepository<Products>();
                await productsRepository.AddAsync(product);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Product added successfully.");
                return CreatedAtAction(nameof(GetProduct), new { id = product.product_id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Products product)
        {
            try
            {
                if (id != product.product_id)
                {
                    _logger.LogWarning("Product ID in the URL does not match the ID in the request body.");
                    return BadRequest("Product ID in the URL does not match the ID in the request body.");
                }

                var productsRepository = _unitOfWork.GetRepository<Products>();
                var existingProduct = await productsRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }

                productsRepository.Update(product);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Product with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var productsRepository = _unitOfWork.GetRepository<Products>();
                var existingProduct = await productsRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }

                await productsRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Product with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

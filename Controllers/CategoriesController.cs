using BikeStoresWebApi.Models;
using BikeStoresWebApi.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeStoresWebApi.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class CategoriesController : ControllerBase
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<CategoriesController> _logger; // Inject ILogger

            public CategoriesController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger; // Initialize ILogger
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
            {
                try
                {
                    var categoriesRepository = _unitOfWork.GetRepository<Categories>();
                    var categories = await categoriesRepository.GetAllAsync();
                    _logger.LogInformation("Retrieved all categories.");
                    return Ok(categories);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while retrieving categories.");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Categories>> GetCategory(int id)
            {
                try
                {
                    var categoriesRepository = _unitOfWork.GetRepository<Categories>();
                    var category = await categoriesRepository.GetByIdAsync(id);

                    if (category == null)
                    {
                        _logger.LogWarning($"Category with ID {id} not found.");
                        return NotFound();
                    }

                    _logger.LogInformation($"Retrieved category with ID {id}.");
                    return Ok(category);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while retrieving category with ID {id}.");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpPost]
            public async Task<IActionResult> PostCategory(Categories category)
            {
                try
                {
                    var categoriesRepository = _unitOfWork.GetRepository<Categories>();
                    await categoriesRepository.AddAsync(category);
                    await _unitOfWork.SaveAsync();
                    _logger.LogInformation("Category added successfully.");
                    return CreatedAtAction(nameof(GetCategory), new { id = category.category_id }, category);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while adding category.");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutCategory(int id, Categories category)
            {
                try
                {
                    if (id != category.category_id)
                    {
                        _logger.LogWarning("Category ID in the URL does not match the ID in the request body.");
                        return BadRequest("Category ID in the URL does not match the ID in the request body.");
                    }

                    var categoriesRepository = _unitOfWork.GetRepository<Categories>();
                    var existingCategory = await categoriesRepository.GetByIdAsync(id);
                    if (existingCategory == null)
                    {
                        _logger.LogWarning($"Category with ID {id} not found.");
                        return NotFound($"Category with ID {id} not found.");
                    }

                    categoriesRepository.Update(category);
                    await _unitOfWork.SaveAsync();

                    _logger.LogInformation($"Category with ID {id} updated successfully.");
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while updating category with ID {id}.");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCategory(int id)
            {
                try
                {
                    var categoriesRepository = _unitOfWork.GetRepository<Categories>();
                    var existingCategory = await categoriesRepository.GetByIdAsync(id);
                    if (existingCategory == null)
                    {
                        _logger.LogWarning($"Category with ID {id} not found.");
                        return NotFound($"Category with ID {id} not found.");
                    }

                    await categoriesRepository.RemoveAsync(id);
                    await _unitOfWork.SaveAsync();

                    _logger.LogInformation($"Category with ID {id} deleted successfully.");
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while deleting category with ID {id}.");
                    return StatusCode(500, "Internal server error");
                }
            }
        }
    
}

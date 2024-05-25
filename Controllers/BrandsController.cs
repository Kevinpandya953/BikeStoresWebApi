using BikeStoresWebApi.Models;
using BikeStoresWebApi.UOW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger
using Serilog;

namespace BikeStoresWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandsController> _logger; // Inject ILogger

        public BrandsController(IUnitOfWork unitOfWork, ILogger<BrandsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger; // Initialize ILogger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brands>>> GetBrands()
        {
            try
            {
                var brandsRepository = _unitOfWork.GetRepository<Brands>();
                var brands = await brandsRepository.GetAllAsync();
                _logger.LogInformation("Retrieved all brands.");
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving brands.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brands>> Getbrands(int id)
        {
            try
            {
                var brandsRepository = _unitOfWork.GetRepository<Brands>();
                var brand = await brandsRepository.GetByIdAsync(id);

                if (brand == null)
                {
                    _logger.LogWarning($"Brand with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Retrieved brand with ID {id}.");
                return Ok(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving brand with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostBrand(Brands brand)
        {
            try
            {
                var brandsRepository = _unitOfWork.GetRepository<Brands>();
                await brandsRepository.AddAsync(brand);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Brand added successfully.");
                return CreatedAtAction(nameof(GetBrands), new { id = brand.brand_id }, brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding brand.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brands brand)
        {
            try
            {
                if (id != brand.brand_id)
                {
                    _logger.LogWarning("Brand ID in the URL does not match the ID in the request body.");
                    return BadRequest("Brand ID in the URL does not match the ID in the request body.");
                }

                var brandsRepository = _unitOfWork.GetRepository<Brands>();
                var existingBrand = await brandsRepository.GetByIdAsync(id);
                if (existingBrand == null)
                {
                    _logger.LogWarning($"Brand with ID {id} not found.");
                    return NotFound($"Brand with ID {id} not found.");
                }

                brandsRepository.Update(brand);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Brand with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating brand with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                var brandsRepository = _unitOfWork.GetRepository<Brands>();
                var existingBrand = await brandsRepository.GetByIdAsync(id);
                if (existingBrand == null)
                {
                    _logger.LogWarning($"Brand with ID {id} not found.");
                    return NotFound($"Brand with ID {id} not found.");
                }

                await brandsRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Brand with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting brand with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}

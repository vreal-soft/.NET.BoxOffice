using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Controllers
{
    [Route("api/spectacles")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SpectacleController : ControllerBase
    {
        private readonly ISpectacleService _service;
        private readonly ILogger<SpectacleController> _logger;
        private readonly SieveProcessor _sieveProcessor;

        public SpectacleController(ISpectacleService service, ILogger<SpectacleController> logger, SieveProcessor sieveProcessor)
        {
            _service = service;
            _logger = logger;
            _sieveProcessor = sieveProcessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel query)
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new PaginatedData<SpectacleDto>(result.AsQueryable(), query, _sieveProcessor));
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status418ImATeapot, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _service.GetById(id));
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status418ImATeapot, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpectacle model)
        {
            try
            {
                return Ok(await _service.Create(model));
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status418ImATeapot, new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(SpectacleDto model)
        {
            try
            {
                return Ok(await _service.Update(model));
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status418ImATeapot, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            try
            {
                return Ok(await _service.Remove(id));
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status418ImATeapot, new { message = ex.Message });
            }
        }
    }
}

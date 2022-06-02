using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BoxOffice.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService service, ILogger<TicketController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAll());
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

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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

        [AllowAnonymous]
        [HttpGet("free-places/{spectacleId}")]
        public async Task<IActionResult> GetFreePlaces([FromRoute] int spectacleId)
        {
            try
            {
                return Ok(await _service.GetFreePlaces(spectacleId));
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

        [Authorize(Roles = "Client")]
        [HttpGet("client/all")]
        public async Task<IActionResult> GetAllInClient()
        {
            try
            {
                return Ok(await _service.GetAllInClient());
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

        [Authorize(Roles = "Admin")]
        [HttpGet("spectacle/{spectacleId}")]
        public async Task<IActionResult> GetAllInSpectacle(int spectacleId)
        {
            try
            {
                return Ok(await _service.GetAllInSpectacle(spectacleId));
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

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> Buy(BuyTicket model)
        {
            try
            {
                return Ok(await _service.Buy(model));
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

        [Authorize(Roles = "Client")]
        [HttpPut("refund/{ticketId}")]
        public async Task<IActionResult> Refund(int ticketId)
        {
            try
            {
                return Ok(await _service.Refund(ticketId));
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

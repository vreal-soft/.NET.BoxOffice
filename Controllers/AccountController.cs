using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BoxOffice.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService service, ILogger<AccountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("client-registration")]
        public async Task<IActionResult> Registration(Registration model)
        {
            try
            {
                var result = await _service.ClientRegistration(model);
                return Ok(result);
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
        [Route("client-login")]
        public async Task<IActionResult> ClientLogin(Login model)
        {
            try
            {
                var result = await _service.ClientLogin(model);
                return Ok(result);
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

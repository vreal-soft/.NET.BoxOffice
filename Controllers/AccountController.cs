﻿using BoxOffice.Core.Data;
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

        [HttpPost("client-registration")]
        public async Task<IActionResult> ClientRegistration(Registration model)
        {
            try
            {
                return Ok(await _service.ClientRegistration(model));
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

        [HttpPost("client-login")]
        public async Task<IActionResult> ClientLogin(Login model)
        {
            try
            {
                return Ok(await _service.ClientLogin(model));
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

        [HttpPost("admin-registration")]
        public async Task<IActionResult> AdminRegistration(Registration model)
        {
            try
            {
                return Ok(await _service.AdminRegistration(model));
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

        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLoginw(Login model)
        {
            try
            {
                return Ok(await _service.AdminLogin(model));
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

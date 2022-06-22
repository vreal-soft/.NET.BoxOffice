﻿using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Settings;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Controllers
{
    [Route("api/spectacles")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SpectacleController : BaseController
    {
        private readonly ISpectacleService _service;
        private readonly SieveProcessor _sieveProcessor;

        public SpectacleController(ISpectacleService service, SieveProcessor sieveProcessor, IHttpContextAccessor accessor, SpectacleDatabaseSettings settings) : base(accessor, settings)
        {
            _service = service;
            _sieveProcessor = sieveProcessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel query)
        {
            var result = await _service.GetAll();
            return Ok(new PaginatedData<SpectacleDto>(result.AsQueryable(), query, _sieveProcessor));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpectacle model)
        {
            await _service.CreateAsync(model, GetCurrentAdmin());
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(SpectacleDto model)
        {
            return Ok(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            return Ok(new { result = await _service.RemoveAsync(id) });
        }
    }
}

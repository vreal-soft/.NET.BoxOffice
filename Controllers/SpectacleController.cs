using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class SpectacleController : BaseController
    {
        private readonly ISpectacleService _service;
        private readonly SieveProcessor _sieveProcessor;

        public SpectacleController(ISpectacleService service, SieveProcessor sieveProcessor, IHttpContextAccessor accessor, AppDbContext context) : base(accessor, context)
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
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _service.GetById(id));
        }

        [AllowAnonymous]
        [HttpGet("file/csv")]
        public async Task<IActionResult> GetCsvFile()
        {
            return File(await _service.CreateCsvFileAsync(),
                        "text/csv",
                        $"spectacle_db_{DateTime.Now.ToString("yyyy/MM/dd")}.csv"
                        );
        } 
        
        [AllowAnonymous]
        [HttpGet("file/xml")]
        public async Task<IActionResult> GetXmlFile()
        {
            return File(await _service.CreateXmlFileAsync(),
                        "text/csv",
                        $"spectacle_db_{DateTime.Now.ToString("yyyy/MM/dd")}.csv"
                        );
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpectacle model)
        {
            return Ok(await _service.CreateAsync(model, GetCurrentAdmin()));
        }

        [AllowAnonymous]
        [HttpPost("from-csv")]
        public async Task<IActionResult> CreateFromCsv(IFormFile file)
        {
            return Ok(await _service.CreateFromCsv(file, GetCurrentAdmin()));
        }

        [AllowAnonymous]
        [HttpPost("from-csv")]
        public async Task<IActionResult> CreateFromXml(IFormFile file)
        {
            return Ok(await _service.CreateFromXml(file, GetCurrentAdmin()));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SpectacleDto model)
        {
            return Ok(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            return Ok(new { result = await _service.RemoveAsync(id) });
        }
    }
}

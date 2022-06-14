using BoxOffice.Core.Commands;
using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Queries;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
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
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMediator _mediator;

        public SpectacleController(SieveProcessor sieveProcessor, IHttpContextAccessor accessor, AppDbContext context, IMediator mediator) : base(accessor, context)
        {
            _sieveProcessor = sieveProcessor;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel query)
        {
            var result = await _mediator.Send(new GetAllSpectaclesQuery());
            return Ok(new PaginatedData<SpectacleDto>(result.AsQueryable(), query, _sieveProcessor));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetSpectacleByIdQuery(id));
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpectacleCommand command)
        {
            command.admin = GetCurrentAdmin();
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateSpectacleCommand model)
        {
            return Ok(await _mediator.Send(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            return Ok(new { result = await _mediator.Send(new RemoveSpectacleCommand(id)) });
        }
    }
}

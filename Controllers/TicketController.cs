using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Commands.Ticket;
using BoxOffice.Core.MediatR.Queries.Ticket;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BoxOffice.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : BaseController
    {
        private readonly ITicketService _service;
        private readonly IMediator _mediator;

        public TicketController(ITicketService service, IHttpContextAccessor accessor, AppDbContext context, IMediator mediator) : base(accessor, context)
        {
            _service = service;
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTicketsQuery());
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetTicketByIdQuery(id));
            return result == null ? NotFound() : Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("free-places/{spectacleId}")]
        public async Task<IActionResult> GetFreePlaces([FromRoute] int spectacleId)
        {
            var result = await _service.GetFreePlaces(spectacleId);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Client")]
        [HttpGet("client/all")]
        public async Task<IActionResult> GetAllInClient()
        {
            var result = await _service.GetAllInClient(GetCurrentClient());
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("spectacle/{spectacleId}")]
        public async Task<IActionResult> GetAllInSpectacle([FromRoute] int spectacleId)
        {
            var result = await _service.GetAllInSpectacle(spectacleId);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> Buy(CreateTicketCommand command)
        {
            command.Client = GetCurrentClient();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Client")]
        [HttpPut("refund/{ticketId}")]
        public async Task<IActionResult> Refund([FromRoute] int ticketId)
        {
            return Ok(new { result = await _service.Refund(ticketId, GetCurrentClient()) });
        }
    }
}

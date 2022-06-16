using BoxOffice.Core.Data;
using BoxOffice.Core.Dto;
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
            return Ok(await _mediator.Send(new GetAllTicketsQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _mediator.Send(new GetTicketByIdQuery(id)));
        }

        [AllowAnonymous]
        [HttpGet("free-places/{spectacleId}")]
        public async Task<IActionResult> GetFreePlaces([FromRoute] int spectacleId)
        {
            return Ok(await _service.GetFreePlaces(spectacleId));
        }

        [Authorize(Roles = "Client")]
        [HttpGet("client/all")]
        public async Task<IActionResult> GetAllInClient()
        {
            return Ok(await _service.GetAllInClient(GetCurrentClient()));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("spectacle/{spectacleId}")]
        public async Task<IActionResult> GetAllInSpectacle([FromRoute] int spectacleId)
        {
            return Ok(await _service.GetAllInSpectacle(spectacleId));
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> Buy(BuyTicket model)
        {
            return Ok(await _service.BuyAsync(model, GetCurrentClient()));
        }

        [Authorize(Roles = "Client")]
        [HttpPut("refund/{ticketId}")]
        public async Task<IActionResult> Refund([FromRoute] int ticketId)
        {
            return Ok(new { result = await _service.Refund(ticketId, GetCurrentClient()) });
        }
    }
}

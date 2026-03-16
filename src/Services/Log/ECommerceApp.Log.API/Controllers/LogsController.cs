using ECommerceApp.Log.Application.Features.Logs.Commands.CreateLog;
using ECommerceApp.Log.Application.Features.Logs.Queries.GetLogs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Log.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] string? level,
            [FromQuery] string? source,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetLogsQuery { Level = level, Source = source },
                cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLog(
            [FromBody] CreateLogCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}

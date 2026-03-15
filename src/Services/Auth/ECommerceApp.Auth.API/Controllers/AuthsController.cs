using ECommerceApp.Auth.Application.Features.Auth.Commands.Login;
using ECommerceApp.Auth.Application.Features.Auth.Commands.RefreshToken;
using ECommerceApp.Auth.Application.Features.Auth.Commands.Register;
using ECommerceApp.Auth.Application.Features.Auth.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserQuery { UserId = id }, cancellationToken);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }
    }
}

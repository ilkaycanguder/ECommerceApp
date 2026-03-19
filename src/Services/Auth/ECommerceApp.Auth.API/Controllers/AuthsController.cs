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
    /// <summary>
    /// Authentication işlemlerini yöneten controller.
    /// JWT token üretimi ve refresh token mekanizması burada yönetilir.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur.
        /// CQRS Pattern: RegisterCommand bir Command nesnesidir, veri değişikliği yapar.
        /// </summary>
        /// <param name="command">Kullanıcı kayıt bilgileri</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Kayıt olan kullanıcı bilgileri</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Kullanıcı girişi yapar ve JWT token döner.
        /// CQRS Pattern: LoginCommand bir Command nesnesidir, refresh token DB'ye yazılır.
        /// </summary>
        /// <param name="command">Email ve şifre bilgileri</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Access token ve refresh token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Süresi dolmuş access token'ı yeniler.
        /// Mevcut refresh token iptal edilir, yeni token çifti üretilir.
        /// </summary>
        /// <param name="command">Mevcut refresh token</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Yeni access token ve refresh token</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Kullanıcı bilgilerini getirir.
        /// CQRS Pattern: GetUserQuery bir Query nesnesidir, veri değişikliği yapmaz.
        /// JWT doğrulaması gerektirir.
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Kullanıcı bilgileri</returns>
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

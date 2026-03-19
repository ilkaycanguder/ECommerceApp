using ECommerceApp.Log.Application.Features.Logs.Commands.CreateLog;
using ECommerceApp.Log.Application.Features.Logs.Queries.GetLogs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Log.API.Controllers
{
    /// <summary>
    /// Merkezi log yönetimini sağlayan controller.
    /// Observer Pattern: RabbitMQ üzerinden diğer servislerden gelen eventleri dinler ve loglar.
    /// Tüm endpointler JWT doğrulaması gerektirir.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Log kayıtlarını listeler.
        /// CQRS Pattern: GetLogsQuery bir Query nesnesidir, veri değişikliği yapmaz.
        /// Level veya Source parametresiyle filtrelenebilir.
        /// </summary>
        /// <param name="level">Opsiyonel log seviyesi filtresi (INFO, WARNING, ERROR, CRITICAL)</param>
        /// <param name="source">Opsiyonel kaynak servis filtresi</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Log kayıtları listesi</returns>
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

        /// <summary>
        /// Yeni log kaydı oluşturur.
        /// CQRS Pattern: CreateLogCommand bir Command nesnesidir, DB'ye yazım yapar.
        /// Strategy Pattern: Log seviyesine göre farklı detay seviyeleri uygulanır.
        /// </summary>
        /// <param name="command">Log bilgileri (Level, Message, Source)</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Başarılı yanıt</returns>
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

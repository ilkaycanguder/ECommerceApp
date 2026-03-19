using ECommerceApp.Product.Application.Features.Products.Commands.CreateProduct;
using ECommerceApp.Product.Application.Features.Products.Commands.UpdateProduct;
using ECommerceApp.Product.Application.Features.Products.Queries.GetAllProducts;
using ECommerceApp.Product.Application.Features.Products.Queries.GetProductById;
using ECommerceApp.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Product.API.Controllers
{
    /// <summary>
    /// Ürün yönetimini sağlayan controller.
    /// CQRS Pattern ile okuma ve yazma işlemleri ayrıştırılmıştır.
    /// Listeleme işlemleri Redis Cache ile optimize edilmiştir.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm ürünleri listeler.
        /// Cache-Aside Pattern: Önce Redis Cache kontrol edilir, yoksa DB'den alınır ve cache'e yazılır.
        /// CQRS Pattern: GetAllProductsQuery bir Query nesnesidir, veri değişikliği yapmaz.
        /// </summary>
        /// <param name="category">Opsiyonel kategori filtresi</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Ürün listesi</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? category,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetAllProductsQuery { Category = category },
                cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// ID'ye göre ürün getirir.
        /// Cache-Aside Pattern: Önce Redis Cache kontrol edilir, yoksa DB'den alınır.
        /// </summary>
        /// <param name="id">Ürün ID'si</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Ürün bilgileri</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetProductByIdQuery { Id = id },
                cancellationToken);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Yeni ürün ekler.
        /// CQRS Pattern: CreateProductCommand bir Command nesnesidir, DB'ye yazım yapar.
        /// Domain Events: Ürün eklendikten sonra ProductAddedEvent fırlatılır.
        /// Cache Invalidation: Ürün listesi cache'i temizlenir.
        /// </summary>
        /// <param name="command">Ürün bilgileri</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Oluşturulan ürün bilgileri</returns>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Mevcut ürünü günceller.
        /// JWT doğrulaması gerektirir — sadece yetkili kullanıcılar güncelleyebilir.
        /// CQRS Pattern: UpdateProductCommand bir Command nesnesidir.
        /// Domain Events: Güncelleme sonrası ProductUpdatedEvent fırlatılır.
        /// Cache Invalidation: İlgili ürün ve liste cache'i temizlenir.
        /// </summary>
        /// <param name="id">Güncellenecek ürün ID'si</param>
        /// <param name="command">Güncel ürün bilgileri</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Güncellenmiş ürün bilgileri</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest(Error.Create("Product.Update", "Id mismatch."));

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
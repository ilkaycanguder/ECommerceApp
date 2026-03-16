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
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
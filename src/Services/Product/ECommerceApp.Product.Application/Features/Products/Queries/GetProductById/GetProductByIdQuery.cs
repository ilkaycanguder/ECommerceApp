using ECommerceApp.Product.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;

namespace ECommerceApp.Product.Application.Features.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery : IRequest<Result<ProductDto>>
    {
        public Guid Id { get; init; }
    }
}

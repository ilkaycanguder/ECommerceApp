using ECommerceApp.Product.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Product.Application.Features.Products.Commands.CreateProduct
{
    public sealed record CreateProductCommand : IRequest<Result<ProductDto>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public string Category { get; init; } = string.Empty;
    }
}

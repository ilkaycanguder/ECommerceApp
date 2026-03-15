using ECommerceApp.Product.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Product.Application.Features.Products.Queries.GetAllProducts
{
    public sealed record GetAllProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>
    {
        public string? Category { get; init; }
    }
}

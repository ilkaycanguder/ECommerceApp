using AutoMapper;
using ECommerceApp.Product.Application.DTOs;
using ECommerceApp.Product.Application.Interfaces;
using ECommerceApp.Product.Domain.Interfaces;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Product.Application.Features.Products.Queries.GetProductById
{
    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(
            IProductRepository productRepository,
            ICacheService cacheService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Cache-Aside pattern
            var cacheKey = $"product:{request.Id}";
            var cachedProduct = await _cacheService.GetAsync<ProductDto>(cacheKey, cancellationToken);

            if (cachedProduct is not null)
                return Result.Success(cachedProduct);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                return Result.Failure<ProductDto>(Error.Create("Product.Get", "Product not found."));

            var productDto = _mapper.Map<ProductDto>(product);

            // Cache'e yaz — 5 dakika
            await _cacheService.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(5), cancellationToken);

            return Result.Success(productDto);
        }
    }
}

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

namespace ECommerceApp.Product.Application.Features.Products.Queries.GetAllProducts
{
    public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(
            IProductRepository productRepository,
            ICacheService cacheService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Cache-Aside pattern — önce cache'e bak
            var cacheKey = $"products:{request.Category ?? "all"}";
            var cachedProducts = await _cacheService.GetAsync<IEnumerable<ProductDto>>(cacheKey, cancellationToken);

            if (cachedProducts is not null)
                return Result.Success(cachedProducts);

            // Cache'de yoksa DB'den al
            var products = string.IsNullOrEmpty(request.Category)
                ? await _productRepository.GetAllAsync(cancellationToken)
                : await _productRepository.GetByCategoryAsync(request.Category, cancellationToken);

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            // Cache'e yaz — 5 dakika
            await _cacheService.SetAsync(cacheKey, productDtos, TimeSpan.FromMinutes(5), cancellationToken);

            return Result.Success(productDtos);
        }
    }
}

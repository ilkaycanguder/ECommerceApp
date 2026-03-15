using AutoMapper;
using ECommerceApp.Contracts.Events.Product;
using ECommerceApp.Product.Application.DTOs;
using ECommerceApp.Product.Application.Interfaces;
using ECommerceApp.Product.Domain.Interfaces;
using ECommerceApp.Shared.Abstractions;
using ECommerceApp.Shared.Models;
using MediatR;

namespace ECommerceApp.Product.Application.Features.Products.Commands.UpdateProduct
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IEventPublisher eventPublisher,
            ICacheService cacheService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                return Result.Failure<ProductDto>(Error.Create("Product.Update", "Product not found."));

            product.Update(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.Category);

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation — güncellenen ürünün cache'ini temizle
            await _cacheService.RemoveAsync($"product:{request.Id}", cancellationToken);
            await _cacheService.RemoveByPrefixAsync("products", cancellationToken);

            // Event fırlat
            await _eventPublisher.PublishAsync(new ProductUpdatedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                UpdatedAt = product.UpdatedAt ?? DateTime.UtcNow
            }, cancellationToken);

            return Result.Success(_mapper.Map<ProductDto>(product));
        }
    }
}

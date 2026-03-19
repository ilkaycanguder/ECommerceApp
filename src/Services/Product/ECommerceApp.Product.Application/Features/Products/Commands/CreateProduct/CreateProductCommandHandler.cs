using AutoMapper;
using ECommerceApp.Contracts.Events.Product;
using ECommerceApp.Product.Application.DTOs;
using ECommerceApp.Product.Application.Interfaces;
using ECommerceApp.Product.Domain.Interfaces;
using ECommerceApp.Shared.Abstractions;
using ECommerceApp.Shared.Models;
using MediatR;
using ProductEntity = ECommerceApp.Product.Domain.Entities.Product;

namespace ECommerceApp.Product.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Create Product Command Handler — CQRS Pattern implementasyonu.
    /// 
    /// CQRS: Bu sınıf sadece "yazma" sorumluluğuna sahiptir.
    /// Ürün listeleme ve getirme işlemleri Query Handler'lar tarafından yönetilir.
    /// 
    /// Domain Events + Outbox Pattern: Ürün DB'ye kaydedildikten sonra
    /// ProductAddedEvent fırlatılır. Bu event RabbitMQ üzerinden Log servisine iletilir.
    /// Böylece servisler arası loose coupling sağlanır.
    /// 
    /// Cache Invalidation: Yeni ürün eklenince "products" prefix'li tüm cache key'leri
    /// temizlenir. Bir sonraki listeleme isteği güncel veriyi DB'den alır.
    /// 
    /// Repository Pattern: IProductRepository aracılığıyla veri erişimi soyutlanmıştır.
    /// </summary>
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(
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

        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var isNameUnique = await _productRepository.IsNameUniqueAsync(request.Name, cancellationToken);

            if (!isNameUnique)
                return Result.Failure<ProductDto>(Error.Create("Product.Create", "Product name already exists."));

            var product = ProductEntity.Create(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.Category);

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation — ürün eklenince liste cache'i temizle
            await _cacheService.RemoveByPrefixAsync("products", cancellationToken);

            // Event fırlat — diğer servisler bilgilendirilsin
            await _eventPublisher.PublishAsync(new ProductAddedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                AddedAt = product.CreatedAt
            }, cancellationToken);

            return Result.Success(_mapper.Map<ProductDto>(product));
        }
    }
}

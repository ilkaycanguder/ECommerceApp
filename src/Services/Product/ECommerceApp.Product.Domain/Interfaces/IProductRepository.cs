using ECommerceApp.Product.Domain.Enums;
using ECommerceApp.Shared.Abstractions;
using ProductEntity = ECommerceApp.Product.Domain.Entities.Product;

namespace ECommerceApp.Product.Domain.Interfaces
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        Task<IEnumerable<ProductEntity>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductEntity>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default);
        Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default);
    }
}

using ECommerceApp.Product.Domain.Enums;
using ECommerceApp.Product.Domain.Interfaces;
using ECommerceApp.Product.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ProductEntity = ECommerceApp.Product.Domain.Entities.Product;

namespace ECommerceApp.Product.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(ProductDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
            => await _dbSet.Where(x => x.Category == category).ToListAsync(cancellationToken);

        public async Task<IEnumerable<ProductEntity>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default)
            => await _dbSet.Where(x => x.Status == status).ToListAsync(cancellationToken);

        public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.Name == name, cancellationToken);
    }
}

using ECommerceApp.Product.Infrastructure.Persistence.Context;
using ECommerceApp.Shared.Abstractions;

namespace ECommerceApp.Product.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductDbContext _context;

        public UnitOfWork(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);
    }
}

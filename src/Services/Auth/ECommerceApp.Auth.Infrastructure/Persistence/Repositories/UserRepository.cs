using ECommerceApp.Auth.Domain.Entities;
using ECommerceApp.Auth.Domain.Interfaces;
using ECommerceApp.Auth.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Auth.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.Email == email, cancellationToken);
    }
}

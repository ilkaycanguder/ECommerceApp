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
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthDbContext context) : base(context) { }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

        public async Task RevokeAllUserTokensAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _dbSet
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
                token.Revoke();
        }
    }
}

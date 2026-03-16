using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Log.Application.Interfaces;
using ECommerceApp.Log.Infrastructure.Persistence.Context;
using ECommerceApp.Log.Infrastructure.Persistence.Entities;

namespace ECommerceApp.Log.Infrastructure.Persistence.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly LogDbContext _context;

        public LogRepository(LogDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreateLogDto logDto, CancellationToken cancellationToken = default)
        {
            var logEntry = new LogEntry
            {
                Level = logDto.Level,
                Message = logDto.Message,
                Source = logDto.Source,
                Exception = logDto.Exception
            };

            await _context.LogEntries.AddAsync(logEntry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<LogDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.LogEntries
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new LogDto
                {
                    Id = x.Id,
                    Level = x.Level,
                    Message = x.Message,
                    Source = x.Source,
                    Exception = x.Exception,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<LogDto>> GetByLevelAsync(string level, CancellationToken cancellationToken = default)
        {
            return await _context.LogEntries
                .Where(x => x.Level == level)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new LogDto
                {
                    Id = x.Id,
                    Level = x.Level,
                    Message = x.Message,
                    Source = x.Source,
                    Exception = x.Exception,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<LogDto>> GetBySourceAsync(string source, CancellationToken cancellationToken = default)
        {
            return await _context.LogEntries
                .Where(x => x.Source == source)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new LogDto
                {
                    Id = x.Id,
                    Level = x.Level,
                    Message = x.Message,
                    Source = x.Source,
                    Exception = x.Exception,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}

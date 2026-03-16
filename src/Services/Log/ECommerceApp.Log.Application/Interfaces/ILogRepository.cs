using ECommerceApp.Log.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Log.Application.Interfaces
{
    public interface ILogRepository
    {
        Task AddAsync(CreateLogDto logDto, CancellationToken cancellationToken = default);
        Task<IEnumerable<LogDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<LogDto>> GetByLevelAsync(string level, CancellationToken cancellationToken = default);
        Task<IEnumerable<LogDto>> GetBySourceAsync(string source, CancellationToken cancellationToken = default);
    }
}

using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Log.Application.Interfaces;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Log.Application.Features.Logs.Queries.GetLogs
{
    public sealed class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, Result<IEnumerable<LogDto>>>
    {
        private readonly ILogRepository _logRepository;

        public GetLogsQueryHandler(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<Result<IEnumerable<LogDto>>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<LogDto> logs;

            if (!string.IsNullOrEmpty(request.Level))
                logs = await _logRepository.GetByLevelAsync(request.Level, cancellationToken);
            else if (!string.IsNullOrEmpty(request.Source))
                logs = await _logRepository.GetBySourceAsync(request.Source, cancellationToken);
            else
                logs = await _logRepository.GetAllAsync(cancellationToken);

            return Result.Success(logs);
        }
    }
}

using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Log.Application.Features.Logs.Queries.GetLogs
{
    public sealed record GetLogsQuery : IRequest<Result<IEnumerable<LogDto>>>
    {
        public string? Level { get; init; }
        public string? Source { get; init; }
    }
}

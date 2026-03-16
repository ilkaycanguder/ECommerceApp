using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Log.Application.DTOs
{
    public sealed record CreateLogDto
    {
        public string Level { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Source { get; init; } = string.Empty;
        public string? Exception { get; init; }
    }
}

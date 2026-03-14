using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Contracts.Events.Auth
{
    public sealed record UserRegisteredEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
    }
}

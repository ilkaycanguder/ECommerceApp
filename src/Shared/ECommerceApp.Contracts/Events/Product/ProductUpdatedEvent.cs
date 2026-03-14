using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Contracts.Events.Product
{
    public sealed record ProductUpdatedEvent
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    }
}

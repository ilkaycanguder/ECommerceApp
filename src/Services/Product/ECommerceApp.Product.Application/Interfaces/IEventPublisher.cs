using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Product.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
    }
}

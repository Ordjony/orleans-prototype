using System.Collections.Generic;
using Orleans.Concurrency;

namespace Prototype.Interfaces.Orders
{
    [Immutable]
    public class OrderCreateData
    {
        public ICollection<OrderItem> Items { get; set; }
    }
}

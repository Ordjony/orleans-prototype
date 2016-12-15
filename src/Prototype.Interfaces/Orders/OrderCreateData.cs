using System.Collections.Generic;

namespace Prototype.Interfaces.Orders
{
    public class OrderCreateData
    {
        public ICollection<OrderItem> Items { get; set; }
    }
}

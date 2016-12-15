using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Prototype.Interfaces.Orders;

namespace Prototype.Grains
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private OrderState _state = OrderState.NotCreated;
        private List<OrderItem> _items = new List<OrderItem>();

        public Task<OrderState> GetState()
        {
            return Task.FromResult(_state);
        }
    }
}

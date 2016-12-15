using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Prototype.Interfaces.Offers;
using Prototype.Interfaces.Orders;

namespace Prototype.Grains
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private OrderState _state = OrderState.NotCreated;
        private readonly List<OrderItem> _items = new List<OrderItem>();
        private readonly Dictionary<int, IOfferGrain> _offerGrains = new Dictionary<int, IOfferGrain>();

        public Task<OrderState> GetState()
        {
            return Task.FromResult(_state);
        }

        public async Task Create(OrderCreateData createData)
        {
            if (IsCreated())
            {
                throw new Exception("Order already has created");
            }

            var reservePromise = new List<Task>();

            foreach (var orderItem in createData.Items)
            {
                var offer = GrainFactory.GetGrain<IOfferGrain>(orderItem.Id);
                _offerGrains.Add(orderItem.Id, offer);

                reservePromise.Add(offer.ReserveForOrder((int)this.GetPrimaryKeyLong(), orderItem.Quantity));
                _items.Add(orderItem);
            }

            await Task.WhenAll(reservePromise);
        }

        private bool IsCreated()
        {
            return _state != OrderState.NotCreated;
        }
    }
}

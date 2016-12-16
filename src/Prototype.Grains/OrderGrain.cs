using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }

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

            var reservePromise = new List<Task<bool>>();

            foreach (var orderItem in createData.Items)
            {
                var offer = GrainFactory.GetGrain<IOfferGrain>(orderItem.Id);
                _offerGrains.Add(orderItem.Id, offer);

                reservePromise.Add(offer.ReserveForOrder((int)this.GetPrimaryKeyLong(), orderItem.Quantity));
                _items.Add(orderItem);
            }

            await Task.WhenAll(reservePromise);
            if (reservePromise.Any(t => !t.Result))
            {
                foreach (var offerGrain in _offerGrains)
                {
                    await offerGrain.Value.CancelOrderReserv(this.GetPrimaryKeyLong());
                }

                throw new Exception("Order can't reserve all items.");
            }

            _state = OrderState.Created;
        }

        public async Task ConfirmPayment()
        {
            var promises = new List<Task<bool>>(_offerGrains.Count);

            foreach (var offerGrain in _offerGrains)
            {
                promises.Add(offerGrain.Value.ConfirmOrderReservation(this.GetPrimaryKeyLong()));
            }

            await Task.WhenAll(promises);
        }

        private bool IsCreated()
        {
            return _state != OrderState.NotCreated;
        }
    }
}

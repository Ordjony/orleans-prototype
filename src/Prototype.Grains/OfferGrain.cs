using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Prototype.Grains.Stores;
using Prototype.Interfaces.Offers;

namespace Prototype.Grains
{
    public class OfferGrain : Grain, IOfferGrain
    {
        private OfferData _offerData;
        private int _stock = 0;
        private int _reserved = 0;
        private Dictionary<long, int> _orderReserved = new Dictionary<long, int>();

        public override Task OnActivateAsync()
        {
            OfferData data;
            OffersStore.Database.TryGetValue(this.GetPrimaryKeyLong(), out data);
            _offerData = data;

            _stock = data?.Stock ?? 0;

            return base.OnActivateAsync();
        }

        public Task<bool> ReserveForOrder(long orderId, int count)
        {
            var newReservation = _reserved + count;
            if (newReservation > _stock)
            {
                return Task.FromResult(false);
            }

            _orderReserved.Add(orderId, count);
            _reserved = newReservation;

            return Task.FromResult(true);
        }

        public Task CancelOrderReserv(long orderId)
        {
            int reservedOrderStock;
            if (!_orderReserved.TryGetValue(orderId, out reservedOrderStock))
            {
                return Task.CompletedTask;
            }

            _orderReserved.Remove(orderId);
            _reserved -= reservedOrderStock;

            return Task.CompletedTask;
        }

        public Task<bool> ConfirmOrderReservation(long orderId)
        {
            int reservedOrderStock;
            if (!_orderReserved.TryGetValue(orderId, out reservedOrderStock))
            {
                return Task.FromResult(false);
            }

            _stock -= reservedOrderStock;
            _reserved -= reservedOrderStock;
            _orderReserved.Remove(orderId);

            return Task.FromResult(true);
        }

        public Task ApplyStock(int stock)
        {
            _stock = stock;
            return Task.CompletedTask;
        }
    }
}

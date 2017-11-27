using System.Threading.Tasks;
using Orleans;
using Prototype.Grains.Stores;
using Prototype.Interfaces.Offers;
using Prototype.Interfaces.Products;

namespace Prototype.Grains
{
    public class OfferGrain : Grain<OfferData>, IOfferGrain
    {
        private IProductGrain _product;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            if (State.Sku != null)
            {
                return;
            }

            State = OffersStore.Instance.Value.GetById(this.GetPrimaryKeyLong());

            _product = GrainFactory.GetGrain<IProductGrain>(State.ProductId);
        }

        public async Task<bool> ReserveForOrder(long orderId, int count)
        {
            var newReservation = State.StockReserved + count;
            if (newReservation > State.Stock)
            {
                return false;
            }

            State.OrderReserved.Add(orderId, count);
            State.StockReserved = newReservation;

            await WriteStateAsync();

            return true;
        }

        public async Task CancelOrderReserv(long orderId)
        {
            if (!State.OrderReserved.TryGetValue(orderId, out var reservedOrderStock))
            {
                return;
            }

            State.OrderReserved.Remove(orderId);
            State.StockReserved -= reservedOrderStock;

            await WriteStateAsync();
        }

        public async Task<bool> ConfirmOrderReservation(long orderId)
        {
            if (!State.OrderReserved.TryGetValue(orderId, out var reservedOrderStock))
            {
                return false;
            }

            State.Stock -= reservedOrderStock;
            State.StockReserved -= reservedOrderStock;
            State.OrderReserved.Remove(orderId);

            await WriteStateAsync();

            return true;
        }

        public async Task ApplyStock(int stock)
        {
            State.Stock = stock;
            await WriteStateAsync();
        }
    }
}

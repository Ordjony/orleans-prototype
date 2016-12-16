using System.Threading.Tasks;
using Orleans;

namespace Prototype.Interfaces.Offers
{
    public interface IOfferGrain : IGrainWithIntegerKey
    {
        Task<bool> ReserveForOrder(long orderId, int count);
        Task CancelOrderReserv(long orderId);
        Task<bool> ConfirmOrderReservation(long orderId);
        Task ApplyStock(int stock);
    }
}

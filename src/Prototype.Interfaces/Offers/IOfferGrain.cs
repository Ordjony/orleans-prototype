using System.Threading.Tasks;
using Orleans;

namespace Prototype.Interfaces.Offers
{
    public interface IOfferGrain : IGrainWithIntegerKey
    {
        Task<bool> ReserveForOrder(int orderId, int count);
    }
}

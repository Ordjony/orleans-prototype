using System.Threading.Tasks;
using Orleans;
using Prototype.Interfaces.Offers;

namespace Prototype.Grains
{
    public class OfferGrain : Grain, IOfferGrain
    {
        public Task<bool> ReserveForOrder(int orderId, int count)
        {
            return Task.FromResult(true);
        }
    }
}

using System.Threading.Tasks;
using Orleans;

namespace Prototype.Interfaces.Orders
{
    public interface IOrderGrain : IGrainWithIntegerKey
    {
        Task<OrderState> GetState();
    }
}

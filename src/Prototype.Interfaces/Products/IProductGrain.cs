using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Prototype.Interfaces.Products
{
    public interface IProductGrain : IGrainWithIntegerKey
    {
        Task UpdatePropertiesAsync(IDictionary<string, string> updateProperties);
        Task<ProductState> GetState();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Prototype.Grains.Stores;
using Prototype.Interfaces.Products;

namespace Prototype.Grains
{
    public class ProductGrain : Grain, IProductGrain
    {
        private long _productId;
        private Dictionary<string, string> _properties = new Dictionary<string, string>();
        private ProductState _state;

        public override Task OnActivateAsync()
        {
            _productId = this.GetPrimaryKeyLong();
            var stored = ProductStore.Instance.Value.GetById(_productId);

            _properties = stored.Properties;
            _state = stored.State;

            return base.OnActivateAsync();
        }

        public Task UpdatePropertiesAsync(IDictionary<string, string> updateProperties)
        {
            foreach (var updateProperty in updateProperties)
            {
                _properties[updateProperty.Key] = updateProperty.Value;
            }

            return Task.CompletedTask;
        }

        public Task<ProductState> GetState()
        {
            return Task.FromResult(_state);
        }
    }
}

using System.Collections.Generic;
using Prototype.Interfaces.Products;

namespace Prototype.Grains.Stores
{
    public class ProductData
    {
        public long Id { get; set; }
        public ProductState State { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}

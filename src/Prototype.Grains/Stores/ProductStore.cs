using System;
using System.Collections.Generic;
using Prototype.Interfaces.Products;

namespace Prototype.Grains.Stores
{
    public class ProductStore
    {
        public static readonly Lazy<ProductStore> Instance = new Lazy<ProductStore>();

        public ProductData GetById(long id)
        {
            return new ProductData
            {
                Id = id,
                State = ProductState.Active,
                Properties = new Dictionary<string, string>
                {
                    ["Color"] = "Red",
                    ["Size"] = "L",
                }
            };
        }
    }
}

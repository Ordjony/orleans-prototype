using System.Collections.Generic;

namespace Prototype.Grains.Stores
{
    public class OffersStore
    {
        public static Dictionary<long, OfferData> Database { get; } = new Dictionary<long, OfferData>
        {
            [1] = new OfferData
            {
                Sku = "123",
                Stock = 10,
                Price = 10,
            },
            [2] = new OfferData
            {
                Sku = "456",
                Stock = 8,
                Price = 40,
            },
            [3] = new OfferData
            {
                Sku = "145",
                Stock = 5,
                Price = 99,
            },
            [4] = new OfferData
            {
                Sku = "987",
                Stock = 25,
                Price = 24,
            },
            [5] = new OfferData
            {
                Sku = "267",
                Stock = 2,
                Price = 140,
            },
            [6] = new OfferData
            {
                Sku = "491",
                Stock = 8,
                Price = 82,
            },

        };
    }
}

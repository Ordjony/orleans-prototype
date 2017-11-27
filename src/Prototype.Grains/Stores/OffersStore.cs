using System;
using System.Collections.Generic;

namespace Prototype.Grains.Stores
{
    public class OffersStore
    {
        public static readonly Lazy<OffersStore> Instance = new Lazy<OffersStore>();

        public OfferData GetById(long id)
        {
            if (_database.TryGetValue(id, out var offer))
            {
                return offer;
            }

            return null;
        }

        private readonly Dictionary<long, OfferData> _database  = new Dictionary<long, OfferData>
        {
            [1] = new OfferData
            {
                Id = 1,
                ProductId = 1,
                Sku = "123",
                Stock = 10,
                Price = 10,
            },
            [2] = new OfferData
            {
                Id = 2,
                ProductId = 2,
                Sku = "456",
                Stock = 8,
                Price = 40,
            },
            [3] = new OfferData
            {
                Id = 3,
                ProductId = 3,
                Sku = "145",
                Stock = 5,
                Price = 99,
            },
            [4] = new OfferData
            {
                Id = 4,
                ProductId = 4,
                Sku = "987",
                Stock = 25,
                Price = 24,
            },
            [5] = new OfferData
            {
                Id = 5,
                ProductId = 5,
                Sku = "267",
                Stock = 2,
                Price = 140,
            },
            [6] = new OfferData
            {
                Id = 6,
                ProductId = 6,
                Sku = "491",
                Stock = 8,
                Price = 82,
            },
            [7] = new OfferData
            {
                Id = 7,
                ProductId = 7,
                Sku = "435645",
                Stock = 80,
                Price = 200,
            },
            [8] = new OfferData
            {
                Id = 8,
                ProductId = 8,
                Sku = "12347",
                Stock = 120,
                Price = 884,
            },
            [9] = new OfferData
            {
                Id = 9,
                ProductId = 9,
                Sku = "09887",
                Stock = 50,
                Price = 64,
            },
            [10] = new OfferData
            {
                Id = 10,
                ProductId = 10,
                Sku = "864654",
                Stock = 80,
                Price = 100,
            }
        };
    }
}

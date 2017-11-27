using System.Collections.Generic;

namespace Prototype.Grains.Stores
{
    public class OfferData
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
        public int StockReserved { get; set; }
        public decimal Price { get; set; }
        public Dictionary<long, int> OrderReserved { get; set; }
    }
}
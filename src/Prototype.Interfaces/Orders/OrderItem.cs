namespace Prototype.Interfaces.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}

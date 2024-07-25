namespace Basket.API.Models
{
    public class ShoppingCartItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; } = string.Empty;   
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

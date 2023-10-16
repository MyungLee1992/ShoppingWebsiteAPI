namespace ShoppingWebsiteAPI.Models
{
    public record CartItem
    {
        public Guid Id { get; init; }
        public Cart Cart { get; init; }
        public Item Item { get; init; }
        public int Quantity { get; init; } = 1;
        public double Price { get; init; }
    }
}

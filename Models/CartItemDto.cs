namespace ShoppingWebsiteAPI.Models {
    public class CartItemDto {
        public int Id { get; set; }
        public Cart Cart { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; } = 1;
        public double Price { get; set; }
    }
}

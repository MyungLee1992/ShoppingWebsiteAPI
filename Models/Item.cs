namespace ShoppingWebsiteAPI.Models
{
    public class Item {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0d;
        public string ImageUrl { get; set; } = string.Empty;
    }
}

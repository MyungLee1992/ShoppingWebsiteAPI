namespace ShoppingWebsiteAPI.Models
{
    public record Item {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public double Price { get; init; } = 0.0d;
        public string ImageUrl { get; init; } = string.Empty;
    }
}

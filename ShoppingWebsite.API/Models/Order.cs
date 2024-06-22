namespace ShoppingWebsiteAPI.Models
{
    public record Order
    {
        public Guid Id { get; init; }
        public int UserId { get; init; }
        public User User { get; init; } = null!;
        public double Total { get; init; } = 0d;
    }
}

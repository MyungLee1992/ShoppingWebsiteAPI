namespace ShoppingWebsiteAPI.Models
{
    public record Cart
    {
        public Guid Id { get; init; }
        public int UserId { get; init; }
        public User User { get; init; } = null!;
    }
}

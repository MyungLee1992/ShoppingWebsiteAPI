namespace ShoppingWebsiteAPI.Models
{
    public record CartDto
    {
        public Guid Id { get; init; }
        public UserDto User { get; init; } = null!;
    }
}

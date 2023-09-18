namespace ShoppingWebsiteAPI.Models
{
    public record CartDto
    {
        public UserDto User { get; init; } = null!;
    }
}

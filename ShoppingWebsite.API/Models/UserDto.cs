namespace ShoppingWebsiteAPI.Models
{
    public record UserDto
    {
        public string UserName { get; init; } = String.Empty;
        public string Password { get; init; } = String.Empty;
    }
}

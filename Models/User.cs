namespace ShoppingWebsiteAPI.Models
{
    public record User
    {
        public int Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public byte[] PasswordHash { get; init; } = new byte[32];
        public byte[] PasswordSalt { get; init; } = new byte[32];
        public string RefreshToken { get; init; } = string.Empty;
        public DateTime TokenCreated { get; init; }
        public DateTime TokenExpires { get; init; }
        public Cart Cart { get; init; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsiteAPI.Models
{
    public record CartItemDto
    {
        public CartDto Cart { get; init; }
        public ItemDto Item { get; init; }
        public int Quantity { get; init; } = 1;
        public double Price { get; init; }
    }
}

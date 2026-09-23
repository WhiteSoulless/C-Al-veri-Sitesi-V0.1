using SalesApp.Models.Shop;

namespace SalesApp.Services.Shop;

public interface ICartService
{
    IReadOnlyList<CartItem> Items { get; }
    int TotalItemCount { get; }
    decimal Subtotal { get; }
    decimal DiscountAmount { get; }
    decimal ShippingFee { get; }
    decimal Total { get; }
    string? AppliedCoupon { get; }

    event EventHandler? CartUpdated;

    void AddToCart(Product product, int quantity = 1, string variant = "");
    void RemoveFromCart(int productId);
    void UpdateQuantity(int productId, int quantity);
    bool ApplyCoupon(string couponCode);
    void ClearCart();
}

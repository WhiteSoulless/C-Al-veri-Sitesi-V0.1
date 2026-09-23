using SalesApp.Models.Shop;

namespace SalesApp.Services.Shop;

public class CartService : ICartService
{
    private readonly List<CartItem> _items = new();
    private string? _appliedCoupon;

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public int TotalItemCount => _items.Sum(i => i.Quantity);

    public decimal Subtotal => _items.Sum(i => i.TotalPrice);

    public decimal DiscountAmount
    {
        get
        {
            if (string.Equals(_appliedCoupon, "CLASS20", StringComparison.OrdinalIgnoreCase))
                return Math.Round(Subtotal * 0.20m, 2); // %20 indirim
            return 0;
        }
    }

    // 1000 TL ve üzeri siparişlerde kargo bedava, altı için 59.90 TL
    public decimal ShippingFee => (Subtotal - DiscountAmount) >= 1000m || _items.Count == 0 ? 0m : 59.90m;

    public decimal Total => Math.Max(0, Subtotal - DiscountAmount + ShippingFee);

    public string? AppliedCoupon => _appliedCoupon;

    public event EventHandler? CartUpdated;

    public void AddToCart(Product product, int quantity = 1, string variant = "")
    {
        var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id && i.SelectedVariant == variant);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem
            {
                Product = product,
                Quantity = quantity,
                SelectedVariant = variant
            });
        }
        NotifyCartChanged();
    }

    public void RemoveFromCart(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            NotifyCartChanged();
        }
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            if (quantity <= 0)
                _items.Remove(item);
            else
                item.Quantity = quantity;

            NotifyCartChanged();
        }
    }

    public bool ApplyCoupon(string couponCode)
    {
        if (string.Equals(couponCode.Trim(), "CLASS20", StringComparison.OrdinalIgnoreCase))
        {
            _appliedCoupon = "CLASS20";
            NotifyCartChanged();
            return true;
        }
        return false;
    }

    public void ClearCart()
    {
        _items.Clear();
        _appliedCoupon = null;
        NotifyCartChanged();
    }

    private void NotifyCartChanged()
    {
        CartUpdated?.Invoke(this, EventArgs.Empty);
    }
}

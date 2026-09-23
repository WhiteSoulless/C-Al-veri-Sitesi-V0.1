using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesApp.Models.Shop;
using SalesApp.Services.Network;
using SalesApp.Services.Notification;
using SalesApp.Services.Shop;

namespace SalesApp.ViewModels.Shop;

public partial class CartViewModel : BaseViewModel
{
    private readonly ICartService _cartService;
    private readonly IConnectivityService _connectivityService;
    private readonly INotificationService _notificationService;

    public ObservableCollection<CartItem> CartItems { get; } = new();

    [ObservableProperty]
    private decimal _subtotal;

    [ObservableProperty]
    private decimal _discountAmount;

    [ObservableProperty]
    private decimal _shippingFee;

    [ObservableProperty]
    private decimal _total;

    [ObservableProperty]
    private string _couponCode = string.Empty;

    [ObservableProperty]
    private string _couponMessage = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCartEmpty))]
    private bool _hasItems;

    public bool IsCartEmpty => !HasItems;
    public bool HasCouponMessage => !string.IsNullOrEmpty(CouponMessage);

    public CartViewModel(
        ICartService cartService,
        IConnectivityService connectivityService,
        INotificationService notificationService)
    {
        _cartService = cartService;
        _connectivityService = connectivityService;
        _notificationService = notificationService;

        Title = "Sepetim";

        _cartService.CartUpdated += (s, e) => RefreshCartState();
    }

    [RelayCommand]
    public void LoadCart()
    {
        RefreshCartState();
    }

    private void RefreshCartState()
    {
        CartItems.Clear();
        foreach (var item in _cartService.Items)
        {
            CartItems.Add(item);
        }

        Subtotal = _cartService.Subtotal;
        DiscountAmount = _cartService.DiscountAmount;
        ShippingFee = _cartService.ShippingFee;
        Total = _cartService.Total;
        HasItems = CartItems.Count > 0;
    }

    [RelayCommand]
    public void IncrementQuantity(CartItem item)
    {
        if (item == null) return;
        _cartService.UpdateQuantity(item.Product.Id, item.Quantity + 1);
    }

    [RelayCommand]
    public void DecrementQuantity(CartItem item)
    {
        if (item == null) return;
        _cartService.UpdateQuantity(item.Product.Id, item.Quantity - 1);
    }

    [RelayCommand]
    public async Task RemoveItemAsync(CartItem item)
    {
        if (item == null) return;

        bool confirm = await _notificationService.ShowConfirmAsync(
            "Ürünü Sil",
            $"{item.Product.Name} ürününü sepetten kaldırmak istediğinize emin misiniz?",
            "Kaldır",
            "Vazgeç");

        if (confirm)
        {
            _cartService.RemoveFromCart(item.Product.Id);
        }
    }

    [RelayCommand]
    public async Task ApplyCouponAsync()
    {
        if (string.IsNullOrWhiteSpace(CouponCode))
        {
            CouponMessage = "Lütfen bir kupon kodu giriniz.";
            return;
        }

        bool success = _cartService.ApplyCoupon(CouponCode);
        if (success)
        {
            CouponMessage = "🎉 '%20' indirim kuponu uygulandı!";
            await _notificationService.ShowAlertAsync("Tebrikler!", "CLASS20 kuponu başarıyla uygulandı!");
        }
        else
        {
            CouponMessage = "Geçersiz veya süresi dolmuş kupon kodu. (İpucu: CLASS20)";
        }
    }

    [RelayCommand]
    public async Task CheckoutAsync()
    {
        if (!HasItems)
        {
            await _notificationService.ShowAlertAsync("Sepetiniz Boş", "Sipariş vermek için lütfen sepetinize ürün ekleyin.");
            return;
        }

        if (!_connectivityService.IsConnected)
        {
            await _notificationService.ShowAlertAsync("İnternet Yok", "Siparişinizi tamamlayabilmek için lütfen internete bağlanın.");
            return;
        }

        bool confirm = await _notificationService.ShowConfirmAsync(
            "Sipariş Onayı",
            $"Toplam {Total:C2} tutarındaki siparişinizi onaylıyor musunuz?",
            "Onayla",
            "Vazgeç");

        if (confirm)
        {
            _cartService.ClearCart();
            await _notificationService.ShowAlertAsync(
                "Siparişiniz Alındı! 📦",
                "Siparişiniz başarıyla oluşturuldu. Sipariş detayları SMS ile iletilmiştir!");
        }
    }
}

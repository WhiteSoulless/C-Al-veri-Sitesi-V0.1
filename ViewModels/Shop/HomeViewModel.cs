using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesApp.Models.Shop;
using SalesApp.Services.Network;
using SalesApp.Services.Notification;
using SalesApp.Services.Shop;

namespace SalesApp.ViewModels.Shop;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IConnectivityService _connectivityService;
    private readonly INotificationService _notificationService;

    public ObservableCollection<Category> Categories { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private Category? _selectedCategory;

    [ObservableProperty]
    private int _cartBadgeCount;

    [ObservableProperty]
    private bool _isOffline;

    public HomeViewModel(
        IProductService productService,
        ICartService cartService,
        IConnectivityService connectivityService,
        INotificationService notificationService)
    {
        _productService = productService;
        _cartService = cartService;
        _connectivityService = connectivityService;
        _notificationService = notificationService;

        Title = "Class Alışveriş";

        _connectivityService.ConnectivityChanged += (s, isConnected) =>
        {
            IsOffline = !isConnected;
            if (!isConnected)
            {
                _notificationService.ShowAlertAsync("Bağlantı Uyarısı", "İnternet bağlantınız koptu. Çevrimdışı moddasınız.");
            }
        };

        _cartService.CartUpdated += (s, e) =>
        {
            CartBadgeCount = _cartService.TotalItemCount;
        };

        IsOffline = !_connectivityService.IsConnected;
    }

    [RelayCommand]
    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            IsOffline = !_connectivityService.IsConnected;

            var categories = await _productService.GetCategoriesAsync();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }

            var products = await _productService.GetFeaturedProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }

            CartBadgeCount = _cartService.TotalItemCount;
        }
        catch (Exception ex)
        {
            await _notificationService.ShowAlertAsync("Hata", $"Veriler yüklenemedi: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SearchAsync()
    {
        if (!_connectivityService.IsConnected)
        {
            await _notificationService.ShowAlertAsync("Bağlantı Yok", "Arama yapmak için lütfen internet bağlantınızı kontrol edin.");
            return;
        }

        IsBusy = true;
        try
        {
            var results = await _productService.SearchProductsAsync(SearchQuery);
            Products.Clear();
            foreach (var product in results)
            {
                Products.Add(product);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SelectCategoryAsync(Category category)
    {
        if (category == null) return;
        SelectedCategory = category;

        IsBusy = true;
        try
        {
            var products = await _productService.GetProductsByCategoryAsync(category.Id);
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void QuickAddToCart(Product product)
    {
        if (product == null) return;

        _cartService.AddToCart(product, 1);
        _notificationService.ShowAlertAsync("Sepete Eklendi", $"{product.Name} başarıyla sepetinize eklendi!");
    }
}

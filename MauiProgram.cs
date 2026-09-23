using Microsoft.Extensions.Logging;
using SalesApp.Services.Auth;
using SalesApp.Services.Network;
using SalesApp.Services.Notification;
using SalesApp.Services.Settings;
using SalesApp.Services.Shop;
using SalesApp.ViewModels.Auth;
using SalesApp.ViewModels.Settings;
using SalesApp.ViewModels.Shop;
using SalesApp.Views.Auth;
using SalesApp.Views.Settings;
using SalesApp.Views.Shop;

namespace SalesApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 1. Servisler (Dependency Injection)
        builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddSingleton<ICartService, CartService>();

        // 2. ViewModel'ler
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<CartViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<OtpViewModel>();

        // 3. Sayfalar (Views)
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<OtpPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

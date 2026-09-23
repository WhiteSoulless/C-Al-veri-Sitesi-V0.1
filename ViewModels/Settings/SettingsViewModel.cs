using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesApp.Services.Auth;
using SalesApp.Services.Notification;
using SalesApp.Services.Settings;

namespace SalesApp.ViewModels.Settings;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IAuthService _authService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private bool _isDarkMode;

    [ObservableProperty]
    private string _currencySymbol = "₺";

    [ObservableProperty]
    private bool _isNotificationsEnabled;

    [ObservableProperty]
    private bool _isSmsEnabled;

    [ObservableProperty]
    private string _userFullName = "Misafir Kullanıcı";

    [ObservableProperty]
    private string _userPhone = string.Empty;

    public SettingsViewModel(
        ISettingsService settingsService,
        IAuthService authService,
        INotificationService notificationService)
    {
        _settingsService = settingsService;
        _authService = authService;
        _notificationService = notificationService;

        Title = "Ayarlar & Profil";

        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = _settingsService.CurrentSettings;
        IsDarkMode = settings.IsDarkMode;
        CurrencySymbol = settings.CurrencySymbol;
        IsNotificationsEnabled = settings.IsNotificationsEnabled;
        IsSmsEnabled = settings.SmsNotificationEnabled;

        if (_authService.CurrentUser != null)
        {
            UserFullName = _authService.CurrentUser.FullName;
            UserPhone = _authService.CurrentUser.PhoneNumber;
        }
    }

    partial void OnIsDarkModeChanged(bool value)
    {
        _settingsService.SetTheme(value);
    }

    [RelayCommand]
    public void SelectCurrency(string currency)
    {
        CurrencySymbol = currency;
        _settingsService.SetCurrency(currency);
        _notificationService.ShowAlertAsync("Para Birimi Güncellendi", $"Varsayılan para birimi '{currency}' olarak ayarlandı.");
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        bool confirm = await _notificationService.ShowConfirmAsync(
            "Çıkış Yap",
            "Hesabınızdan çıkış yapmak istediğinize emin misiniz?",
            "Evet, Çıkış Yap",
            "İptal");

        if (confirm)
        {
            await _authService.LogoutAsync();
            await _notificationService.ShowAlertAsync("Çıkış Yapıldı", "Başarıyla çıkış yaptınız.");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

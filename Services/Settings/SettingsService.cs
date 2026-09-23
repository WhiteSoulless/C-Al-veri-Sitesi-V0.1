using SalesApp.Models.Settings;

namespace SalesApp.Services.Settings;

public class SettingsService : ISettingsService
{
    private const string KeyDarkMode = "setting_dark_mode";
    private const string KeyCurrency = "setting_currency";
    private const string KeyLanguage = "setting_language";
    private const string KeyNotifications = "setting_notifications";
    private const string KeySms = "setting_sms";

    private AppSettings _settings;

    public AppSettings CurrentSettings => _settings;

    public SettingsService()
    {
        _settings = new AppSettings
        {
            IsDarkMode = Preferences.Default.Get(KeyDarkMode, false),
            CurrencySymbol = Preferences.Default.Get(KeyCurrency, "₺"),
            Language = Preferences.Default.Get(KeyLanguage, "tr-TR"),
            IsNotificationsEnabled = Preferences.Default.Get(KeyNotifications, true),
            SmsNotificationEnabled = Preferences.Default.Get(KeySms, true)
        };

        ApplyTheme(_settings.IsDarkMode);
    }

    public void SaveSettings(AppSettings settings)
    {
        _settings = settings;
        Preferences.Default.Set(KeyDarkMode, settings.IsDarkMode);
        Preferences.Default.Set(KeyCurrency, settings.CurrencySymbol);
        Preferences.Default.Set(KeyLanguage, settings.Language);
        Preferences.Default.Set(KeyNotifications, settings.IsNotificationsEnabled);
        Preferences.Default.Set(KeySms, settings.SmsNotificationEnabled);

        ApplyTheme(settings.IsDarkMode);
    }

    public void SetTheme(bool isDark)
    {
        _settings.IsDarkMode = isDark;
        Preferences.Default.Set(KeyDarkMode, isDark);
        ApplyTheme(isDark);
    }

    public void SetCurrency(string currencySymbol)
    {
        _settings.CurrencySymbol = currencySymbol;
        Preferences.Default.Set(KeyCurrency, currencySymbol);
    }

    private void ApplyTheme(bool isDark)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
        }
    }
}

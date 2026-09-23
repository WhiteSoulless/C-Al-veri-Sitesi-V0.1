using SalesApp.Models.Settings;

namespace SalesApp.Services.Settings;

public interface ISettingsService
{
    AppSettings CurrentSettings { get; }
    void SaveSettings(AppSettings settings);
    void SetTheme(bool isDark);
    void SetCurrency(string currencySymbol);
}

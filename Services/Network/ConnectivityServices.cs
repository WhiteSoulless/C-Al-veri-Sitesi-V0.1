namespace SalesApp.Services.Network;

public class ConnectivityService : IConnectivityService
{
    // Cihazın internet erişimi var mı kontrol eder:
    public bool IsConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    public event EventHandler<bool>? ConnectivityChanged;

    public ConnectivityService()
    {
        // Cihazın bağlantı durumundaki değişiklikleri dinliyoruz:
        Connectivity.Current.ConnectivityChanged += OnNetworkStatusChanged;
    }

    private void OnNetworkStatusChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        bool isConnected = e.NetworkAccess == NetworkAccess.Internet;
        // Olayı dinleyen ekranlara haber veriyoruz:
        ConnectivityChanged?.Invoke(this, isConnected);
    }
}
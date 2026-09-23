using SalesApp.ViewModels.Auth;

namespace SalesApp.Views.Auth;

public partial class OtpPage : ContentPage
{
    public OtpPage(OtpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

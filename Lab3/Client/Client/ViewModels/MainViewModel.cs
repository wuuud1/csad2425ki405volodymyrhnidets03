using Client.Presentation.Services.Navigator;

namespace Client.Presentation.ViewModels;

public class MainViewModel : BaseViewModel
{
    public MainViewModel(INavigator navigator) : base(navigator)
    {
        navigator.NavigateTo<HomeViewModel>();
    }
}

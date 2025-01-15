using Client.Presentation.Services.Navigator;

namespace Client.Presentation.ViewModels;

/// <summary>
/// ViewModel for the main page, responsible for navigating to the Home page on initialization.
/// </summary>
public class MainViewModel : BaseViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="navigator">The navigator used for page navigation.</param>
    public MainViewModel(INavigator navigator) : base(navigator)
    {
        // Navigate to HomeViewModel as the initial page
        navigator.NavigateTo<HomeViewModel>();
    }
}

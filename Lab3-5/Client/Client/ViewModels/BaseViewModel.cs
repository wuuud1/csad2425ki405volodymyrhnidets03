using Client.Domain.Services;
using Client.Presentation.Services.Navigator;

namespace Client.Presentation.ViewModels;

/// <summary>
/// The base class for all view models in the application. It provides the core functionality for navigation and property change notification.
/// </summary>
public class BaseViewModel : ObservableObject
{
    private INavigator _navigator;

    /// <summary>
    /// Gets the navigator instance used for navigating between views.
    /// </summary>
    public INavigator Navigator
    {
        get => _navigator;
        private set
        {
            _navigator = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
    /// </summary>
    /// <param name="navigator">The navigator instance used for view navigation.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="navigator"/> is null.</exception>
    public BaseViewModel(INavigator navigator)
    {
        Navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
    }
}


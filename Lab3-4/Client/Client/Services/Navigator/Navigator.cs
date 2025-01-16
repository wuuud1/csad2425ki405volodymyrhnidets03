using Client.Domain.Services;
using Client.Presentation.ViewModels;

namespace Client.Presentation.Services.Navigator;

/// <summary>
/// The <see cref="Navigator"/> class provides navigation functionality by managing the current view
/// and allowing navigation to different view models.
/// </summary>
public class Navigator : ObservableObject, INavigator
{
    private readonly Func<Type, BaseViewModel> _viewModelFactory;
    private BaseViewModel _currentView;

    /// <summary>
    /// Gets or sets the current view that is being displayed.
    /// </summary>
    public BaseViewModel CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Navigator"/> class.
    /// </summary>
    /// <param name="viewModelFactory">A function that creates a new instance of a view model 
    /// based on its type.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="viewModelFactory"/> is null.</exception>
    public Navigator(Func<Type, BaseViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
    }

    /// <summary>
    /// Navigates to a new view model of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the view model to navigate to.</typeparam>
    public void NavigateTo<T>() where T : BaseViewModel
    {
        BaseViewModel viewModelBase = _viewModelFactory.Invoke(typeof(T));
        CurrentView = viewModelBase;
    }
}

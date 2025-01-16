using Client.Domain.Services;
using Client.Presentation.ViewModels;

namespace Client.Presentation.Services.Navigator;

public class Navigator : ObservableObject, INavigator
{
    private readonly Func<Type, BaseViewModel> _viewModelFactory;
    private BaseViewModel _currentView;

    public BaseViewModel CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }

    public Navigator(Func<Type, BaseViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
    }

    public void NavigateTo<T>() where T : BaseViewModel
    {
        BaseViewModel viewModelBase = _viewModelFactory.Invoke(typeof(T));
        CurrentView = viewModelBase;
    }
}

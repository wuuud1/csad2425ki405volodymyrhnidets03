using Client.Domain.Services;
using Client.Presentation.Services.Navigator;

namespace Client.Presentation.ViewModels;

public class BaseViewModel : ObservableObject
{
    private INavigator _navigator;
    public INavigator Navigator
    {
        get => _navigator;
        private set
        {
            _navigator = value;
            OnPropertyChanged();
        }
    }

    public BaseViewModel(INavigator navigator)
    {
        Navigator = navigator ?? throw new ArgumentNullException("navigator");
    }
}

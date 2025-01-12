using Client.Presentation.ViewModels;

namespace Client.Presentation.Services.Navigator;

public interface INavigator
{
    BaseViewModel CurrentView { get; set; }

    void NavigateTo<T>() where T : BaseViewModel;
}
